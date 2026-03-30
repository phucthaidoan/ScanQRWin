# ScanQRWin — Design Spec

**Date:** 2026-03-30
**Status:** Approved

---

## Overview

A .NET 9 Windows Forms desktop application that decodes QR codes from images. The user can supply an image via file browser, clipboard paste, or drag-and-drop. The decoded text is displayed on the form with inline status feedback.

---

## Tech Stack

- **Platform:** .NET 9, Windows Forms
- **QR Decoder:** ZXing.Net 0.16.11 + ZXing.Net.Bindings.Windows.Compatibility 0.16.14
- **Imaging:** System.Drawing.Bitmap (built into WinForms)

---

## Project Structure

```
ScanQRWin/
├── ScanQRWin.csproj
├── Program.cs              # WinForms entry point
├── MainForm.cs             # All UI and decode logic
└── MainForm.Designer.cs    # Designer-generated layout
```

No additional abstractions. `DecodeImage(Bitmap bmp)` is a private method on `MainForm` — it wraps a single `BarcodeReader.Decode()` call and is not worth extracting.

---

## UI Layout

```
┌─────────────────────────────────────────┐
│  [Browse Image...]                       │  Button
│                                         │
│  ┌─────────────────────────────────┐    │
│  │                                 │    │
│  │        Image Preview            │    │  PictureBox (SizeMode: Zoom)
│  │                                 │    │
│  └─────────────────────────────────┘    │
│                                         │
│  Result:                                │
│  ┌─────────────────────────────────┐    │
│  │ (decoded text here)             │    │  TextBox (ReadOnly, Multiline)
│  └─────────────────────────────────┘    │
│                                         │
│  ● Status message (red/green/gray)      │  Label
└─────────────────────────────────────────┘
```

### Controls

| Control | Type | Notes |
|---|---|---|
| `btnBrowse` | Button | Opens OpenFileDialog |
| `pictureBox` | PictureBox | SizeMode = Zoom, AllowDrop = true |
| `txtResult` | TextBox | ReadOnly = true, Multiline = true |
| `lblStatus` | Label | Color reflects result state |

---

## Input Methods

### Browse (File Dialog)
- Button click opens `OpenFileDialog`
- Filter: `"Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tiff;*.tif"`
- On confirm: load bitmap, run decode pipeline

### Paste (Clipboard)
- `MainForm.KeyPreview = true`
- Handle `KeyDown`: detect `Ctrl+V`
- Call `Clipboard.GetImage()` → cast to `Bitmap`
- If null: show "No image found in clipboard." in red

### Drag & Drop
- `MainForm.AllowDrop = true`
- `DragEnter`: accept if `e.Data.GetDataPresent(DataFormats.FileDrop)`
- `DragDrop`: get file path array, take first entry, attempt to load as `Bitmap`
- If not a valid image: show "Dropped file is not a supported image." in red

---

## Decode Pipeline

All three input methods funnel into the same pipeline:

```
Bitmap
  → DecodeImage(Bitmap bmp)
      → new BarcodeReader().Decode(bmp)
  → Result?
      Yes → txtResult.Text = result.Text
            lblStatus: green, "QR code decoded successfully."
      No  → txtResult.Text = ""
            lblStatus: red, context-specific message
```

Decoding runs synchronously on the UI thread. ZXing.Net decode on a typical desktop image completes in well under 100ms — no async needed.

---

## Status Label Behavior

| State | Color | Message |
|---|---|---|
| Initial | Gray | "Ready." |
| Success | Green | "QR code decoded successfully." |
| No QR found | Red | "No QR code found in image." |
| Bad file | Red | "Could not load image file." |
| No clipboard image | Red | "No image found in clipboard." |
| Bad drag-drop | Red | "Dropped file is not a supported image." |

Status is cleared (reset to gray "Ready.") each time a new decode attempt begins.

---

## Error Handling

- All `Bitmap` loading is wrapped in `try/catch` to handle corrupted or unsupported files
- ZXing returns `null` (not an exception) when no QR code is found — handled as a no-result case
- No unhandled exceptions should surface to the user

---

## Out of Scope

- Copying decoded result to clipboard
- Opening URLs automatically
- Scanning from webcam/camera
- Generating QR codes
- Multi-code detection in a single image
