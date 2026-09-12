@echo off
setlocal

set "ROOT=%~dp0"
set "PROJECT=%ROOT%WpfClone\HolosMigratorWpf.csproj"

dotnet run --project "%PROJECT%"
