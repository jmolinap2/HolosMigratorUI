# HolosMigratorWpf

Clon WPF/XAML de `HolosMigratorUI` inspirado en el look de `Win11Debloat`.

## Ejecutar

Desde la raiz del repo:

```powershell
.\RunWpf.bat
```

O directamente:

```powershell
dotnet run --project .\WpfClone\HolosMigratorWpf.csproj
```

## Incluye

- Navegacion lateral: Operations, Dashboard, Storage, Log Center y Settings.
- Tema oscuro con tarjetas, bordes finos y boton principal tipo Win11Debloat.
- Lectura de `.env` del repo padre.
- Ejecucion de `deploy-hostinger.ps1` y `migrate-hostinger-ui.ps1`.
- Captura de salida en tiempo real, progreso y log local.
- Dashboard/Storage/Logs usando el `Core` existente.

El WinForms original queda intacto.
