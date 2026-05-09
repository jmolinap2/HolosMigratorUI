# Plan maestro de evolucion - HolosMigratorUI

## 1) Objetivo general
Transformar Holos Migrator de una sola pantalla operativa a un minisistema modular de control, monitoreo y operacion de VPS, manteniendo la pantalla actual intacta como modulo principal de ejecucion.

## 2) Contexto y hallazgos iniciales
A partir del log revisado:
- El migrator finaliza correctamente.
- Se observa salida con prefijo ERR en eventos de Docker Compose que no siempre implican fallo.
- Se imprime en log la cadena de conexion completa con credenciales.
- En la cadena aparece Encrypt=False y TrustServerCertificate=True.

Conclusiones operativas:
- El problema principal inmediato no es que el proceso falle, sino la exposicion de secretos en logs.
- El manejo de cifrado de conexion requiere politica por entorno y validaciones explicitas.

## 3) Resultado esperado del producto
Un panel de control WinForms con:
- Navegacion lateral por modulos.
- Dashboard con estado del sistema.
- Centro de logs con busqueda y filtros.
- Pantalla de ejecucion actual preservada.
- Indicadores utiles para operacion diaria en VPS sin interfaz grafica remota.

## 4) Principios de diseno
- No romper flujo actual de migracion/despliegue.
- Seguridad primero: no exponer secretos.
- Observabilidad accionable: estado, errores, tiempos y trazabilidad.
- Arquitectura modular para crecer sin rehacer Form1 completo.
- UX clara y visual para operacion rapida.

## 5) Alcance funcional por fases

### Fase 0 - Hardening inmediato (seguridad y log)
Objetivo: eliminar riesgo de exposicion de credenciales y normalizar entorno.

Entregables:
- Redaccion de secretos en logs (passwords, tokens, connection strings).
- Politica de mascarado en salida UI y archivo.
- Validator de entorno (Development, Staging, Production) con comportamiento explicito.
- Reglas de conexion por entorno:
  - Produccion: Encrypt=True, TrustServerCertificate=False (o excepcion controlada).
  - Entornos internos controlados: excepcion permitida y documentada, nunca logueada en texto plano.
- Indicador visual en UI: Entorno activo + nivel de riesgo.

Criterios de aceptacion:
- No aparece ningun password/token en logs.
- No aparece cadena completa de conexion en logs.
- El usuario ve entorno activo y validacion antes de ejecutar.

### Fase 1 - Shell de navegacion (multi pantalla)
Objetivo: convertir la app en contenedor modular.

Entregables:
- Estructura de app con contenedor principal y menu lateral.
- Modulos iniciales:
  - Operaciones (pantalla actual, sin perdida de funciones).
  - Dashboard.
  - Logs.
  - Configuracion.
  - Herramientas.
- Sistema de carga por UserControl para cada modulo.
- Header comun con estado global y acciones rapidas.

Criterios de aceptacion:
- Se puede navegar entre modulos sin reiniciar.
- El modulo Operaciones conserva comportamiento actual.

### Fase 2 - Dashboard operativo y monitoreo visual
Objetivo: dar visibilidad inmediata del estado del VPS y servicios.

Entregables:
- Tarjetas de estado:
  - VPS reachable / no reachable.
  - SQL container status.
  - API container status.
  - Front container status.
  - Ultimo deploy y ultimo migrator run.
- Semaforos por estado (OK, Warning, Error).
- Indicadores de tendencia:
  - Ultimos N runs (duracion, exito/fallo).
  - Tiempo desde ultimo exito.
- Panel de acciones rapidas:
  - Ejecutar checks.
  - Abrir logs.
  - Reiniciar servicios clave (si aplica).

Criterios de aceptacion:
- El operador identifica salud general en menos de 10 segundos.
- Los estados del dashboard coinciden con comandos reales del servidor.

### Fase 3 - Centro de logs profesional
Objetivo: convertir logs en herramienta de diagnostico util.

Entregables:
- Visor de logs con tabs:
  - Log local UI.
  - Log remoto migrator.
  - Logs de servicios (API, Front, SQL) bajo demanda.
- Filtros por severidad, fecha, modulo y texto.
- Resaltado semantico de eventos:
  - Error real.
  - Warning.
  - Evento operativo normal por stderr.
