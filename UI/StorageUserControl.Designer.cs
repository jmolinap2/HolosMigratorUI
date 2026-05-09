namespace HolosMigratorUI.UI;

partial class StorageUserControl
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlHeader = new Panel();
        flpButtons = new FlowLayoutPanel();
        btnPruneAll = new Button();
        btnPruneImages = new Button();
        btnPruneBuilder = new Button();
        btnRefresh = new Button();
        lblTitle = new Label();
        pnlCards = new Panel();
        flpCards = new FlowLayoutPanel();
        pnlCardDiskUsed = new Panel();
        lblDiskUsed = new Label();
        lblDiskUsedTitle = new Label();
        pnlAccDiskUsed = new Panel();
        pnlCardDiskFree = new Panel();
        lblDiskFree = new Label();
        lblDiskFreeTitle = new Label();
        pnlAccDiskFree = new Panel();
        pnlCardDiskPct = new Panel();
        lblDiskPct = new Label();
        lblDiskPctTitle = new Label();
        pnlAccDiskPct = new Panel();
        pnlCardDockerImg = new Panel();
        lblDockerImg = new Label();
        lblDockerImgTitle = new Label();
        pnlAccDockerImg = new Panel();
        pnlCardDockerVol = new Panel();
        lblDockerVol = new Label();
        lblDockerVolTitle = new Label();
        pnlAccDockerVol = new Panel();
        pnlCardDockerCach = new Panel();
        lblDockerCach = new Label();
        lblDockerCachTitle = new Label();
        pnlAccDockerCach = new Panel();
        lblStatus = new Label();
        pnlBody = new Panel();
        tabControl = new TabControl();
        tabImages = new TabPage();
        gridImages = new DataGridView();
        tabDirs = new TabPage();
        gridTopDirs = new DataGridView();
        pnlHeader.SuspendLayout();
        flpButtons.SuspendLayout();
        pnlCards.SuspendLayout();
        flpCards.SuspendLayout();
        pnlCardDiskUsed.SuspendLayout();
        pnlCardDiskFree.SuspendLayout();
        pnlCardDiskPct.SuspendLayout();
        pnlCardDockerImg.SuspendLayout();
        pnlCardDockerVol.SuspendLayout();
        pnlCardDockerCach.SuspendLayout();
        pnlBody.SuspendLayout();
        tabControl.SuspendLayout();
        tabImages.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridImages).BeginInit();
        tabDirs.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridTopDirs).BeginInit();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(12, 12, 18);
        pnlHeader.Controls.Add(flpButtons);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(4, 5, 4, 5);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1920, 93);
        pnlHeader.TabIndex = 3;
        // 
        // flpButtons
        // 
        flpButtons.BackColor = Color.Transparent;
        flpButtons.Controls.Add(btnPruneAll);
        flpButtons.Controls.Add(btnPruneImages);
        flpButtons.Controls.Add(btnPruneBuilder);
        flpButtons.Controls.Add(btnRefresh);
        flpButtons.Dock = DockStyle.Fill;
        flpButtons.FlowDirection = FlowDirection.RightToLeft;
        flpButtons.Location = new Point(543, 0);
        flpButtons.Margin = new Padding(4, 5, 4, 5);
        flpButtons.Name = "flpButtons";
        flpButtons.Padding = new Padding(0, 13, 11, 13);
        flpButtons.Size = new Size(1377, 93);
        flpButtons.TabIndex = 1;
        flpButtons.WrapContents = false;
        // 
        // btnPruneAll
        // 
        btnPruneAll.AutoSize = true;
        btnPruneAll.BackColor = Color.FromArgb(80, 10, 10);
        btnPruneAll.Cursor = Cursors.Hand;
        btnPruneAll.FlatAppearance.BorderSize = 0;
        btnPruneAll.FlatStyle = FlatStyle.Flat;
        btnPruneAll.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        btnPruneAll.ForeColor = Color.FromArgb(255, 60, 60);
        btnPruneAll.Location = new Point(1109, 13);
        btnPruneAll.Margin = new Padding(6, 0, 6, 0);
        btnPruneAll.Name = "btnPruneAll";
        btnPruneAll.Size = new Size(251, 60);
        btnPruneAll.TabIndex = 0;
        btnPruneAll.Text = "☠  Limpieza Total";
        btnPruneAll.UseVisualStyleBackColor = false;
        // 
        // btnPruneImages
        // 
        btnPruneImages.AutoSize = true;
        btnPruneImages.BackColor = Color.FromArgb(50, 18, 18);
        btnPruneImages.Cursor = Cursors.Hand;
        btnPruneImages.FlatAppearance.BorderSize = 0;
        btnPruneImages.FlatStyle = FlatStyle.Flat;
        btnPruneImages.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        btnPruneImages.ForeColor = Color.FromArgb(255, 120, 120);
        btnPruneImages.Location = new Point(804, 13);
        btnPruneImages.Margin = new Padding(6, 0, 6, 0);
        btnPruneImages.Name = "btnPruneImages";
        btnPruneImages.Size = new Size(293, 60);
        btnPruneImages.TabIndex = 1;
        btnPruneImages.Text = "🗑  Limpiar Imágenes";
        btnPruneImages.UseVisualStyleBackColor = false;
        // 
        // btnPruneBuilder
        // 
        btnPruneBuilder.AutoSize = true;
        btnPruneBuilder.BackColor = Color.FromArgb(60, 38, 10);
        btnPruneBuilder.Cursor = Cursors.Hand;
        btnPruneBuilder.FlatAppearance.BorderSize = 0;
        btnPruneBuilder.FlatStyle = FlatStyle.Flat;
        btnPruneBuilder.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        btnPruneBuilder.ForeColor = Color.FromArgb(255, 187, 68);
        btnPruneBuilder.Location = new Point(476, 13);
        btnPruneBuilder.Margin = new Padding(6, 0, 6, 0);
        btnPruneBuilder.Name = "btnPruneBuilder";
        btnPruneBuilder.Size = new Size(316, 60);
        btnPruneBuilder.TabIndex = 2;
        btnPruneBuilder.Text = "\U0001f9f9  Limpiar Build Cache";
        btnPruneBuilder.UseVisualStyleBackColor = false;
        // 
        // btnRefresh
        // 
        btnRefresh.AutoSize = true;
        btnRefresh.BackColor = Color.FromArgb(22, 80, 100);
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.Cyan;
        btnRefresh.Location = new Point(280, 13);
        btnRefresh.Margin = new Padding(6, 0, 6, 0);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(184, 60);
        btnRefresh.TabIndex = 3;
        btnRefresh.Text = "⟳  Actualizar";
        btnRefresh.UseVisualStyleBackColor = false;
        // 
        // lblTitle
        // 
        lblTitle.Dock = DockStyle.Left;
        lblTitle.Font = new Font("Consolas", 14F, FontStyle.Bold);
        lblTitle.ForeColor = Color.Cyan;
        lblTitle.Location = new Point(0, 0);
        lblTitle.Margin = new Padding(4, 0, 4, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(23, 0, 0, 0);
        lblTitle.Size = new Size(543, 93);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "ALMACENAMIENTO VPS";
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlCards
        // 
        pnlCards.BackColor = Color.FromArgb(12, 14, 20);
        pnlCards.Controls.Add(flpCards);
        pnlCards.Dock = DockStyle.Top;
        pnlCards.Location = new Point(0, 93);
        pnlCards.Margin = new Padding(4, 5, 4, 5);
        pnlCards.Name = "pnlCards";
        pnlCards.Padding = new Padding(17, 13, 17, 10);
        pnlCards.Size = new Size(1920, 180);
        pnlCards.TabIndex = 2;
        // 
        // flpCards
        // 
        flpCards.BackColor = Color.Transparent;
        flpCards.Controls.Add(pnlCardDiskUsed);
        flpCards.Controls.Add(pnlCardDiskFree);
        flpCards.Controls.Add(pnlCardDiskPct);
        flpCards.Controls.Add(pnlCardDockerImg);
        flpCards.Controls.Add(pnlCardDockerVol);
        flpCards.Controls.Add(pnlCardDockerCach);
        flpCards.Dock = DockStyle.Fill;
        flpCards.Location = new Point(17, 13);
        flpCards.Margin = new Padding(4, 5, 4, 5);
        flpCards.Name = "flpCards";
        flpCards.Size = new Size(1886, 157);
        flpCards.TabIndex = 0;
        flpCards.WrapContents = false;
        // 
        // pnlCardDiskUsed
        // 
        pnlCardDiskUsed.BackColor = Color.FromArgb(18, 22, 32);
        pnlCardDiskUsed.Controls.Add(lblDiskUsed);
        pnlCardDiskUsed.Controls.Add(lblDiskUsedTitle);
        pnlCardDiskUsed.Controls.Add(pnlAccDiskUsed);
        pnlCardDiskUsed.Location = new Point(0, 0);
        pnlCardDiskUsed.Margin = new Padding(0, 0, 14, 0);
        pnlCardDiskUsed.Name = "pnlCardDiskUsed";
        pnlCardDiskUsed.Size = new Size(211, 157);
        pnlCardDiskUsed.TabIndex = 0;
        // 
        // lblDiskUsed
        // 
        lblDiskUsed.Dock = DockStyle.Fill;
        lblDiskUsed.Font = new Font("Consolas", 16F, FontStyle.Bold);
        lblDiskUsed.ForeColor = Color.FromArgb(0, 200, 255);
        lblDiskUsed.Location = new Point(0, 42);
        lblDiskUsed.Margin = new Padding(4, 0, 4, 0);
        lblDiskUsed.Name = "lblDiskUsed";
        lblDiskUsed.Size = new Size(211, 115);
        lblDiskUsed.TabIndex = 0;
        lblDiskUsed.Text = "--";
        lblDiskUsed.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDiskUsedTitle
        // 
        lblDiskUsedTitle.Dock = DockStyle.Top;
        lblDiskUsedTitle.Font = new Font("Segoe UI", 8F);
        lblDiskUsedTitle.ForeColor = Color.FromArgb(140, 160, 180);
        lblDiskUsedTitle.Location = new Point(0, 5);
        lblDiskUsedTitle.Margin = new Padding(4, 0, 4, 0);
        lblDiskUsedTitle.Name = "lblDiskUsedTitle";
        lblDiskUsedTitle.Padding = new Padding(11, 0, 0, 0);
        lblDiskUsedTitle.Size = new Size(211, 37);
        lblDiskUsedTitle.TabIndex = 1;
        lblDiskUsedTitle.Text = "DISCO USADO";
        lblDiskUsedTitle.TextAlign = ContentAlignment.BottomLeft;
        // 
        // pnlAccDiskUsed
        // 
        pnlAccDiskUsed.BackColor = Color.FromArgb(0, 200, 255);
        pnlAccDiskUsed.Dock = DockStyle.Top;
        pnlAccDiskUsed.Location = new Point(0, 0);
        pnlAccDiskUsed.Margin = new Padding(4, 5, 4, 5);
        pnlAccDiskUsed.Name = "pnlAccDiskUsed";
        pnlAccDiskUsed.Size = new Size(211, 5);
        pnlAccDiskUsed.TabIndex = 2;
        // 
        // pnlCardDiskFree
        // 
        pnlCardDiskFree.BackColor = Color.FromArgb(18, 22, 32);
        pnlCardDiskFree.Controls.Add(lblDiskFree);
        pnlCardDiskFree.Controls.Add(lblDiskFreeTitle);
        pnlCardDiskFree.Controls.Add(pnlAccDiskFree);
        pnlCardDiskFree.Location = new Point(225, 0);
        pnlCardDiskFree.Margin = new Padding(0, 0, 14, 0);
        pnlCardDiskFree.Name = "pnlCardDiskFree";
        pnlCardDiskFree.Size = new Size(211, 157);
        pnlCardDiskFree.TabIndex = 1;
        // 
        // lblDiskFree
        // 
        lblDiskFree.Dock = DockStyle.Fill;
        lblDiskFree.Font = new Font("Consolas", 16F, FontStyle.Bold);
        lblDiskFree.ForeColor = Color.FromArgb(0, 230, 130);
        lblDiskFree.Location = new Point(0, 42);
        lblDiskFree.Margin = new Padding(4, 0, 4, 0);
        lblDiskFree.Name = "lblDiskFree";
        lblDiskFree.Size = new Size(211, 115);
        lblDiskFree.TabIndex = 0;
        lblDiskFree.Text = "--";
        lblDiskFree.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDiskFreeTitle
        // 
        lblDiskFreeTitle.Dock = DockStyle.Top;
        lblDiskFreeTitle.Font = new Font("Segoe UI", 8F);
        lblDiskFreeTitle.ForeColor = Color.FromArgb(140, 160, 180);
        lblDiskFreeTitle.Location = new Point(0, 5);
        lblDiskFreeTitle.Margin = new Padding(4, 0, 4, 0);
        lblDiskFreeTitle.Name = "lblDiskFreeTitle";
        lblDiskFreeTitle.Padding = new Padding(11, 0, 0, 0);
        lblDiskFreeTitle.Size = new Size(211, 37);
        lblDiskFreeTitle.TabIndex = 1;
        lblDiskFreeTitle.Text = "DISCO LIBRE";
        lblDiskFreeTitle.TextAlign = ContentAlignment.BottomLeft;
        // 
        // pnlAccDiskFree
        // 
        pnlAccDiskFree.BackColor = Color.FromArgb(0, 230, 130);
        pnlAccDiskFree.Dock = DockStyle.Top;
        pnlAccDiskFree.Location = new Point(0, 0);
        pnlAccDiskFree.Margin = new Padding(4, 5, 4, 5);
        pnlAccDiskFree.Name = "pnlAccDiskFree";
        pnlAccDiskFree.Size = new Size(211, 5);
        pnlAccDiskFree.TabIndex = 2;
        // 
        // pnlCardDiskPct
        // 
        pnlCardDiskPct.BackColor = Color.FromArgb(18, 22, 32);
        pnlCardDiskPct.Controls.Add(lblDiskPct);
        pnlCardDiskPct.Controls.Add(lblDiskPctTitle);
        pnlCardDiskPct.Controls.Add(pnlAccDiskPct);
        pnlCardDiskPct.Location = new Point(450, 0);
        pnlCardDiskPct.Margin = new Padding(0, 0, 14, 0);
        pnlCardDiskPct.Name = "pnlCardDiskPct";
        pnlCardDiskPct.Size = new Size(211, 157);
        pnlCardDiskPct.TabIndex = 2;
        // 
        // lblDiskPct
        // 
        lblDiskPct.Dock = DockStyle.Fill;
        lblDiskPct.Font = new Font("Consolas", 16F, FontStyle.Bold);
        lblDiskPct.ForeColor = Color.FromArgb(255, 187, 68);
        lblDiskPct.Location = new Point(0, 42);
        lblDiskPct.Margin = new Padding(4, 0, 4, 0);
        lblDiskPct.Name = "lblDiskPct";
        lblDiskPct.Size = new Size(211, 115);
        lblDiskPct.TabIndex = 0;
        lblDiskPct.Text = "--";
        lblDiskPct.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDiskPctTitle
        // 
        lblDiskPctTitle.Dock = DockStyle.Top;
        lblDiskPctTitle.Font = new Font("Segoe UI", 8F);
        lblDiskPctTitle.ForeColor = Color.FromArgb(140, 160, 180);
        lblDiskPctTitle.Location = new Point(0, 5);
        lblDiskPctTitle.Margin = new Padding(4, 0, 4, 0);
        lblDiskPctTitle.Name = "lblDiskPctTitle";
        lblDiskPctTitle.Padding = new Padding(11, 0, 0, 0);
        lblDiskPctTitle.Size = new Size(211, 37);
        lblDiskPctTitle.TabIndex = 1;
        lblDiskPctTitle.Text = "USO %";
        lblDiskPctTitle.TextAlign = ContentAlignment.BottomLeft;
        // 
        // pnlAccDiskPct
        // 
        pnlAccDiskPct.BackColor = Color.FromArgb(255, 187, 68);
        pnlAccDiskPct.Dock = DockStyle.Top;
        pnlAccDiskPct.Location = new Point(0, 0);
        pnlAccDiskPct.Margin = new Padding(4, 5, 4, 5);
        pnlAccDiskPct.Name = "pnlAccDiskPct";
        pnlAccDiskPct.Size = new Size(211, 5);
        pnlAccDiskPct.TabIndex = 2;
        // 
        // pnlCardDockerImg
        // 
        pnlCardDockerImg.BackColor = Color.FromArgb(18, 22, 32);
        pnlCardDockerImg.Controls.Add(lblDockerImg);
        pnlCardDockerImg.Controls.Add(lblDockerImgTitle);
        pnlCardDockerImg.Controls.Add(pnlAccDockerImg);
        pnlCardDockerImg.Location = new Point(675, 0);
        pnlCardDockerImg.Margin = new Padding(0, 0, 14, 0);
        pnlCardDockerImg.Name = "pnlCardDockerImg";
        pnlCardDockerImg.Size = new Size(211, 157);
        pnlCardDockerImg.TabIndex = 3;
        // 
        // lblDockerImg
        // 
        lblDockerImg.Dock = DockStyle.Fill;
        lblDockerImg.Font = new Font("Consolas", 16F, FontStyle.Bold);
        lblDockerImg.ForeColor = Color.FromArgb(200, 130, 255);
        lblDockerImg.Location = new Point(0, 42);
        lblDockerImg.Margin = new Padding(4, 0, 4, 0);
        lblDockerImg.Name = "lblDockerImg";
        lblDockerImg.Size = new Size(211, 115);
        lblDockerImg.TabIndex = 0;
        lblDockerImg.Text = "--";
        lblDockerImg.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDockerImgTitle
        // 
        lblDockerImgTitle.Dock = DockStyle.Top;
        lblDockerImgTitle.Font = new Font("Segoe UI", 8F);
        lblDockerImgTitle.ForeColor = Color.FromArgb(140, 160, 180);
        lblDockerImgTitle.Location = new Point(0, 5);
        lblDockerImgTitle.Margin = new Padding(4, 0, 4, 0);
        lblDockerImgTitle.Name = "lblDockerImgTitle";
        lblDockerImgTitle.Padding = new Padding(11, 0, 0, 0);
        lblDockerImgTitle.Size = new Size(211, 37);
        lblDockerImgTitle.TabIndex = 1;
        lblDockerImgTitle.Text = "IMÁGENES";
        lblDockerImgTitle.TextAlign = ContentAlignment.BottomLeft;
        // 
        // pnlAccDockerImg
        // 
        pnlAccDockerImg.BackColor = Color.FromArgb(200, 130, 255);
        pnlAccDockerImg.Dock = DockStyle.Top;
        pnlAccDockerImg.Location = new Point(0, 0);
        pnlAccDockerImg.Margin = new Padding(4, 5, 4, 5);
        pnlAccDockerImg.Name = "pnlAccDockerImg";
        pnlAccDockerImg.Size = new Size(211, 5);
        pnlAccDockerImg.TabIndex = 2;
        // 
        // pnlCardDockerVol
        // 
        pnlCardDockerVol.BackColor = Color.FromArgb(18, 22, 32);
        pnlCardDockerVol.Controls.Add(lblDockerVol);
        pnlCardDockerVol.Controls.Add(lblDockerVolTitle);
        pnlCardDockerVol.Controls.Add(pnlAccDockerVol);
        pnlCardDockerVol.Location = new Point(900, 0);
        pnlCardDockerVol.Margin = new Padding(0, 0, 14, 0);
        pnlCardDockerVol.Name = "pnlCardDockerVol";
        pnlCardDockerVol.Size = new Size(211, 157);
        pnlCardDockerVol.TabIndex = 4;
        // 
        // lblDockerVol
        // 
        lblDockerVol.Dock = DockStyle.Fill;
        lblDockerVol.Font = new Font("Consolas", 16F, FontStyle.Bold);
        lblDockerVol.ForeColor = Color.FromArgb(130, 200, 255);
        lblDockerVol.Location = new Point(0, 42);
        lblDockerVol.Margin = new Padding(4, 0, 4, 0);
        lblDockerVol.Name = "lblDockerVol";
        lblDockerVol.Size = new Size(211, 115);
        lblDockerVol.TabIndex = 0;
        lblDockerVol.Text = "--";
        lblDockerVol.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDockerVolTitle
        // 
        lblDockerVolTitle.Dock = DockStyle.Top;
        lblDockerVolTitle.Font = new Font("Segoe UI", 8F);
        lblDockerVolTitle.ForeColor = Color.FromArgb(140, 160, 180);
        lblDockerVolTitle.Location = new Point(0, 5);
        lblDockerVolTitle.Margin = new Padding(4, 0, 4, 0);
        lblDockerVolTitle.Name = "lblDockerVolTitle";
        lblDockerVolTitle.Padding = new Padding(11, 0, 0, 0);
        lblDockerVolTitle.Size = new Size(211, 37);
        lblDockerVolTitle.TabIndex = 1;
        lblDockerVolTitle.Text = "VOLÚMENES";
        lblDockerVolTitle.TextAlign = ContentAlignment.BottomLeft;
        // 
        // pnlAccDockerVol
        // 
        pnlAccDockerVol.BackColor = Color.FromArgb(130, 200, 255);
        pnlAccDockerVol.Dock = DockStyle.Top;
        pnlAccDockerVol.Location = new Point(0, 0);
        pnlAccDockerVol.Margin = new Padding(4, 5, 4, 5);
        pnlAccDockerVol.Name = "pnlAccDockerVol";
        pnlAccDockerVol.Size = new Size(211, 5);
        pnlAccDockerVol.TabIndex = 2;
        // 
        // pnlCardDockerCach
        // 
        pnlCardDockerCach.BackColor = Color.FromArgb(18, 22, 32);
        pnlCardDockerCach.Controls.Add(lblDockerCach);
        pnlCardDockerCach.Controls.Add(lblDockerCachTitle);
        pnlCardDockerCach.Controls.Add(pnlAccDockerCach);
        pnlCardDockerCach.Location = new Point(1125, 0);
        pnlCardDockerCach.Margin = new Padding(0, 0, 14, 0);
        pnlCardDockerCach.Name = "pnlCardDockerCach";
        pnlCardDockerCach.Size = new Size(211, 157);
        pnlCardDockerCach.TabIndex = 5;
        // 
        // lblDockerCach
        // 
        lblDockerCach.Dock = DockStyle.Fill;
        lblDockerCach.Font = new Font("Consolas", 16F, FontStyle.Bold);
        lblDockerCach.ForeColor = Color.FromArgb(255, 120, 60);
        lblDockerCach.Location = new Point(0, 42);
        lblDockerCach.Margin = new Padding(4, 0, 4, 0);
        lblDockerCach.Name = "lblDockerCach";
        lblDockerCach.Size = new Size(211, 115);
        lblDockerCach.TabIndex = 0;
        lblDockerCach.Text = "--";
        lblDockerCach.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDockerCachTitle
        // 
        lblDockerCachTitle.Dock = DockStyle.Top;
        lblDockerCachTitle.Font = new Font("Segoe UI", 8F);
        lblDockerCachTitle.ForeColor = Color.FromArgb(140, 160, 180);
        lblDockerCachTitle.Location = new Point(0, 5);
        lblDockerCachTitle.Margin = new Padding(4, 0, 4, 0);
        lblDockerCachTitle.Name = "lblDockerCachTitle";
        lblDockerCachTitle.Padding = new Padding(11, 0, 0, 0);
        lblDockerCachTitle.Size = new Size(211, 37);
        lblDockerCachTitle.TabIndex = 1;
        lblDockerCachTitle.Text = "BUILD CACHE";
        lblDockerCachTitle.TextAlign = ContentAlignment.BottomLeft;
        // 
        // pnlAccDockerCach
        // 
        pnlAccDockerCach.BackColor = Color.FromArgb(255, 120, 60);
        pnlAccDockerCach.Dock = DockStyle.Top;
        pnlAccDockerCach.Location = new Point(0, 0);
        pnlAccDockerCach.Margin = new Padding(4, 5, 4, 5);
        pnlAccDockerCach.Name = "pnlAccDockerCach";
        pnlAccDockerCach.Size = new Size(211, 5);
        pnlAccDockerCach.TabIndex = 2;
        // 
        // lblStatus
        // 
        lblStatus.Dock = DockStyle.Top;
        lblStatus.Font = new Font("Segoe UI", 9F);
        lblStatus.ForeColor = Color.FromArgb(140, 160, 180);
        lblStatus.Location = new Point(0, 273);
        lblStatus.Margin = new Padding(4, 0, 4, 0);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(11, 0, 0, 0);
        lblStatus.Size = new Size(1920, 40);
        lblStatus.TabIndex = 1;
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlBody
        // 
        pnlBody.Controls.Add(tabControl);
        pnlBody.Dock = DockStyle.Fill;
        pnlBody.Location = new Point(0, 313);
        pnlBody.Margin = new Padding(4, 5, 4, 5);
        pnlBody.Name = "pnlBody";
        pnlBody.Padding = new Padding(17, 13, 17, 20);
        pnlBody.Size = new Size(1920, 779);
        pnlBody.TabIndex = 0;
        // 
        // tabControl
        // 
        tabControl.Controls.Add(tabImages);
        tabControl.Controls.Add(tabDirs);
        tabControl.Dock = DockStyle.Fill;
        tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
        tabControl.Font = new Font("Segoe UI Semibold", 9.5F);
        tabControl.ItemSize = new Size(210, 36);
        tabControl.Location = new Point(17, 13);
        tabControl.Margin = new Padding(4, 5, 4, 5);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(1886, 746);
        tabControl.SizeMode = TabSizeMode.Fixed;
        tabControl.TabIndex = 0;
        // 
        // tabImages
        // 
        tabImages.BackColor = Color.FromArgb(9, 9, 11);
        tabImages.Controls.Add(gridImages);
        tabImages.ForeColor = Color.White;
        tabImages.Location = new Point(4, 40);
        tabImages.Margin = new Padding(4, 5, 4, 5);
        tabImages.Name = "tabImages";
        tabImages.Size = new Size(1878, 702);
        tabImages.TabIndex = 0;
        tabImages.Text = "Imágenes Docker";
        // 
        // gridImages
        // 
        gridImages.BackgroundColor = Color.FromArgb(17, 24, 34);
        gridImages.BorderStyle = BorderStyle.None;
        gridImages.ColumnHeadersHeight = 34;
        gridImages.Dock = DockStyle.Fill;
        gridImages.Location = new Point(0, 0);
        gridImages.Margin = new Padding(4, 5, 4, 5);
        gridImages.Name = "gridImages";
        gridImages.RowHeadersWidth = 62;
        gridImages.Size = new Size(1878, 702);
        gridImages.TabIndex = 0;
        // 
        // tabDirs
        // 
        tabDirs.BackColor = Color.FromArgb(9, 9, 11);
        tabDirs.Controls.Add(gridTopDirs);
        tabDirs.ForeColor = Color.White;
        tabDirs.Location = new Point(4, 40);
        tabDirs.Margin = new Padding(4, 5, 4, 5);
        tabDirs.Name = "tabDirs";
        tabDirs.Size = new Size(1878, 702);
        tabDirs.TabIndex = 1;
        tabDirs.Text = "Directorios más pesados";
        // 
        // gridTopDirs
        // 
        gridTopDirs.BackgroundColor = Color.FromArgb(17, 24, 34);
        gridTopDirs.BorderStyle = BorderStyle.None;
        gridTopDirs.ColumnHeadersHeight = 34;
        gridTopDirs.Dock = DockStyle.Fill;
        gridTopDirs.Location = new Point(0, 0);
        gridTopDirs.Margin = new Padding(4, 5, 4, 5);
        gridTopDirs.Name = "gridTopDirs";
        gridTopDirs.RowHeadersWidth = 62;
        gridTopDirs.Size = new Size(1878, 702);
        gridTopDirs.TabIndex = 0;
        // 
        // StorageUserControl
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(9, 9, 11);
        Controls.Add(pnlBody);
        Controls.Add(lblStatus);
        Controls.Add(pnlCards);
        Controls.Add(pnlHeader);
        ForeColor = Color.FromArgb(220, 230, 240);
        Margin = new Padding(4, 5, 4, 5);
        MinimumSize = new Size(857, 500);
        Name = "StorageUserControl";
        Size = new Size(1920, 1092);
        pnlHeader.ResumeLayout(false);
        flpButtons.ResumeLayout(false);
        flpButtons.PerformLayout();
        pnlCards.ResumeLayout(false);
        flpCards.ResumeLayout(false);
        pnlCardDiskUsed.ResumeLayout(false);
        pnlCardDiskFree.ResumeLayout(false);
        pnlCardDiskPct.ResumeLayout(false);
        pnlCardDockerImg.ResumeLayout(false);
        pnlCardDockerVol.ResumeLayout(false);
        pnlCardDockerCach.ResumeLayout(false);
        pnlBody.ResumeLayout(false);
        tabControl.ResumeLayout(false);
        tabImages.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)gridImages).EndInit();
        tabDirs.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)gridTopDirs).EndInit();
        ResumeLayout(false);
    }

    // ── Control fields ────────────────────────────────────────────────────
    private Panel           pnlHeader;
    private Label           lblTitle;
    private FlowLayoutPanel flpButtons;
    private Button          btnRefresh;
    private Button          btnPruneBuilder;
    private Button          btnPruneImages;
    private Button          btnPruneAll;

    private Panel           pnlCards;
    private FlowLayoutPanel flpCards;

    private Panel pnlCardDiskUsed;
    private Panel pnlAccDiskUsed;
    private Label lblDiskUsedTitle;
    private Label lblDiskUsed;

    private Panel pnlCardDiskFree;
    private Panel pnlAccDiskFree;
    private Label lblDiskFreeTitle;
    private Label lblDiskFree;

    private Panel pnlCardDiskPct;
    private Panel pnlAccDiskPct;
    private Label lblDiskPctTitle;
    private Label lblDiskPct;

    private Panel pnlCardDockerImg;
    private Panel pnlAccDockerImg;
    private Label lblDockerImgTitle;
    private Label lblDockerImg;

    private Panel pnlCardDockerVol;
    private Panel pnlAccDockerVol;
    private Label lblDockerVolTitle;
    private Label lblDockerVol;

    private Panel pnlCardDockerCach;
    private Panel pnlAccDockerCach;
    private Label lblDockerCachTitle;
    private Label lblDockerCach;

    private Label           lblStatus;
    private Panel           pnlBody;
    private TabControl      tabControl;
    private TabPage         tabImages;
    private DataGridView    gridImages;
    private TabPage         tabDirs;
    private DataGridView    gridTopDirs;
}
