# ScanQRWin Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a .NET 9 WinForms app that decodes QR codes from images supplied via file browse, clipboard paste, or drag-and-drop, displaying the result inline.

**Architecture:** A single `MainForm` owns all UI and decode logic. Three input paths (browse, paste, drag-drop) each produce a `Bitmap` and funnel into a shared `DecodeImage(Bitmap)` method. ZXing.Net does the actual decoding. No layers, no abstractions beyond what's needed.

**Tech Stack:** .NET 9, Windows Forms, ZXing.Net 0.16.11, ZXing.Net.Bindings.Windows.Compatibility 0.16.14

---

## File Map

| File | Action | Responsibility |
|---|---|---|
| `ScanQRWin.csproj` | Create | Project definition, NuGet references |
| `Program.cs` | Create | WinForms entry point |
| `MainForm.cs` | Create | All UI event handlers + `DecodeImage` |
| `MainForm.Designer.cs` | Create | Control layout and wiring |

---

## Task 1: Scaffold the project

**Files:**
- Create: `ScanQRWin.csproj`
- Create: `Program.cs`

- [ ] **Step 1: Create the .csproj**

Create `ScanQRWin.csproj` with this exact content:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net9.0-windows</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <ApplicationIcon />
    <StartupObject>ScanQRWin.Program</StartupObject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="ZXing.Net" Version="0.16.11" />
    <PackageReference Include="ZXing.Net.Bindings.Windows.Compatibility" Version="0.16.14" />
  </ItemGroup>

</Project>
```

- [ ] **Step 2: Create Program.cs**

Create `Program.cs`:

```csharp
using System.Windows.Forms;

namespace ScanQRWin;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
```

- [ ] **Step 3: Restore NuGet packages**

Run from the project root:
```
dotnet restore
```
Expected: output ending with `Restore completed` and no errors.

- [ ] **Step 4: Commit**

```bash
git init
git add ScanQRWin.csproj Program.cs
git commit -m "feat: scaffold .NET 9 WinForms project with ZXing.Net"
```

---

## Task 2: Build the form layout (Designer)

**Files:**
- Create: `MainForm.Designer.cs`

- [ ] **Step 1: Create MainForm.Designer.cs**

Create `MainForm.Designer.cs`:

```csharp
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
```

- [ ] **Step 2: Verify it compiles (no MainForm.cs yet — create a stub)**

Create a temporary stub `MainForm.cs`:

```csharp
namespace ScanQRWin;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }
}
```

- [ ] **Step 3: Build**

```
dotnet build
```
Expected: `Build succeeded` with 0 errors.

- [ ] **Step 4: Commit**

```bash
git add MainForm.Designer.cs MainForm.cs
git commit -m "feat: add form layout with browse button, picture box, result textbox, status label"
```

---

## Task 3: Implement the decode pipeline and status helper

**Files:**
- Modify: `MainForm.cs` (replace stub with full implementation)

- [ ] **Step 1: Replace MainForm.cs with full implementation**

Replace `MainForm.cs` entirely:

```csharp
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
```

- [ ] **Step 2: Build**

```
dotnet build
```
Expected: `Build succeeded` with 0 errors. If you see a missing `ZXing.Windows.Compatibility` namespace error, confirm `ZXing.Net.Bindings.Windows.Compatibility` is in the `.csproj` and run `dotnet restore` first.

- [ ] **Step 3: Commit**

```bash
git add MainForm.cs
git commit -m "feat: implement browse/paste/drag-drop inputs and ZXing.Net decode pipeline"
```

---

## Task 4: Manual smoke test

There are no practical unit tests for WinForms UI event wiring — test manually.

- [ ] **Step 1: Run the app**

```
dotnet run
```
Expected: A window appears titled "ScanQRWin" with a "Browse Image..." button, a large image area, a result textbox, and a gray "Ready." label.

- [ ] **Step 2: Test Browse — valid QR image**

1. Find or generate a QR code PNG (e.g. use https://www.qr-code-generator.com/ to make one encoding "Hello, World!")
2. Click "Browse Image...", select the file
3. Expected: image shown in preview, result textbox shows `Hello, World!`, status label turns green: "QR code decoded successfully."

- [ ] **Step 3: Test Browse — image with no QR code**

1. Click "Browse Image...", select any plain photo (e.g. a landscape JPEG)
2. Expected: image shown, result textbox empty, status label turns red: "No QR code found in image."

- [ ] **Step 4: Test Browse — invalid file**

1. Click "Browse Image...", select a non-image file (e.g. rename a `.txt` to `.png` and try to open it)
2. Expected: status label turns red: "Could not load image file." No crash.

- [ ] **Step 5: Test Paste — image in clipboard**

1. Open the QR code PNG in any image viewer, select all and copy (Ctrl+A, Ctrl+C)
   *Or:* In Paint, open the QR PNG, Ctrl+A, Ctrl+C
2. Click on the ScanQRWin window to focus it, press Ctrl+V
3. Expected: image shown in preview, result decoded, status green.

- [ ] **Step 6: Test Paste — no image in clipboard**

1. Copy some text (e.g. Ctrl+C on a word in Notepad)
2. Focus ScanQRWin, press Ctrl+V
3. Expected: status red: "No image found in clipboard."

- [ ] **Step 7: Test Drag & Drop — valid QR image**

1. Open File Explorer, drag the QR code PNG onto the image preview area
2. Expected: image shown, result decoded, status green.

- [ ] **Step 8: Test Drag & Drop — non-image file**

1. Drag a `.txt` or `.exe` file onto the image preview area
2. Expected: status red: "Dropped file is not a supported image." No crash.

- [ ] **Step 9: Commit if all tests pass**

```bash
git add .
git commit -m "chore: manual smoke tests passed for all three input paths"
```

---

## Task 5: Final polish — window title and startup state

**Files:**
- Modify: `MainForm.cs`

- [ ] **Step 1: Verify initial state**

Run the app and confirm:
- `txtResult` is empty
- `lblStatus` shows "Ready." in gray
- `pictureBox` is blank

These are already set by the Designer and constructor — no code change needed unless something looks wrong.

- [ ] **Step 2: Ensure pictureBox drag events fire (not just form-level)**

The `DragEnter`/`DragDrop` events are wired to `pictureBox` in the constructor. Confirm `pictureBox.AllowDrop = true` is set in `MainForm.Designer.cs` (it is, per Task 2). No change needed.

- [ ] **Step 3: Final build and run**

```
dotnet build && dotnet run
```
Expected: clean build, app opens correctly.

- [ ] **Step 4: Final commit**

```bash
git add .
git commit -m "chore: final verification, all inputs working"
```
