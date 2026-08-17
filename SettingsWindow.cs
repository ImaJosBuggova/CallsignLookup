using System.Configuration;

namespace CallsignLookup;

public class SettingsWindow : Form
{
    Postavke postavke;

    private readonly TabControl tabControl = new()
    {
        Dock = DockStyle.Fill
    };

    private readonly Button saveButton = new()
    {
        Text = "Spremi",
        Width = 100,
        Dock = DockStyle.Bottom
    };

    readonly CheckBox qrzEnabledCheckBox = new() { Text = "Omogući", Anchor = AnchorStyles.Left };
    readonly TextBox qrzUsernameTextBox = new() { Dock = DockStyle.Fill };
    readonly TextBox qrzPasswordTextBox = new() { Dock = DockStyle.Fill, UseSystemPasswordChar = true };

    readonly CheckBox hamqthEnabledCheckBox = new() { Text = "Omogući", Anchor = AnchorStyles.Left };
    readonly TextBox hamqthUsernameTextBox = new() { Dock = DockStyle.Fill };
    readonly TextBox hamqthPasswordTextBox = new() { Dock = DockStyle.Fill, UseSystemPasswordChar = true };

    readonly CheckBox qrzcqEnabledCheckBox = new() { Text = "Omogući", Anchor = AnchorStyles.Left };
    readonly TextBox qrzcqUsernameTextBox = new() { Dock = DockStyle.Fill };
    readonly TextBox qrzcqPasswordTextBox = new() { Dock = DockStyle.Fill, UseSystemPasswordChar = true };

    readonly CheckBox callookEnabledCheckBox = new() { Text = "Omogući", Anchor = AnchorStyles.Left };

    public SettingsWindow(Postavke postavke)
    {
        this.postavke = postavke;

        InitializeWindow();
        LoadSettings();
    }

    private void InitializeWindow()
    {
        Text = "Postavke";
        Width = 300;
        Height = 300;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        // Add tabs
        tabControl.TabPages.Add(CreateQRZTab());
        tabControl.TabPages.Add(CreateHamQTHTab());
        tabControl.TabPages.Add(CreateQRZCQTab());
        tabControl.TabPages.Add(CreateCallookTab());

        saveButton.Click += SaveButton_Click;

        Controls.Add(tabControl);
        Controls.Add(saveButton);
    }

    private TabPage CreateQRZTab()
    {
        var tabPage = new TabPage("QRZ");
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            AutoSize = true
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        panel.Controls.Add(qrzEnabledCheckBox, 0, 0);
        panel.Controls.Add(new Label { Text = "Korisničko ime:", Anchor = AnchorStyles.Left }, 0, 1);
        panel.Controls.Add(qrzUsernameTextBox, 1, 1);
        panel.Controls.Add(new Label { Text = "Lozinka:", Anchor = AnchorStyles.Left }, 0, 2);
        panel.Controls.Add(qrzPasswordTextBox, 1, 2);

        tabPage.Controls.Add(panel);
        return tabPage;
    }

    private TabPage CreateHamQTHTab()
    {
        var tabPage = new TabPage("HamQTH");
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            AutoSize = true
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        panel.Controls.Add(hamqthEnabledCheckBox, 0, 0);
        panel.Controls.Add(new Label { Text = "Korisničko ime:", Anchor = AnchorStyles.Left }, 0, 1);
        panel.Controls.Add(hamqthUsernameTextBox, 1, 1);
        panel.Controls.Add(new Label { Text = "Lozinka:", Anchor = AnchorStyles.Left }, 0, 2);
        panel.Controls.Add(hamqthPasswordTextBox, 1, 2);

        tabPage.Controls.Add(panel);
        return tabPage;
    }

    private TabPage CreateQRZCQTab()
    {
        var tabPage = new TabPage("QRZCQ");
        var panel = new TableLayoutPanel()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            AutoSize = true
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        panel.Controls.Add(qrzcqEnabledCheckBox, 0, 0);
        panel.Controls.Add(new Label { Text = "Korisničko ime:", Anchor = AnchorStyles.Left }, 0, 1);
        panel.Controls.Add(qrzcqUsernameTextBox, 1, 1);
        panel.Controls.Add(new Label { Text = "Lozinka:", Anchor = AnchorStyles.Left }, 0, 2);
        panel.Controls.Add(qrzcqPasswordTextBox, 1, 2);

        tabPage.Controls.Add(panel);
        return tabPage;
    }

    private TabPage CreateCallookTab()
    {
        var tabPage = new TabPage("Callook");
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            AutoSize = true
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        panel.Controls.Add(callookEnabledCheckBox, 0, 0);

        tabPage.Controls.Add(panel);
        return tabPage;
    }

    private void LoadSettings()
    {
        qrzEnabledCheckBox.Checked = postavke.QRZ;
        qrzUsernameTextBox.Text = postavke.QRZUsername;
        qrzPasswordTextBox.Text = postavke.QRZPassword;

        hamqthEnabledCheckBox.Checked = postavke.HamQTH;
        hamqthUsernameTextBox.Text = postavke.HamQTHUsername;
        hamqthPasswordTextBox.Text = postavke.HamQTHPassword;

        qrzcqEnabledCheckBox.Checked = postavke.QRZCQ;
        qrzcqUsernameTextBox.Text = postavke.QRZCQUsername;
        qrzcqPasswordTextBox.Text = postavke.QRZCQPassword;

        callookEnabledCheckBox.Checked = postavke.Callook;
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        try
        {
            postavke.QRZ = qrzEnabledCheckBox.Checked;
            postavke.QRZUsername = qrzUsernameTextBox.Text;
            postavke.QRZPassword = qrzPasswordTextBox.Text;

            postavke.HamQTH = hamqthEnabledCheckBox.Checked;
            postavke.HamQTHUsername = hamqthUsernameTextBox.Text;
            postavke.HamQTHPassword = hamqthPasswordTextBox.Text;

            postavke.QRZCQ = qrzcqEnabledCheckBox.Checked;
            postavke.QRZCQUsername = qrzcqUsernameTextBox.Text;
            postavke.QRZCQPassword = qrzcqPasswordTextBox.Text;

            postavke.Callook = callookEnabledCheckBox.Checked;

            // Save to file
            postavke.SaveChanges();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
