﻿namespace PetFamily.Infrastructure.DbContext.PostgresSQL;

public sealed record ApplicationDbContextOptions
{
    public const string CONNECTIONSTRING_SECTION_FOR_POSTGRESSQL = "Psql_Connection_String";
    public const string CONNECTIONSTRING_FOR_POSTGRESSQL = "Postgres_SQL";
}