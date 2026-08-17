using System.Diagnostics;
using MAES.Core;

namespace CallsignLookup;

public class MainWindow : Form
{
    Postavke postavke = AppData.Load<Postavke>("postavke.json");

    readonly ToolStripButton loadButton = new ()
    {
        Text = "Učitaj"
    };

    readonly ToolStripButton validateButton = new ()
    {
        Text = "Validiraj",
        Enabled = false
    };

    readonly ToolStripButton settingsButton = new ()
    {
        Text = "Postavke"
    };

    readonly DataGridView grid = new ()
    {
        Dock = DockStyle.Fill,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        BackgroundColor = SystemColors.Control,
        ReadOnly = true,
        AllowUserToResizeRows = false,
        AllowUserToResizeColumns = false
    };

    readonly StatusStrip statusStrip = new ();

    readonly ToolStripLabel ukupnoVezaLabel = new ();

    public MainWindow()
    {
        loadButton.Click += ucitajLog;
        validateButton.Click += validate;
        settingsButton.Click += OpenSettings;
        grid.RowPrePaint += gridRowPrePaint;

        ToolStrip toolStrip = new ()
        {
            GripStyle = ToolStripGripStyle.Hidden
        };

        toolStrip.Items.Add(loadButton);
        toolStrip.Items.Add(validateButton);
        toolStrip.Items.Add(new ToolStripSeparator());
        toolStrip.Items.Add(settingsButton);
        
        statusStrip.Items.Add(ukupnoVezaLabel);
        
        grid.AutoGenerateColumns = false;
        grid.Columns.AddRange([
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Datum",
                DataPropertyName = nameof(Veza.Datum),
                Width = 100
            },
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Pozivni znak",
                DataPropertyName = nameof(Veza.PozivniZnak),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            },
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Bazni znak",
                DataPropertyName = nameof(Veza.BazniPozivniZnak),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            },
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Kont.",
                DataPropertyName = nameof(Veza.Continent),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            },
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Poslano",
                DataPropertyName = nameof(Veza.Poslano),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            },
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Primljeno",
                DataPropertyName = nameof(Veza.Primljeno),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            },
            new DataGridViewTextBoxColumn
            {
                HeaderText = "Validirao",
                DataPropertyName = nameof(Veza.Validirao),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            }
        ]);

        Controls.Add(grid);
        Controls.Add(toolStrip);
        Controls.Add(statusStrip);

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Text = "Callsign Lookup";
    }

    async void ucitajLog(object? sender, EventArgs? e)
    {
        using OpenFileDialog dialog = new ();

        if(dialog.ShowDialog() != DialogResult.OK) return;

        var veze = await dialog.FileName.UcitajLog();

        grid.DataSource = null;
        grid.DataSource = veze;

        ukupnoVezaLabel.Text = $"Učitano {veze.Count} veza";
        validateButton.Enabled = true;
    }

    async void validate(object? sender, EventArgs? e)
    {
        if(grid.DataSource is not List<Veza> veze) return;

        using var client = new HttpClient();

        Stopwatch sw = new ();
        sw.Start();

        for(int i = 0; i < veze.Count; i++)
        {
            ukupnoVezaLabel.Text = $"[{i+1}/{veze.Count}] Provjeravam {veze[i].PozivniZnak}";

            if(postavke.QRZ && await veze[i].SearchQRZ(client, postavke.QRZUsername, postavke.QRZPassword)) veze[i].Validirao = "qrz.com";
            else if(postavke.HamQTH && await veze[i].SearchHamQTH(client, postavke.HamQTHUsername, postavke.HamQTHPassword)) veze[i].Validirao = "hamqth.com";
            else if(postavke.QRZCQ && await veze[i].SearchQRZCQ(client, postavke.QRZCQUsername, postavke.QRZCQPassword)) veze[i].Validirao = "qrzcq.com";
            else if(postavke.Callook && await veze[i].SearchCallook(client)) veze[i].Validirao = "callook.info";

            veze[i].Checked = true;
        }

        sw.Stop();

        grid.Refresh();

        var nepronadeno = veze.Count(x => string.IsNullOrWhiteSpace(x.Validirao));
        ukupnoVezaLabel.Text = $"Pronađeno {veze.Count - nepronadeno} od {veze.Count} veza (nedostaje {nepronadeno}) u {sw.Elapsed:mm\\:ss}";
    }

    void gridRowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
    {
        if (grid.Rows[e.RowIndex].DataBoundItem is not Veza veza)
            return;
    
        if (veza.Checked && string.IsNullOrWhiteSpace(veza.Validirao))
        {
            grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
            grid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
        }
        else
        {
            grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            grid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
        }
    }

    void OpenSettings(object? sender, EventArgs e)
    {
        using var settingsForm = new SettingsWindow(postavke);
        settingsForm.ShowDialog(this);
    }
}
