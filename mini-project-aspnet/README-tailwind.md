# Tailwind CSS setup

This project now uses Tailwind CSS.

## Install dependencies

```powershell
npm.cmd install
```

## Build CSS once

```powershell
npm.cmd run build:css
```

## Watch mode (recommended during development)

```powershell
npm.cmd run watch:css
```

Tailwind input file: `Styles/tailwind.css`  
Generated output file: `wwwroot/css/tailwind.css`

> Keep the watch command running while editing `.cshtml` files so Tailwind classes are regenerated automatically.
