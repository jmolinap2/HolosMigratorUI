# Acceso al VPS y estado del deploy

> Este documento registra qué pasó, qué se corrigió y qué falta para que el deploy
> vía `HolosMigratorUI` funcione. Ningún secreto (password, token, contenido de
> llave privada) se escribe aquí — solo dónde vive cada uno.

## Contexto: reinstalación del VPS (2026-09-12)

El VPS de Hostinger (`srv1975063.hstgr.cloud`) fue reinstalado con Ubuntu 26.04 LTS.
Eso invalidó todo lo que `HolosMigratorUI/.env` tenía guardado:

- **IP cambió**: de `76.13.37.115` a **`187.7.24.119`**.
- **Host key SSH cambió** (normal tras reinstalar).
- **`authorized_keys` se vació** — ninguna llave vieja seguía autorizada.
- **La contraseña root cambió** (la guardada en `.env` ya no servía).
- **El repo `/root/OmniSuite` ya no existía** en el servidor (reinstalación = disco limpio).

## Qué se corrigió en `HolosMigratorUI/.env`

| Variable | Antes | Ahora |
|---|---|---|
| `SERVER_HOST` | `76.13.37.115` (IP vieja) | `187.7.24.119` |
| `REPO_LOCAL` | `C:\Repos\OmniSuite` (no existe) | `C:\Repos\HolosCoreOps` (mismo repo, carpeta renombrada) |
| `SSH_KEY_PATH` | `~/.ssh/id_ed25519` (con passphrase — no sirve para automatización) | `C:\Repos\Secretos\VPS Linux\holoscoreops_vps_ed25519` (sin passphrase, dedicada) |
| `SSH_PASSWORD` | contraseña vieja, ya no válida | vacía — se usa solo la llave |
| `GIT_TOKEN` | token vencido (GitHub daba 401) | token nuevo, generado 2026-09-12, **vence 2027-01-02**. Scope: `repo`. Copia de respaldo en `C:\Repos\Secretos\VPS Linux\secreto git.txt` |

## Llave SSH: cuál usar y por qué

En `C:\Repos\Secretos\VPS Linux\` puede haber varios archivos de llave. La única
que está autorizada en el servidor **y** funciona de forma no interactiva (sin pedir
passphrase) es:

```
C:\Repos\Secretos\VPS Linux\holoscoreops_vps_ed25519       (privada, SIN passphrase)
C:\Repos\Secretos\VPS Linux\holoscoreops_vps_ed25519.pub    (pública)
```

Los archivos `certificados` / `certificados.pub` en esa misma carpeta son restos de
un intento anterior — no corresponden a ninguna llave autorizada en el servidor
actualmente. Se pueden borrar cuando se confirme que ya no hacen falta.

El `~/.ssh/authorized_keys` del servidor terminó con **3 entradas** (una es la llave
dedicada de arriba; las otras dos son de intentos previos, con el mismo comentario
`jmolinap95@gmail.com` pero contenido distinto entre sí). No estorban, pero se
pueden limpiar a mano si se quiere dejar solo la dedicada.

### Importante: en la app, seleccionar "Modo SSH: Key" a mano

`HolosMigratorUI` guarda la contraseña SSH cifrada en
`%AppData%\HolosMigratorUI\settings.json` si estaba marcado "Recordar clave SSH".
Vaciar `SSH_PASSWORD` en `.env` **no** borra eso — el campo de password en la UI
puede seguir mostrando la contraseña vieja al abrir la app. Para evitar que la app
intente usarla, elegir explícitamente **Modo SSH → Key** en el desplegable (no
dejar "Auto") antes de ejecutar.

## Estado del servidor (verificado 2026-09-12)

- SSH por llave: **funciona** (`ssh -i ".../holoscoreops_vps_ed25519" root@187.7.24.119`).
- `git`, `docker`, `docker compose`: **ya instalados** en el VPS.
- Repo clonado en `/root/OmniSuite`, rama activa: `develop`.
- **Sin contenedores ni volúmenes previos** — VPS completamente limpio, no hay
  datos de producción que se puedan perder con la primera corrida del deploy.

## Ramas del repo: ojo con esto

- **`master`** (rama principal en GitHub) **no tiene** `scripts/deploy-hostinger.ps1`
  ni ningún `docker-compose*.yml` — solo trae `scripts/start-dev.ps1`. No sirve como
  rama de deploy con la herramienta actual.
- **`develop`** sí tiene todo: los 4 `docker-compose*.yml` y todos los scripts de
  `scripts/`. Ya incluye el merge de `migration/postgresql` (PR #3). Es la rama que
  usa `deploy-hostinger.ps1` por defecto.

## Pendiente: elegir motor de base de datos

`develop` soporta **ambos** motores automáticamente (el código detecta el tipo de
connection string). Hay compose para los dos:

- `docker-compose.hostinger.yml` → SQL Server (como antes de la reinstalación).
- `docker-compose.postgresql.production.yml` → PostgreSQL, incluye un servicio
  `holos-worker` adicional que no existe en la versión SQL Server.

Como el VPS está limpio, no hay costo de migración por elegir uno u otro — es
puramente una decisión de qué motor usar de ahora en adelante. **Decisión pendiente
del usuario.**

## Siguiente paso una vez decidido el motor

Desde `HolosMigratorUI`:

1. Modo SSH: **Key**.
2. Repo local: `C:\Repos\HolosCoreOps` (ya debería cargar solo).
3. Branch: `develop`.
4. Compose file: `docker-compose.hostinger.yml` o `docker-compose.postgresql.production.yml`
   según lo decidido (el campo de compose es editable en Opciones avanzadas).
5. Ejecutar.
