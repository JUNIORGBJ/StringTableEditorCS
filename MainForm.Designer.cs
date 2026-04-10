using System.ComponentModel;
using System.Drawing;

#nullable disable

namespace StringTableEditorCS
{
    partial class MainForm
    {
        private IContainer components = null;
        private Panel pnlControls;
        private Button btnOpenTable;
        private Button btnOpenData;
        private Button btnSave;
        private Label lblStatus;
        private TextBox txtSearch;
        private Button btnClearSearch;
        private Label lblSearchStatus;
        private SplitContainer splitMain;
        private ListBox lstEntries;
        private RichTextBox rtxtContent;
        private ToolTip toolTipMain;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            pnlControls = new Panel();
            btnOpenTable = new Button();
            btnOpenData = new Button();
            btnSave = new Button();
            lblStatus = new Label();
            txtSearch = new TextBox();
            btnClearSearch = new Button();
            lblSearchStatus = new Label();
            splitMain = new SplitContainer();
            lstEntries = new ListBox();
            rtxtContent = new RichTextBox();
            toolTipMain = new ToolTip(components);
            pnlControls.SuspendLayout();
            ((ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            SuspendLayout();
            // 
            // pnlControls
            // 
            pnlControls.Controls.Add(btnOpenTable);
            pnlControls.Controls.Add(btnOpenData);
            pnlControls.Controls.Add(btnSave);
            pnlControls.Controls.Add(lblStatus);
            pnlControls.Controls.Add(txtSearch);
            pnlControls.Controls.Add(btnClearSearch);
            pnlControls.Controls.Add(lblSearchStatus);
            pnlControls.Dock = DockStyle.Top;
            pnlControls.Location = new Point(0, 0);
            pnlControls.Name = "pnlControls";
            pnlControls.Size = new Size(984, 90);
            pnlControls.TabIndex = 0;
            // 
            // btnOpenTable
            // 
            btnOpenTable.Location = new Point(10, 10);
            btnOpenTable.Name = "btnOpenTable";
            btnOpenTable.Size = new Size(150, 23);
            btnOpenTable.TabIndex = 0;
            btnOpenTable.Text = "Abrir Tabela (.bin)";
            btnOpenTable.UseVisualStyleBackColor = true;
            btnOpenTable.Click += BtnOpenTable_Click;
            // 
            // btnOpenData
            // 
            btnOpenData.Location = new Point(170, 10);
            btnOpenData.Name = "btnOpenData";
            btnOpenData.Size = new Size(150, 23);
            btnOpenData.TabIndex = 1;
            btnOpenData.Text = "Abrir Dados (.bin)";
            btnOpenData.UseVisualStyleBackColor = true;
            btnOpenData.Click += BtnOpenData_Click;
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new Point(330, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 23);
            btnSave.TabIndex = 2;
            btnSave.Text = "Salvar Alterações";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.AutoEllipsis = true;
            lblStatus.Location = new Point(490, 14);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(482, 15);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Pronto";
            toolTipMain.SetToolTip(lblStatus, "Pronto");
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.Location = new Point(10, 50);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Pesquisar entradas por ID ou conteúdo";
            txtSearch.Size = new Size(310, 23);
            txtSearch.TabIndex = 4;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // btnClearSearch
            // 
            btnClearSearch.Location = new Point(330, 49);
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Size = new Size(120, 25);
            btnClearSearch.TabIndex = 5;
            btnClearSearch.Text = "Limpar pesquisa";
            btnClearSearch.UseVisualStyleBackColor = true;
            btnClearSearch.Click += BtnClearSearch_Click;
            // 
            // lblSearchStatus
            // 
            lblSearchStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSearchStatus.Location = new Point(460, 53);
            lblSearchStatus.Name = "lblSearchStatus";
            lblSearchStatus.Size = new Size(512, 15);
            lblSearchStatus.TabIndex = 6;
            lblSearchStatus.Text = "Nenhuma entrada carregada.";
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 90);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(lstEntries);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(rtxtContent);
            splitMain.Size = new Size(984, 601);
            splitMain.SplitterDistance = 320;
            splitMain.TabIndex = 1;
            // 
            // lstEntries
            // 
            lstEntries.Dock = DockStyle.Fill;
            lstEntries.FormattingEnabled = true;
            lstEntries.ItemHeight = 15;
            lstEntries.Location = new Point(0, 0);
            lstEntries.Name = "lstEntries";
            lstEntries.Size = new Size(320, 601);
            lstEntries.TabIndex = 0;
            lstEntries.SelectedIndexChanged += LstEntries_SelectedIndexChanged;
            // 
            // rtxtContent
            // 
            rtxtContent.Dock = DockStyle.Fill;
            rtxtContent.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtxtContent.Location = new Point(0, 0);
            rtxtContent.Name = "rtxtContent";
            rtxtContent.Size = new Size(660, 601);
            rtxtContent.TabIndex = 0;
            rtxtContent.Text = "";
            rtxtContent.TextChanged += RtxtContent_TextChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 691);
            Controls.Add(splitMain);
            Controls.Add(pnlControls);
            MinimumSize = new Size(800, 500);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Animal Crossing String Table Editor (By_GBJ)";
            pnlControls.ResumeLayout(false);
            pnlControls.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}

#nullable restore
