using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using StringTableEditorCS.Models;

namespace StringTableEditorCS
{
    public partial class MainForm : Form
    {
        private readonly StringTable _stringTable = new();
        private List<StringTableEntry> _filteredEntries = new List<StringTableEntry>();
        private string? _tablePath;
        private string? _dataPath;
        private string? _currentEditedFilePath;
        private bool _isUpdatingEditor;

        public MainForm()
        {
            InitializeComponent();
            ConfigureFormState();
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
            UpdateEditingFileStatus(_tablePath);
            ShowSearchFeedback("Tabela carregada. Selecione o arquivo de dados para iniciar a edição.", SystemColors.GrayText);
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
            UpdateEditingFileStatus(_dataPath);
            ShowSearchFeedback("Arquivo de dados selecionado.", SystemColors.GrayText);
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
                btnSave.Enabled = _stringTable.Entries.Count > 0;
                ApplySearchFilter();
                UpdateEditingFileStatus(_dataPath);
            }
            catch (Exception ex)
            {
                btnSave.Enabled = false;
                ShowSearchFeedback("Falha ao carregar os arquivos selecionados.", Color.Firebrick);
                MessageBox.Show($"Erro ao carregar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LstEntries_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateEditorFromSelection();
        }

        private void RtxtContent_TextChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingEditor || lstEntries.SelectedItem is not StringTableEntry entry)
            {
                return;
            }

            entry.Content = rtxtContent.Text;
            lstEntries.Refresh();
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
                RefreshEntriesList();
                UpdateEditingFileStatus(_dataPath);
                ShowSearchFeedback("Alterações salvas com sucesso.", Color.DarkGreen);
                MessageBox.Show("Salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowSearchFeedback("Falha ao salvar os arquivos em edição.", Color.Firebrick);
                MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        private void BtnClearSearch_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            txtSearch.Focus();
        }

        private void RefreshEntriesList()
        {
            ApplySearchFilter();
            lstEntries.Refresh();
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            ApplyStatusLabelText();
        }

