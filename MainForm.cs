using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StringTableEditorCS.Models;

namespace StringTableEditorCS;

public class MainForm : Form
{
    private readonly StringTable _stringTable = new();
    private readonly ListBox _lstEntries;
    private readonly RichTextBox _rtxtContent;
    private readonly Button _btnOpenTable;
    private readonly Button _btnOpenData;
    private readonly Button _btnSave;
    private readonly Label _lblStatus;

    private string? _tablePath;
    private string? _dataPath;

    public MainForm()
    {
        Text = "Animal Crossing String Table Editor (By GBJ)";
        Size = new Size(1000, 700);
        StartPosition = FormStartPosition.CenterScreen;

        var pnlControls = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60
        };

        _btnOpenTable = new Button
        {
            Text = "Abrir Tabela (.bin)",
            Left = 10,
            Top = 10,
            Width = 150
        };
        _btnOpenTable.Click += BtnOpenTable_Click;

        _btnOpenData = new Button
        {
            Text = "Abrir Dados (.bin)",
            Left = 170,
            Top = 10,
            Width = 150
        };
        _btnOpenData.Click += BtnOpenData_Click;

        _btnSave = new Button
        {
            Text = "Salvar Alterações",
            Left = 330,
            Top = 10,
            Width = 150,
            Enabled = false
        };
        _btnSave.Click += BtnSave_Click;

        _lblStatus = new Label
        {
            Text = "Pronto",
            Left = 10,
            Top = 40,
            Width = 700
        };

        pnlControls.Controls.Add(_btnOpenTable);
        pnlControls.Controls.Add(_btnOpenData);
        pnlControls.Controls.Add(_btnSave);
        pnlControls.Controls.Add(_lblStatus);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterDistance = 320
        };

        _lstEntries = new ListBox
        {
            Dock = DockStyle.Fill
        };
        _lstEntries.SelectedIndexChanged += LstEntries_SelectedIndexChanged;

        _rtxtContent = new RichTextBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 12)
        };
        _rtxtContent.TextChanged += RtxtContent_TextChanged;

        split.Panel1.Controls.Add(_lstEntries);
        split.Panel2.Controls.Add(_rtxtContent);

        Controls.Add(split);
        Controls.Add(pnlControls);
    }

    private void BtnOpenTable_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "Arquivos Binários|*.bin|Todos os arquivos|*.*"
        };

        if (ofd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        _tablePath = ofd.FileName;
        _lblStatus.Text = $"Tabela: {Path.GetFileName(_tablePath)} - Agora abra os dados.";
        TryLoadFiles();
    }

    private void BtnOpenData_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "Arquivos Binários|*.bin|Todos os arquivos|*.*"
        };

        if (ofd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        _dataPath = ofd.FileName;
        _lblStatus.Text = $"Dados: {Path.GetFileName(_dataPath)}";
        TryLoadFiles();
    }

    private void TryLoadFiles()
    {
        if (string.IsNullOrWhiteSpace(_tablePath) || string.IsNullOrWhiteSpace(_dataPath))
        {
            return;
        }

        try
        {
            _stringTable.LoadTableFromFiles(_tablePath, _dataPath);
            _lstEntries.DataSource = null;
            _lstEntries.DataSource = _stringTable.Entries;
            _btnSave.Enabled = true;
            _lblStatus.Text = $"Carregado: {_stringTable.Entries.Count} entradas.";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao carregar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LstEntries_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_lstEntries.SelectedItem is not StringTableEntry entry)
        {
            return;
        }

        _rtxtContent.Text = entry.Content;
    }

    private void RtxtContent_TextChanged(object? sender, EventArgs e)
    {
        if (_lstEntries.SelectedItem is not StringTableEntry entry)
        {
            return;
        }

        entry.Content = _rtxtContent.Text;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_tablePath) || string.IsNullOrWhiteSpace(_dataPath))
        {
            return;
        }

        try
        {
            _stringTable.SaveTableToFiles(_tablePath, _dataPath);
            MessageBox.Show("Salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
