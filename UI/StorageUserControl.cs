using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using HolosMigratorUI.Core;

namespace HolosMigratorUI.UI;

public partial class StorageUserControl : UserControl
{
    private readonly Func<string> _hostProvider;
    private readonly Func<int>    _sshPortProvider;
    private readonly Func<string> _serverUserProvider;
    private readonly Func<string> _sshKeyPathProvider;
    private readonly Func<string> _sshPasswordProvider;

    public StorageUserControl()
        : this(() => string.Empty, () => 22, () => string.Empty, () => string.Empty, () => string.Empty) { }

    public StorageUserControl(
        Func<string> hostProvider,
        Func<int>    sshPortProvider,
        Func<string> serverUserProvider,
        Func<string> sshKeyPathProvider,
        Func<string> sshPasswordProvider)
    {
        _hostProvider        = hostProvider;
        _sshPortProvider     = sshPortProvider;
        _serverUserProvider  = serverUserProvider;
        _sshKeyPathProvider  = sshKeyPathProvider;
        _sshPasswordProvider = sshPasswordProvider;

        InitializeComponent();
        ApplyGridStyles();
        BindEvents();

        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            _ = RefreshAsync();
    }

    // ── Inicialización ────────────────────────────────────────────────────

    private void BindEvents()
    {
        btnRefresh.Click      += async (_, _) => await RefreshAsync();
        btnPruneBuilder.Click += async (_, _) => await PruneAsync("builder");
        btnPruneImages.Click  += async (_, _) => await PruneAsync("images");
        btnPruneAll.Click     += async (_, _) =>
        {
            var r = MessageBox.Show(
                "Esto eliminará TODAS las imágenes, contenedores detenidos y volúmenes no usados.\n¿Continuar?",
                "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes) await PruneAsync("system");
        };
        tabControl.DrawItem += DrawTab;
    }

    private void ApplyGridStyles()
    {
        UIHelper.AplicarEstiloModernoTabla(gridImages);
        UIHelper.AplicarEstiloModernoTabla(gridTopDirs);
    }

    // ── Lógica de negocio ─────────────────────────────────────────────────

    private async Task RefreshAsync()
    {
        if (!ValidateCredentials()) return;

        SetStatus("Consultando almacenamiento remoto…", Color.FromArgb(140, 200, 255));
        SetButtonsEnabled(false);

        var raw = await HealthCheckService.TryGetRemoteStorageInfoAsync(
            _hostProvider(), _serverUserProvider(), _sshPortProvider(),
            NullIfEmpty(_sshKeyPathProvider()), NullIfEmpty(_sshPasswordProvider()));

        SetButtonsEnabled(true);

        if (string.IsNullOrWhiteSpace(raw))
        {
            SetStatus("No se pudo obtener datos del VPS.", Color.FromArgb(255, 100, 100));
            return;
        }

        ParseAndDisplay(raw);
        SetStatus($"Actualizado: {DateTime.Now:HH:mm:ss}", Color.FromArgb(80, 220, 140));
    }

    private async Task PruneAsync(string target)
    {
        if (!ValidateCredentials()) return;

        var label = target switch
        {
            "builder" => "build cache",
            "images"  => "imágenes no usadas",
            "system"  => "SISTEMA COMPLETO",
            _         => target
        };

        SetStatus($"Limpiando {label}…", Color.FromArgb(255, 187, 68));
        SetButtonsEnabled(false);

        var result = await HealthCheckService.TryRunDockerPruneAsync(
            _hostProvider(), _serverUserProvider(), _sshPortProvider(),
            NullIfEmpty(_sshKeyPathProvider()), NullIfEmpty(_sshPasswordProvider()), target);

        SetButtonsEnabled(true);
        SetStatus($"Limpieza completada ({label}). Actualizando…", Color.FromArgb(80, 220, 140));

        if (!string.IsNullOrWhiteSpace(result))
            MessageBox.Show(result, $"Resultado – {label}", MessageBoxButtons.OK, MessageBoxIcon.Information);

        await RefreshAsync();
    }

    private void ParseAndDisplay(string raw)
    {
        var section  = "";
        var imgTable = new DataTable();
        imgTable.Columns.Add("Imagen");
        imgTable.Columns.Add("Tamaño");
        imgTable.Columns.Add("ID");

        var dirTable = new DataTable();
        dirTable.Columns.Add("Tamaño");
        dirTable.Columns.Add("Directorio");

        foreach (var rawLine in raw.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var line = rawLine.Trim();
            if (line.StartsWith('[') && line.EndsWith(']')) { section = line; continue; }

            switch (section)
            {
                case "[DISK]":
                    var dparts = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                    if (dparts.Length >= 5 && dparts[0] != "Filesystem")
                    {
                        SafeSet(lblDiskUsed, dparts[2]);
                        SafeSet(lblDiskFree, dparts[3]);
                        SafeSet(lblDiskPct,  dparts[4]);
                        var pct = int.TryParse(dparts[4].TrimEnd('%'), out var p) ? p : -1;
                        lblDiskPct.Invoke(() => lblDiskPct.ForeColor = pct >= 80
                            ? Color.FromArgb(255, 80, 80)
                            : pct >= 60 ? Color.FromArgb(255, 187, 68)
                            : Color.FromArgb(0, 230, 130));
                    }
                    break;

                case "[DOCKER]":
                    var sparts = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                    if (sparts.Length >= 4)
                    {
                        if (sparts[0] == "Images")     SafeSet(lblDockerImg,  sparts[3]);
                        if (sparts[0] == "Local")      SafeSet(lblDockerVol,  sparts[3]);
                        if (sparts[0] == "Build")      SafeSet(lblDockerCach, sparts[3]);
                    }
                    break;

                case "[DOCKER_IMAGES]":
                    var iparts = line.Split('\t');
                    if (iparts.Length >= 3)
                        imgTable.Rows.Add(iparts[0], iparts[1], iparts[2]);
                    break;

                case "[TOP_DIRS]":
                    var tparts = line.Split('\t');
                    if (tparts.Length >= 2)
                        dirTable.Rows.Add(tparts[0], tparts[1]);
                    break;
            }
        }

        gridImages.Invoke(()  => gridImages.DataSource  = imgTable);
        gridTopDirs.Invoke(() => gridTopDirs.DataSource = dirTable);
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private bool ValidateCredentials()
    {
        var host = _hostProvider().Trim();
        var user = _serverUserProvider().Trim();
        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user))
        {
            MessageBox.Show("Configura SERVER_HOST y SERVER_USER.", "Faltan datos",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    private void SetStatus(string msg, Color color) =>
        lblStatus.Invoke(() => { lblStatus.Text = msg; lblStatus.ForeColor = color; });

    private void SetButtonsEnabled(bool enabled) =>
        Invoke(() =>
        {
            btnRefresh.Enabled      = enabled;
            btnPruneBuilder.Enabled = enabled;
            btnPruneImages.Enabled  = enabled;
            btnPruneAll.Enabled     = enabled;
        });

    private static string? NullIfEmpty(string s) =>
        string.IsNullOrWhiteSpace(s) ? null : s;

    private static void SafeSet(Label lbl, string val) =>
        lbl.Invoke(() => lbl.Text = val);

    private static void DrawTab(object? sender, DrawItemEventArgs e)
    {
        if (sender is not TabControl tc) return;
        var tab    = tc.TabPages[e.Index];
        bool active = e.Index == tc.SelectedIndex;

        using var bg = new SolidBrush(active ? Color.FromArgb(22, 80, 100) : Color.FromArgb(15, 18, 26));
        e.Graphics.FillRectangle(bg, e.Bounds);

        using var fg = new SolidBrush(active ? Color.Cyan : Color.FromArgb(160, 180, 200));
        var sf = new StringFormat
        {
            Alignment     = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        e.Graphics.DrawString(
            tab.Text,
            new Font("Segoe UI Semibold", 9.5F, active ? FontStyle.Bold : FontStyle.Regular),
            fg, e.Bounds, sf);
    }
}
