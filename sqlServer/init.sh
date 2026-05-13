#!/bin/bash

SA_PASSWORD="${SA_PASSWORD}"
DB_NAME="PruebaTecnica"
SCRIPTS_DIR="/scripts"

/opt/mssql/bin/sqlservr &
MSSQL_PID=$!

echo "Esperando que SQL Server inicie..."
for i in $(seq 1 30); do
    /opt/mssql-tools18/bin/sqlcmd \
        -S localhost \
        -U sa \
        -P "$SA_PASSWORD" \
        -Q "SELECT 1" \
        -No \
        > /dev/null 2>&1
    if [ $? -eq 0 ]; then
        echo "SQL Server listo."
        break
    fi
    echo "  Intento $i/30 - esperando 2s..."
    sleep 2
done

echo "Creando base de datos $DB_NAME (si no existe)..."
/opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "$SA_PASSWORD" \
    -No \
    -Q "IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'$DB_NAME') CREATE DATABASE [$DB_NAME];"

echo "Ejecutando scripts de inicializacion..."
for SCRIPT in $(ls "$SCRIPTS_DIR"/*.sql | sort); do
    echo "  -> $SCRIPT"
    /opt/mssql-tools18/bin/sqlcmd \
        -S localhost \
        -U sa \
        -P "$SA_PASSWORD" \
        -d "$DB_NAME" \
        -No \
        -i "$SCRIPT"
done

echo "Inicializacion completada."

wait $MSSQL_PID
