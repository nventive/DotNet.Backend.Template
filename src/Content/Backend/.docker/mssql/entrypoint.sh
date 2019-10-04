#!/bin/bash
/opt/mssql/bin/sqlservr & /opt/mssql-custom/restore-database.sh

while true; do
	SERVERPID=$(pgrep -xo sqlservr);

	if [ $? -ne 0 ]; then
		break;
	fi;

	wait $SERVERPID 2>&1;

	if [ $? -ne 0 ]; then
		break;
	fi;
done;
