#!/bin/bash
set -e

POSTGRES="psql --username ${POSTGRES_USER}"

echo "Creating SolarDev SQL database:"

$POSTGRES <<EOSQL
CREATE DATABASE ${POSTGRES_DATABASE} OWNER ${POSTGRES_USER};
EOSQL
