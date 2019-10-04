FROM mcr.microsoft.com/mssql/server:2017-latest-ubuntu

RUN mkdir -p /opt/mssql-custom

COPY entrypoint.sh /opt/mssql-custom/entrypoint.sh
COPY restore-database.sh /opt/mssql-custom/restore-database.sh

RUN chmod +x /opt/mssql-custom/entrypoint.sh
RUN chmod +x /opt/mssql-custom/restore-database.sh
CMD /bin/bash /opt/mssql-custom/entrypoint.sh
