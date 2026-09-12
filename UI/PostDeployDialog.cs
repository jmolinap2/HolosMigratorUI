using HolosMigratorUI.Core;

namespace HolosMigratorUI.UI;

/// <summary>
/// Modal de verificacion posterior al deploy. A diferencia del pre-vuelo, esto corre
/// DESPUES de que el script termino (incluso si reporto exito) porque el script no
/// valida codigos HTTP reales ni detecta contenedores en crash-loop.
/// </summary>
public sealed class PostDeployDialog : Form
{
    private readonly Func<Task<IReadOnlyList<HealthCheckService.PreflightCheck>?>> _runChecks;
    private readonly FlowLayoutPanel _rows = new() { Dock = DockStyle.Top, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true, Padding = new Padding(20, 16, 20, 8) };
    private readonly Label _title = new()
    {
        Text = "VERIFICACIÓN POST-DEPLOY",
        Dock = DockStyle.Top,
        Height = 34,
        Font = new Font("Consolas", 12, FontStyle.Bold),
        ForeColor = Color.FromArgb(0, 255, 255),
        Padding = new Padding(20, 10, 0, 0)
    };
    private readonly Label _spinner = new()
    {
        Text = "Verificando contenedores y respuestas reales del servidor...",
        Dock = DockStyle.Top,
        Height = 30,
        ForeColor = Color.FromArgb(0, 255, 65),
        Font = new Font("Consolas", 9.5F),
        Padding = new Padding(20, 0, 0, 0)
    };
    private readonly Panel _footer = new() { Dock = DockStyle.Bottom, Height = 54 };

    public PostDeployDialog(Func<Task<IReadOnlyList<HealthCheckService.PreflightCheck>?>> runChecks)
    {
        _runChecks = runChecks;

        Text = "Holos Migrator";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        Width = 600;
        Height = 260;
        BackColor = Color.FromArgb(9, 9, 11);
        ForeColor = Color.FromArgb(0, 255, 65);

        var btnClose = new Button
        {
            Text = "Cerrar",
            FlatStyle = FlatStyle.Flat,
            Width = 110,
            Height = 34,
            Location = new Point(20, 10),
            ForeColor = Color.FromArgb(200, 200, 200),
            BackColor = Color.FromArgb(24, 24, 30)
        };
        btnClose.Click += (_, _) => Close();
        _footer.Controls.Add(btnClose);

        Controls.Add(_rows);
        Controls.Add(_spinner);
        Controls.Add(_footer);
        Controls.Add(_title);

        Shown += async (_, _) => await RunAsync();
    }

    private async Task RunAsync()
    {
        var results = await _runChecks();

        if (results == null)
        {
            _spinner.Text = "✗ No se pudo conectar por SSH para verificar el resultado del deploy.";
            _spinner.ForeColor = Color.FromArgb(255, 85, 85);
            return;
        }

        _spinner.Visible = false;
        Height = 260 + (results.Count * 30);

        var anyFailed = false;
        foreach (var check in results)
        {
            AddRow(check);
            if (!check.Ok) anyFailed = true;
        }

        _title.Text = anyFailed
            ? "VERIFICACIÓN POST-DEPLOY — SE ENCONTRARON PROBLEMAS"
            : "VERIFICACIÓN POST-DEPLOY — TODO SANO";
        _title.ForeColor = anyFailed ? Color.FromArgb(255, 85, 85) : Color.FromArgb(0, 255, 65);
    }

    private void AddRow(HealthCheckService.PreflightCheck check)
    {
        var color = check.Ok ? Color.FromArgb(0, 255, 65) : Color.FromArgb(255, 85, 85);
        var icon = check.Ok ? "✓" : "✗";

        var line = new Label
        {
            Text = $"{icon} {check.Label}",
            AutoSize = true,
            ForeColor = color,
            Font = new Font("Consolas", 9.5F)
        };
        _rows.Controls.Add(line);

        if (!check.Ok && !string.IsNullOrWhiteSpace(check.Detail))
        {
            var detail = new Label
            {
                Text = $"    {check.Detail}",
                AutoSize = true,
                ForeColor = Color.FromArgb(200, 130, 60),
                Font = new Font("Consolas", 8.5F),
                Margin = new Padding(0, 0, 0, 6)
            };
            _rows.Controls.Add(detail);
        }
    }
}
