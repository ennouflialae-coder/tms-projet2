# TMS Project - Team Setup Guide

## For Teammates

### Prerequisites
- Windows
- .NET 10 SDK
- Git
- SQL Server LocalDB available on the machine

## Step 1: Clone the Project

Open PowerShell and run:

```powershell
git clone https://github.com/Wissalbenallam/EliteBook2.git
cd EliteBook2
```

## Step 2: Restore Dependencies

```powershell
dotnet restore
```

## Step 3: Run the Project

```powershell
dotnet run
```

Wait for a line like:

```text
Now listening on: https://localhost:7xxx
```

## Step 4: Verify Everything Works

1. Open the URL shown in the terminal.
2. Then open `/TestDatabase`.
3. You should see a successful database connection and seeded sample data.

## Project Notes

- Framework: `.NET 10`
- App type: `ASP.NET Core`
- Database: `SQL Server LocalDB`
- Connection string is in `appsettings.json`
- Sample data is initialized automatically on first run

## Common Commands

```powershell
dotnet restore
dotnet build
dotnet run
```

## Troubleshooting

### Packages are missing

```powershell
dotnet restore
dotnet build
```

### Build errors

```powershell
dotnet clean
dotnet build
dotnet run
```

### Database issue

Make sure LocalDB is available on the machine, then run the app again.

## Files That Should Not Be Committed

- `bin/`
- `obj/`
- `.vs/`
- `.dotnet/`
- `appsettings.Development.json`