- Exportacion de segmento filtrado.
- Descarga rapida a carpeta seleccionada.

Criterios de aceptacion:
- Se puede aislar un incidente en menos de 2 minutos.
- Se reduce falso positivo de lineas ERR no criticas.

### Fase 4 - Configuracion y perfiles de operacion
Objetivo: simplificar uso repetitivo y reducir errores manuales.

Entregables:
- Perfil por entorno (Dev, Staging, Prod).
- Presets por objetivo (Deploy completo, solo migraciones, solo checks).
- Campos sensibles protegidos con cifrado local (DPAPI u opcion equivalente).
- Validaciones previas a ejecucion con checklist visual.

Criterios de aceptacion:
- Se puede ejecutar un flujo completo con 1 a 2 clics.
- Los valores criticos no se guardan en texto plano.

### Fase 5 - Alertas e indicadores avanzados
Objetivo: operacion proactiva y no solo reactiva.

Entregables:
- Reglas de alerta:
  - Fallo consecutivo de migrator.
  - API no responde en health check.
  - SQL caido.
- Centro de eventos recientes.
- Indicadores KPI:
  - Exito de despliegues (7/30 dias).
  - MTTR basico (tiempo medio de recuperacion).
  - Duracion promedio de run.

Criterios de aceptacion:
- El sistema alerta antes de que el operador detecte por inspeccion manual.

## 6) Arquitectura tecnica objetivo

### Estructura de UI propuesta
- MainShellForm
  - SidebarMenuControl
  - TopStatusBarControl
  - ContentHostPanel
- Modulos (UserControls):
  - OperationsModule (conteniendo la vista actual)
  - DashboardModule
  - LogsModule
  - SettingsModule
  - ToolsModule

### Servicios internos
- ProcessExecutionService
- RemoteCommandService
- LogIngestionService
- LogSanitizerService
- HealthCheckService
- SettingsSecurityService

### Modelos base
- RunSummary
- ServiceHealth
- EnvironmentProfile
- LogEntryNormalized

## 7) Seguridad y compliance operativa
- Nunca registrar secretos en texto plano.
- Sanitizacion obligatoria antes de mostrar/guardar log.
- Reglas explicitas para conexion SQL segun entorno.
- Registro de auditoria de ejecuciones sin exponer credenciales.

## 8) Plan de implementacion incremental

Iteracion A (rapida, riesgo alto):
- Fase 0 completa.
- Preparacion de shell de navegacion (base de Fase 1).

Iteracion B (valor visible inmediato):
- Fase 1 completa.
- Dashboard minimo viable de Fase 2.

Iteracion C (operacion diaria robusta):
- Fase 3 completa.
- Presets principales de Fase 4.

Iteracion D (madurez):
- Fase 4 restante.
- Fase 5 completa.

## 9) Criterios de calidad
- Sin regresion en ejecucion actual.
- Tiempos de respuesta UI adecuados durante procesos largos.
- Manejo de errores consistente y comprensible.
- Cobertura minima de pruebas para parser de logs y sanitizacion.

## 10) Riesgos y mitigaciones
- Riesgo: acoplar demasiado nueva UI con Form1 actual.
  - Mitigacion: extraer servicios y desacoplar logica de presentacion.

- Riesgo: falsos positivos al interpretar ERR de stderr.
  - Mitigacion: clasificador por contexto (Docker event vs error real).

- Riesgo: degradacion UX por exceso de datos.
  - Mitigacion: dashboard por capas (resumen + detalle bajo demanda).

## 11) Definicion de terminado (DoD)
- Modulo Operaciones actual preservado y funcional.
- Menu lateral funcional con al menos 5 modulos.
- Dashboard con estado en vivo y ultima ejecucion.
- Centro de logs con filtros y exportacion.
- Secretos protegidos y no visibles en logs.
- Documento tecnico y guia de uso actualizados.

## 12) Orden de ejecucion recomendado
1. Seguridad de logs y secretos (Fase 0).
2. Navegacion modular sin romper flujo actual (Fase 1).
3. Dashboard operativo minimo viable (Fase 2).
4. Centro de logs util para diagnostico (Fase 3).
5. Presets y perfiles por entorno (Fase 4).
6. Alertas y KPI (Fase 5).
