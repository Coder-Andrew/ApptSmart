### Useful Commands

# Scaffold app DB to reflect new models
`dotnet ef dbcontext scaffold Name=AppConnection Microsoft.EntityFrameworkCore.SqlServer -o Models/AppModels --context AppDbContext --context-dir Models --use-database-names --force --no-onconfiguring`

# Drop Auth Db
`dotnet ef database drop --context AuthDbContext`

# Create Migration for Auth DB
`dotnet ef migrations add {Migration Name} --context AuthDbContext` 

# Apply migrations to Auth DB
`dotnet ef database update --context AuthDbContext`


Left off changing First and Last name to app db instead of auth db. Need to update seeder and auth service.