@echo off

call dotnet build -c Release

call start cmd.exe /k "dotnet run -c Release --project .\src\DeepBlue.Blazor"
call start cmd.exe /k "dapr run --app-id gateway --resources-path .\src\Componants --app-port 30001 -- dotnet run -c Release --project .\src\api\DeepBlue.Api.Gateway"
call start cmd.exe /k "dapr run --app-id move-validator --resources-path .\src\Componants --app-port 30002 -- dotnet run -c Release --project .\src\api\DeepBlue.Api.MoveValidator"