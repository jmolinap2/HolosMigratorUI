using HolosMigratorUI.Core;

namespace HolosMigratorUI.UI;

public enum PreflightOutcome
{
    Cancelled,
    Proceed,
    GoToSettings
}

/// <summary>
/// Modal de verificacion previa al deploy. Corre los checks al mostrarse; si todos pasan
/// se auto-cierra y deja continuar solo. Si algo falla, se queda abierto con las opciones
/// de ir a corregirlo, forzar el deploy de todas formas, o cancelar.
/// </summary>
public sealed class PreflightDialog : Form
{
    private readonly Func<Task<IReadOnlyList<HealthCheckService.PreflightCheck>?>> _runChecks;
    private readonly FlowLayoutPanel _rows = new() { Dock = DockStyle.Top, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true, Padding = new Padding(20, 16, 20, 8) };
    private readonly Label _title = new()
    {
        Text = "VERIFICACIÓN PRE-VUELO",
        Dock = DockStyle.Top,
        Height = 34,
        Font = new Font("Consolas", 12, FontStyle.Bold),
        ForeColor = Color.FromArgb(0, 255, 255),
        Padding = new Padding(20, 10, 0, 0)
    };
    private readonly Label _spinner = new()
    {
        Text = "Verificando conexión SSH, herramientas remotas y configuración...",
        Dock = DockStyle.Top,
        Height = 30,
        ForeColor = Color.FromArgb(0, 255, 65),
        Font = new Font("Consolas", 9.5F),
        Padding = new Padding(20, 0, 0, 0)
    };
    private readonly Panel _footer = new() { Dock = DockStyle.Bottom, Height = 54, Visible = false };

    public PreflightOutcome Outcome { get; private set; } = PreflightOutcome.Cancelled;

    public PreflightDialog(Func<Task<IReadOnlyList<HealthCheckService.PreflightCheck>?>> runChecks)
    {
        _runChecks = runChecks;

        Text = "Holos Migrator";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        Width = 560;
        Height = 260;
        BackColor = Color.FromArgb(9, 9, 11);
        ForeColor = Color.FromArgb(0, 255, 65);

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
            _spinner.Text = "✗ No se pudo conectar/autenticar por SSH.";
            _spinner.ForeColor = Color.FromArgb(255, 85, 85);
            ShowFooter(failed: true, connectionFailed: true);
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

        if (!anyFailed)
        {
            await Task.Delay(700);
            Outcome = PreflightOutcome.Proceed;
            DialogResult = DialogResult.OK;
            Close();
            return;
        }

        ShowFooter(failed: true, connectionFailed: false);
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

    private void ShowFooter(bool failed, bool connectionFailed)
    {
        if (!failed)
        {
            return;
        }

        _footer.Controls.Clear();
        _footer.Visible = true;

        var btnCancel = new Button
        {
            Text = "Cancelar",
            FlatStyle = FlatStyle.Flat,
            Width = 110,
            Height = 34,
            Location = new Point(20, 10),
            ForeColor = Color.FromArgb(200, 200, 200),
            BackColor = Color.FromArgb(24, 24, 30)
        };
        btnCancel.Click += (_, _) => { Outcome = PreflightOutcome.Cancelled; DialogResult = DialogResult.Cancel; Close(); };
        _footer.Controls.Add(btnCancel);

        if (!connectionFailed)
        {
            var btnSettings = new Button
            {
                Text = "Ir a Settings",
                FlatStyle = FlatStyle.Flat,
                Width = 140,
                Height = 34,
                Location = new Point(140, 10),
                ForeColor = Color.FromArgb(150, 237, 255),
                BackColor = Color.FromArgb(19, 47, 71)
            };
            btnSettings.Click += (_, _) => { Outcome = PreflightOutcome.GoToSettings; DialogResult = DialogResult.Cancel; Close(); };
            _footer.Controls.Add(btnSettings);

            var btnForce = new Button
            {
                Text = "Ejecutar de todas formas",
                FlatStyle = FlatStyle.Flat,
                Width = 210,
                Height = 34,
                Location = new Point(290, 10),
                ForeColor = Color.FromArgb(10, 20, 14),
                BackColor = Color.FromArgb(255, 85, 85),
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold)
            };
            btnForce.Click += (_, _) => { Outcome = PreflightOutcome.Proceed; DialogResult = DialogResult.OK; Close(); };
            _footer.Controls.Add(btnForce);
        }
    }
}
