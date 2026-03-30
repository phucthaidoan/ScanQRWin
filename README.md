# ScanQRWin

A .NET 9 Windows Forms desktop app that decodes QR codes from images.

## Features

- **Browse** — Open an image file via file dialog (PNG, JPG, BMP, GIF, TIFF)
- **Paste** — Press `Ctrl+V` to paste an image directly from clipboard
- **Drag & Drop** — Drag an image file onto the preview area
- Decoded text is displayed inline with color-coded status feedback

## Requirements

- Windows 11
- [.NET 9 Runtime](https://dotnet.microsoft.com/download/dotnet/9.0)

## Run

```
dotnet run
```

## Build

```
dotnet build
```

## Dependencies

- [ZXing.Net](https://github.com/micjahn/ZXing.Net) — QR code decoding

---

> Vibe-coded using [Superpowers](https://github.com/obra/superpowers) in [Claude Code](https://claude.ai/code).
