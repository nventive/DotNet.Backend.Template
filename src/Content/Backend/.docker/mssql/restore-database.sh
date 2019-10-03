#!/bin/bash

_DQCHR='"'

echoLog() {
    echo "$(date --utc '+%Y-%m-%d %H:%M:%S.%2N') dock-start  $@"
}

echoErr() {
    >&2 echo "$(date --utc '+%Y-%m-%d %H:%M:%S.%2N') dock-start  $@"
}

mssqlWait() {
    while true; do
        /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -Q "SELECT GETDATE()" > /dev/null 2>&1;
        
        if [ $? -eq 0 ]; then
            break;
        fi;
    
        echoLog "SQL Server not ready, retrying in 5s..."
        sleep 5;
    done;
}

if [ ! -f "/var/opt/mssql/data/${DATABASE_NAME}.mdf" ]; then
    mssqlWait;
    echoLog "Creating ${DATABASE_NAME}..."
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -Q "CREATE DATABASE [${DATABASE_NAME}]";
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -d $DATABASE_NAME -Q "CREATE LOGIN $DATABASE_USER WITH PASSWORD = '$DATABASE_PASS'; CREATE USER $DATABASE_USER FOR LOGIN $DATABASE_USER; EXEC sp_addrolemember 'db_owner', '$DATABASE_USER'";
    echoLog "Database ${DATABASE_NAME} created."
fi

if [ ! -d "/opt/mssql-custom/to-restore" ]; then
    exit 0;
fi

find /opt/mssql-custom/to-restore \( -name "*.bak" -o -name "*.trn" \) | while read _BACKUP_FILE; do
    _RESTORED_DB_NAME=$(basename "${_BACKUP_FILE%.*}")
    
    if [ -f "/var/opt/mssql/data/${_RESTORED_DB_NAME}.mdf" ]; then
        continue;
    fi
    
    mssqlWait;
    echoLog "Restoring $_RESTORED_DB_NAME from backup..."

    # Read file metadata and see if a FULL Database Backup (BackupType = 1, RecoveryMode = FULL) is available
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -s , -W -Q "SET NOCOUNT ON; RESTORE HEADERONLY FROM DISK = '$_BACKUP_FILE'" | grep -E -v "^[-,]+$" > /tmp/mssqlrestore.txt;
    
    _MSSQL_EXIT_CODE=$?
    
    if [ "$_MSSQL_EXIT_CODE" -gt 0 ]; then
        echoErr "$_BACKUP_FILE is not a valid backup file."
        continue;
    fi
    
    _BACKUP_TYPE_COL=$(head -1 /tmp/mssqlrestore.txt | tr ',' '\n' | cat -n | grep -E "\s+BackupType$" | awk '{print $1}')
    _RECOVERY_MODEL_COL=$(head -1 /tmp/mssqlrestore.txt | tr ',' '\n' | cat -n | grep -E "\s+RecoveryModel$" | awk '{print $1}')
    _POSITION_COL=$(head -1 /tmp/mssqlrestore.txt | tr ',' '\n' | cat -n | grep -E "\s+Position$" | awk '{print $1}')
    _BACKUP_FILE_INDEX=$(cat /tmp/mssqlrestore.txt | awk -F , "{OFS = ${_DQCHR},${_DQCHR}; print \$${_POSITION_COL},\$${_BACKUP_TYPE_COL},\$${_RECOVERY_MODEL_COL}}" | grep -E ",1,FULL$" | head -1 | awk -F , '{print $1}')

    # If no position is returned, the database backup cannot be restored
    if [ -z "$_BACKUP_FILE_INDEX" ]; then
        echoErr "$_BACKUP_FILE does not contain a FULL Database Backup (BackupType = 1, RecoveryMode = FULL). Skipping."
        continue;
    fi
    
    # Read the filelist included with the backup to retrieve the data file and log file name (for restore).
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -s , -W -Q "SET NOCOUNT ON; RESTORE FILELISTONLY FROM DISK = '$_BACKUP_FILE' WITH FILE = ${_BACKUP_FILE_INDEX}" | grep -E -v "^[-,]+$" > /tmp/mssqlrestore.txt;
    
    _LOGICAL_NAME_COL=$(head -1 /tmp/mssqlrestore.txt | tr ',' '\n' | cat -n | grep -E "\s+LogicalName$" | awk '{print $1}')
    _TYPE_COL=$(head -1 /tmp/mssqlrestore.txt | tr ',' '\n' | cat -n | grep -E "\s+Type$" | awk '{print $1}')
    
    _DATA_FILE_NAME=$(cat /tmp/mssqlrestore.txt | awk -F , "{OFS = ${_DQCHR},${_DQCHR}; print \$${_LOGICAL_NAME_COL},\$${_TYPE_COL}}" | grep -E ",D$" | head -1 | awk -F , '{print $1}')
    _LOG_FILE_NAME=$(cat /tmp/mssqlrestore.txt | awk -F , "{OFS = ${_DQCHR},${_DQCHR}; print \$${_LOGICAL_NAME_COL},\$${_TYPE_COL}}" | grep -E ",L$" | head -1 | awk -F , '{print $1}')
    
    # Restore the database
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -Q "RESTORE DATABASE [${_RESTORED_DB_NAME}] FROM DISK = N'$_BACKUP_FILE' WITH FILE = ${_BACKUP_FILE_INDEX}, MOVE N'${_DATA_FILE_NAME}' TO '/var/opt/mssql/data/${_RESTORED_DB_NAME}.mdf', MOVE N'${_LOG_FILE_NAME}' TO '/var/opt/mssql/data/${_RESTORED_DB_NAME}.ldf'";

    # Ensure database is set to RECOVERY MODE SIMPLE
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -Q "ALTER DATABASE [${_RESTORED_DB_NAME}] SET RECOVERY SIMPLE WITH NO_WAIT";
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -Q "BACKUP DATABASE [${_RESTORED_DB_NAME}] TO DISK = N'/tmp/backup.bak'";
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -d $_RESTORED_DB_NAME -Q "DBCC SHRINKFILE([${_LOG_FILE_NAME}])";
    rm -rf /tmp/backup.bak;

    # Give access to $DATABASE_USER
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -d $_RESTORED_DB_NAME -Q "DROP USER IF EXISTS $DATABASE_USER";
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -d $_RESTORED_DB_NAME -Q "CREATE USER $DATABASE_USER FOR LOGIN $DATABASE_USER; EXEC sp_addrolemember 'db_owner', '$DATABASE_USER'";
    
    echoLog "Restored $_RESTORED_DB_NAME from backup."
done;

if [ -f "/tmp/mssqlrestore.txt" ]; then
    rm -f /tmp/mssqlrestore.txt
fi

exit 0;
