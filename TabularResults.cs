using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;

namespace AVBible.TabularResults
{
    class TableColor
    {
        private string text;
        private UInt32? rgb;

        public Brush color
        {
            get
            {
                return this.rgb.HasValue
                    ? new SolidColorBrush(Color.FromArgb((byte)((this.rgb.Value >> 24) & 0xFF), (byte)((this.rgb.Value >> 16) & 0xFF), (byte)((this.rgb.Value >> 8) & 0xFF), (byte)(this.rgb.Value & 0xFF)))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString(this.text));
            }
        }
        public TableColor(string value)
        {
            this.text = value;
            this.rgb = this.text.StartsWith("#") ? UInt32.Parse(this.text.Substring(1), System.Globalization.NumberStyles.HexNumber) : null;
        }
    }
    class TableRow : CustomTableStyle
    {
        private CustomTable Table;
        public bool IsHeader { get; internal set; }
        public string[] Content { get; internal set; }

        public TableRow(CustomTable table, string style) : base(style)
        {
            this.Table = table;
        }
        public TableColor ActualForeground
        {
            get
            {
                return this.Foreground ?? this.Table.Foreground ?? new TableColor("black");
            }
        }
        public TableColor ActualBackground
        {
            get
            {
                return this.Background ?? this.Table.Background ?? new TableColor("gray");
            }
        }
        public byte ActualFontSize
        {
            get
            {
                if (this.FontSize > 0)
                    return this.FontSize;
                if (this.Table.FontSize > 0)
                    return this.Table.FontSize;
                if (this.Table.Rows.Count > 0 && this.Table.Rows[0].FontSize > 0)
                    return this.Table.Rows[0].FontSize;
                return 24;
            }
        }
        public string[] ActualFontFamilies
        {
            get
            {
                if (this.FontFamilies.Length > 0)
                    return this.FontFamilies;
                if (this.Table.FontFamilies.Length > 0)
                    return this.Table.FontFamilies;
                if (this.Table.Rows.Count > 0 && this.Table.Rows[0].FontFamilies.Length > 0)
                    return this.Table.Rows[0].FontFamilies;
                return ["calibri"];
            }
        }
    }
    abstract class CustomTableStyle
    {
        public static readonly string[] TR = ["<tr ", "</tr>"];
        public static readonly string[] TD = ["<td>", "</td>"];
        public string Style { get; protected set; }

        public CustomTableStyle(string input)
        {
            this.Style = string.Empty;

            int idx = input.IndexOf("style='");
            if (idx >= 0 && idx < input.Length - 1)
            {
                idx += "style='".Length;
                int end = input.IndexOf("'", idx + 1);

                if (end > idx)
                {
                    this.Style = input.Substring(idx, end - idx);
                }
            }
        }
        public TableColor? Foreground
        {
            get
            {
                int idx = this.Style.IndexOf(";color:");
                if (idx < 0 && this.Style.StartsWith("color:"))
                    idx = "color:".Length;
                else if (idx > 0)
                    idx += ";color:".Length;

                int end = idx >= 0 && idx < this.Style.Length - 1 ? this.Style.IndexOf(';', idx + 1) : this.Style.Length;
                if (idx >= 0 && end > idx)
                {
                    return new TableColor(this.Style.Substring(idx, end - idx));
                }
                return null;
            }
        }
        public TableColor? Background
        {
            get
            {
                int idx = this.Style.IndexOf("background-color:");
                if (idx >= 0)
                    idx += "background-color:".Length;

                int end = idx >= 0 && idx < this.Style.Length - 1 ? this.Style.IndexOf(';', idx + 1) : this.Style.Length;
                if (idx >= 0 && end > idx)
                {
                    return new TableColor(this.Style.Substring(idx, end - idx));
                }
                return null;
            }
        }
        public byte FontSize
        {
            get
            {
                int idx = this.Style.IndexOf("font-size:");
                if (idx >= 0)
                    idx += "font-size:".Length;

                int end = idx >= 0 && idx < this.Style.Length - 1 ? this.Style.IndexOf("px", idx + 1) : -1;
                if (idx >= 0 && end > idx)
                {
                    return byte.Parse(this.Style.Substring(idx, end - idx));
                }
                return 0;
            }
        }
        public string[] FontFamilies
        {
            get
            {
                int idx = this.Style.IndexOf("font-family:");
                if (idx > 0)
                    idx += "font-family:".Length;

                int end = idx >= 0 && idx < this.Style.Length - 1 ? this.Style.IndexOf(';', idx + 1) : this.Style.Length;
                if (idx >= 0 && end > idx)
                {
                    return this.Style.Substring(idx, end - idx).Split(',');
                }
                return new string[0];
            }
        }
    }
    class CustomTable : CustomTableStyle
    {
        public string Style { get; private set; }
        public List<TableRow> Rows { get; private set; }

        public CustomTable(string input) : base(input)
        {
            this.Rows = new();
        }
        public bool AddRow(string input, bool isHeader = false)
        {
            TableRow row = new(this, input);
            string[] fields = input.Split(TD, StringSplitOptions.RemoveEmptyEntries);

            if (fields.Length > 1)
            {
                row.Content = new string[fields.Length - 1];
                for (int i = 1; i < fields.Length; i++)
                {
                    row.Content[i - 1] = fields[i];
                }
                row.IsHeader = isHeader;
                this.Rows.Add(row);
#if DEBUG
                var test1 = row.ActualForeground;
                var test2 = row.ActualBackground;
                var test3 = row.ActualFontSize;
                var test4 = row.ActualFontFamilies;
#endif
                return true;
            }
            return false;
        }
        public bool Render(ResultsWindow window)
        {
            var table = window.ResultsFlowTable;
            table.BorderThickness = new(3.0);
            if (this.Rows.Count >= 1 && this.Rows[0].Content.Length >= 1)
            {
                if (this.Background != null)
                {
                    window.Background = this.Background.color;
                    table.Background = this.Background.color;
                }
                if (this.Foreground != null)
                {
                    window.Foreground = this.Foreground.color;
                    table.Foreground = this.Foreground.color;
                    table.BorderBrush = this.Foreground.color;
                }

                // Add columns definitions
                for (int c = 0; c < this.Rows[0].Content.Length; c++)
                {
                    table.Columns.Add(new TableColumn());
                }
                // Add records
                TableRowGroup results = new();
                for (int r = 0; r < this.Rows.Count; r++)
                {
                    System.Windows.Documents.TableRow record = new();
                    var row = this.Rows[r];
                    for (int c = 0; c < row.Content.Length; c++)
                    {
                        Run run = new(row.Content[c]);
                        Paragraph paragraph = new Paragraph(run);
                        paragraph.FontWeight = row.IsHeader ? FontWeights.Bold : FontWeights.Normal;
                        paragraph.FontSize = row.ActualFontSize;
                        paragraph.Background = row.ActualBackground.color;
                        paragraph.Foreground = row.ActualForeground.color;
                        TableCell cell = new TableCell(paragraph);
                        record.Cells.Add(cell);
                    }
                    results.Rows.Add(record);
                }
                table.RowGroups.Add(results);
                return true;
            }
            return false;
        }
    }
}