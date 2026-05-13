FROM mcr.microsoft.com/mssql/server:2022-latest

USER root

ENV ACCEPT_EULA=Y

COPY init.sh /init.sh
COPY Commit/001/All/*.sql /scripts/

RUN sed -i 's/\r//' /init.sh && chmod +x /init.sh

USER mssql

ENTRYPOINT ["/bin/bash", "/init.sh"]
