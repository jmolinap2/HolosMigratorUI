using System.Drawing;
using System.Windows.Forms;

namespace HolosMigratorUI.UI;

public static class UIHelper
{
    public static void AplicarEstiloModernoTabla(DataGridView grid)
    {
        // 1. Colores base
        Color colorFondoPrincipal = Color.FromArgb(17, 24, 34);
        Color colorFondoAlterno = Color.FromArgb(21, 30, 42);
        Color colorTexto = Color.FromArgb(226, 233, 241);
        Color colorBorde = Color.FromArgb(43, 58, 76);
        Color colorSeleccion = Color.FromArgb(20, 104, 163);

        // 2. Configuraciones estructurales
        grid.BackgroundColor = colorFondoPrincipal;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.GridColor = colorBorde;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        grid.AllowUserToResizeRows = false;
        grid.RowHeadersVisible = false;
        grid.AllowUserToAddRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.ReadOnly = true;

        // 3. Estilo de los encabezados de columna
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 18, 28);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(173, 220, 255);
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(12, 18, 28);
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

        // 4. Estilo de las celdas normales
        grid.DefaultCellStyle.BackColor = colorFondoPrincipal;
        grid.DefaultCellStyle.ForeColor = colorTexto;
        grid.DefaultCellStyle.SelectionBackColor = colorSeleccion;
        grid.DefaultCellStyle.SelectionForeColor = Color.White;
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        grid.DefaultCellStyle.Padding = new Padding(4, 1, 4, 1);
        grid.RowTemplate.Height = 32;

        // 5. Filas alternas
        grid.AlternatingRowsDefaultCellStyle.BackColor = colorFondoAlterno;
    }

    public static Button CrearBotonMenu(string texto)
    {
        var btn = new Button
        {
            Text = texto,
            FlatStyle = FlatStyle.Flat,
            Dock = DockStyle.Top,
            Height = 50,
            ForeColor = Color.FromArgb(200, 200, 210),
            BackColor = Color.FromArgb(15, 17, 26),
            Cursor = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(20, 0, 0, 0),
            Font = new Font("Consolas", 11, FontStyle.Bold)
        };
        btn.FlatAppearance.BorderSize = 0;
        
        btn.MouseEnter += delegate { btn.BackColor = Color.FromArgb(28, 30, 40); btn.ForeColor = Color.Cyan; };
        btn.MouseLeave += delegate { btn.BackColor = Color.FromArgb(15, 17, 26); btn.ForeColor = Color.FromArgb(200, 200, 210); };
        
        return btn;
    }
}