using System.Drawing;
using System.Windows.Forms;
using ZXing;
using ZXing.Windows.Compatibility;

namespace ScanQRWin;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        KeyPreview = true;
        AllowDrop = true;

        btnBrowse.Click += BtnBrowse_Click;
        KeyDown += MainForm_KeyDown;
        pictureBox.DragEnter += PictureBox_DragEnter;
        pictureBox.DragDrop += PictureBox_DragDrop;
    }

    // ── Input: Browse ────────────────────────────────────────────────────────

    private void BtnBrowse_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tiff;*.tif",
            Title = "Select an image containing a QR code"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        SetStatus(Color.Gray, "Ready.");
        Bitmap? bmp = LoadBitmapFromFile(dlg.FileName);
        if (bmp is null) return;

        pictureBox.Image = bmp;
        ProcessBitmap(bmp, noQrMessage: "No QR code found in image.");
    }

    // ── Input: Paste ─────────────────────────────────────────────────────────

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.V)
        {
            SetStatus(Color.Gray, "Ready.");
            Image? img = Clipboard.GetImage();
            if (img is null)
            {
                SetStatus(Color.Red, "No image found in clipboard.");
                return;
            }

            Bitmap bmp = new(img);
            pictureBox.Image = bmp;
            ProcessBitmap(bmp, noQrMessage: "No QR code found in image.");
        }
    }

    // ── Input: Drag & Drop ───────────────────────────────────────────────────

    private void PictureBox_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data is not null && e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effect = DragDropEffects.Copy;
        else
            e.Effect = DragDropEffects.None;
    }

    private void PictureBox_DragDrop(object? sender, DragEventArgs e)
    {
        SetStatus(Color.Gray, "Ready.");
        string[]? files = e.Data?.GetData(DataFormats.FileDrop) as string[];
        if (files is null || files.Length == 0) return;

        Bitmap? bmp = LoadBitmapFromFile(files[0]);
        if (bmp is null) return;

        pictureBox.Image = bmp;
        ProcessBitmap(bmp, noQrMessage: "No QR code found in image.");
    }

    // ── Decode Pipeline ──────────────────────────────────────────────────────

    private Bitmap? LoadBitmapFromFile(string path)
    {
        try
        {
            return new Bitmap(path);
        }
        catch
        {
            SetStatus(Color.Red, "Could not load image file.");
            return null;
        }
    }

    private void ProcessBitmap(Bitmap bmp, string noQrMessage)
    {
        string? decoded = DecodeImage(bmp);
        if (decoded is not null)
        {
            txtResult.Text = decoded;
            SetStatus(Color.Green, "QR code decoded successfully.");
        }
        else
        {
            txtResult.Text = string.Empty;
            SetStatus(Color.Red, noQrMessage);
        }
    }

    private static string? DecodeImage(Bitmap bmp)
    {
        var reader = new BarcodeReader();
        var result = reader.Decode(bmp);
        return result?.Text;
    }

    // ── Status Helper ────────────────────────────────────────────────────────

    private void SetStatus(Color color, string message)
    {
        lblStatus.ForeColor = color;
        lblStatus.Text = message;
    }
}