        private void ConfigureFormState()
        {
            Resize += MainForm_Resize;
            rtxtContent.ReadOnly = true;
            btnClearSearch.Enabled = false;
            ShowSearchFeedback("Nenhuma entrada carregada.", SystemColors.GrayText);
            UpdateEditingFileStatus(null);
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            try
            {
                var searchText = txtSearch.Text.Trim();
                var selectedEntryId = lstEntries.SelectedItem is StringTableEntry selectedEntry ? selectedEntry.Id : (int?)null;
                var filteredEntries = string.IsNullOrWhiteSpace(searchText)
                    ? _stringTable.Entries.ToList()
                    : _stringTable.Entries.Where(entry => EntryMatchesSearch(entry, searchText)).ToList();

                _filteredEntries = filteredEntries;
                BindEntries(filteredEntries, selectedEntryId);
                UpdateSearchFeedback(searchText, filteredEntries.Count);
            }
            catch (Exception ex)
            {
                ShowSearchFeedback("Ocorreu um erro ao aplicar a pesquisa.", Color.Firebrick);
                MessageBox.Show($"Erro ao pesquisar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindEntries(List<StringTableEntry> entries, int? selectedEntryId)
        {
            _isUpdatingEditor = true;

            try
            {
                lstEntries.BeginUpdate();
                lstEntries.DataSource = null;
                lstEntries.DataSource = entries;

                if (selectedEntryId.HasValue)
                {
                    SelectEntryById(selectedEntryId.Value);
                }

                if (lstEntries.SelectedIndex == -1 && lstEntries.Items.Count > 0)
                {
                    lstEntries.SelectedIndex = 0;
                }
            }
            finally
            {
                lstEntries.EndUpdate();
                _isUpdatingEditor = false;
            }

            UpdateEditorFromSelection();
        }

        private void SelectEntryById(int selectedEntryId)
        {
            for (var index = 0; index < _filteredEntries.Count; index++)
            {
                if (_filteredEntries[index].Id == selectedEntryId)
                {
                    lstEntries.SelectedIndex = index;
                    return;
                }
            }
        }

        private void UpdateEditorFromSelection()
        {
            _isUpdatingEditor = true;

            try
            {
                if (lstEntries.SelectedItem is not StringTableEntry entry)
                {
                    rtxtContent.Clear();
                    rtxtContent.ReadOnly = true;
                    return;
                }

                rtxtContent.ReadOnly = false;

                if (!string.Equals(rtxtContent.Text, entry.Content, StringComparison.Ordinal))
                {
                    rtxtContent.Text = entry.Content;
                }
            }
            finally
            {
                _isUpdatingEditor = false;
            }
        }

        private bool EntryMatchesSearch(StringTableEntry entry, string searchText)
        {
            return entry.Id.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || entry.Content.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || entry.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateSearchFeedback(string searchText, int filteredCount)
        {
            btnClearSearch.Enabled = !string.IsNullOrWhiteSpace(searchText);

            if (string.IsNullOrWhiteSpace(searchText))
            {
                var message = _stringTable.Entries.Count == 0
                    ? "Nenhuma entrada carregada."
                    : $"Exibindo {_stringTable.Entries.Count} entrada(s).";

                ShowSearchFeedback(message, SystemColors.GrayText);
                return;
            }

            var searchColor = filteredCount > 0 ? Color.DarkGreen : Color.Firebrick;
            var searchMessage = filteredCount > 0
                ? $"{filteredCount} resultado(s) encontrado(s) para \"{searchText}\"."
                : $"Nenhum resultado encontrado para \"{searchText}\".";

            ShowSearchFeedback(searchMessage, searchColor);
        }

        private void ShowSearchFeedback(string message, Color color)
        {
            lblSearchStatus.ForeColor = color;
            lblSearchStatus.Text = message;
        }

        private void UpdateEditingFileStatus(string? filePath)
        {
            _currentEditedFilePath = string.IsNullOrWhiteSpace(filePath) ? null : Path.GetFullPath(filePath);
            ApplyStatusLabelText();
        }

        private void ApplyStatusLabelText()
        {
            var prefix = "Arquivo em edição: ";

            if (string.IsNullOrWhiteSpace(_currentEditedFilePath))
            {
                lblStatus.Text = $"{prefix}nenhum.";
                toolTipMain.SetToolTip(lblStatus, string.Empty);
                return;
            }

            var fullText = $"{prefix}{_currentEditedFilePath}";

            if (CanDisplayStatusText(fullText))
            {
                lblStatus.Text = fullText;
                toolTipMain.SetToolTip(lblStatus, fullText);
                return;
            }

            var fileName = Path.GetFileName(_currentEditedFilePath);
            var abbreviatedPathText = $"{prefix}...{Path.DirectorySeparatorChar}{fileName}";

            if (CanDisplayStatusText(abbreviatedPathText))
            {
                lblStatus.Text = abbreviatedPathText;
                toolTipMain.SetToolTip(lblStatus, fullText);
                return;
            }

            lblStatus.Text = FitTextToLabel($"{prefix}{fileName}");
            toolTipMain.SetToolTip(lblStatus, fullText);
        }

        private bool CanDisplayStatusText(string text)
        {
            if (lblStatus.ClientSize.Width <= 0)
            {
                return true;
            }

            return TextRenderer.MeasureText(text, lblStatus.Font).Width <= lblStatus.ClientSize.Width;
        }

        private string FitTextToLabel(string text)
        {
            if (CanDisplayStatusText(text))
            {
                return text;
            }

            const string ellipsis = "...";
            var length = text.Length;

            while (length > 0)
            {
                var candidate = text.Substring(0, length) + ellipsis;

                if (CanDisplayStatusText(candidate))
                {
                    return candidate;
                }

                length--;
            }

            return ellipsis;
        }
    }
}
