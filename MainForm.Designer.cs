namespace ScanQRWin;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.Button btnBrowse;
    private System.Windows.Forms.PictureBox pictureBox;
    private System.Windows.Forms.Label lblResult;
    private System.Windows.Forms.TextBox txtResult;
    private System.Windows.Forms.Label lblStatus;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        btnBrowse = new System.Windows.Forms.Button();
        pictureBox = new System.Windows.Forms.PictureBox();
        lblResult = new System.Windows.Forms.Label();
        txtResult = new System.Windows.Forms.TextBox();
        lblStatus = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
        SuspendLayout();

        // btnBrowse
        btnBrowse.Location = new System.Drawing.Point(12, 12);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new System.Drawing.Size(120, 30);
        btnBrowse.TabIndex = 0;
        btnBrowse.Text = "Browse Image...";
        btnBrowse.UseVisualStyleBackColor = true;

        // pictureBox
        pictureBox.AllowDrop = true;
        pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        pictureBox.Location = new System.Drawing.Point(12, 55);
        pictureBox.Name = "pictureBox";
        pictureBox.Size = new System.Drawing.Size(460, 260);
        pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        pictureBox.TabIndex = 1;
        pictureBox.TabStop = false;

        // lblResult
        lblResult.AutoSize = true;
        lblResult.Location = new System.Drawing.Point(12, 328);
        lblResult.Name = "lblResult";
        lblResult.Size = new System.Drawing.Size(46, 15);
        lblResult.Text = "Result:";

        // txtResult
        txtResult.Location = new System.Drawing.Point(12, 346);
        txtResult.Multiline = true;
        txtResult.Name = "txtResult";
        txtResult.ReadOnly = true;
        txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        txtResult.Size = new System.Drawing.Size(460, 80);
        txtResult.TabIndex = 2;

        // lblStatus
        lblStatus.AutoSize = true;
        lblStatus.ForeColor = System.Drawing.Color.Gray;
        lblStatus.Location = new System.Drawing.Point(12, 438);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new System.Drawing.Size(38, 15);
        lblStatus.Text = "Ready.";

        // MainForm
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(484, 471);
        Controls.Add(btnBrowse);
        Controls.Add(pictureBox);
        Controls.Add(lblResult);
        Controls.Add(txtResult);
        Controls.Add(lblStatus);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "MainForm";
        Text = "ScanQRWin";
        ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
