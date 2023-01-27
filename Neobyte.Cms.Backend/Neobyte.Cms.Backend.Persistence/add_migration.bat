echo off
dotnet tool install --global dotnet-ef
set /p migration_name="Migration name (PascalCase): "
echo Adding migration %migration_name%...

dotnet ef migrations add %migration_name% --startup-project .\..\Neobyte.Cms.Backend\