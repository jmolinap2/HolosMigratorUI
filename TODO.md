- [ ] Añadir UI en `MainShellForm` para: checkbox “Programar inicio” y opciones “30 min” / “1 hora”.
- [ ] Extender `UiSettings` para persistir si el temporizador está activo y la duración elegida.
- [ ] Implementar lógica en `RunSelectedActionAsync()` para programar el arranque y actualizar UI/labels durante la espera.
- [ ] Agregar cancelación en `StopCurrentProcess()` para cancelar el temporizador si aún no inició.

- [ ] Asegurar que el flujo original (sin temporizador) no cambie.
- [ ] Compilar y validar comportamiento (inicio inmediato, inicio programado, Stop durante espera y durante ejecución).

