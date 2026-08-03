# Docker Desktop Troubleshooting (Windows)

## Your current error

If you previously saw:

```text
Docker Desktop is unable to start
engine linux/wsl failed to start: wsl is not installed
```

That indicates **WSL2 was not installed**. After `wsl --install` and reboot, Docker Desktop should start normally (`docker version` shows both Client and Server).

---

## Fix (recommended)

### 1. Install WSL2 (Administrator PowerShell)

```powershell
wsl --install
```

Or install components manually:

```powershell
dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart
dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart
```

Then **restart the computer**.

### 2. After reboot

```powershell
wsl --set-default-version 2
wsl --update
wsl --status
```

You should see a default distro (often Ubuntu) and WSL version 2.

### 3. Start Docker Desktop

- Open Docker Desktop from Start menu
- Wait until status shows **Docker Desktop is running** (whale icon steady, not animating)
- Verify:

```powershell
docker version
docker compose version
```

Both should show **Server** section without errors.

### 4. Run the MVP

```bash
cd prototype
docker compose up --build
./scripts/demo.sh
```

---

## If `wsl --install` fails

| Issue | Action |
|-------|--------|
| Corporate policy blocks WSL | Ask IT to enable WSL2 or use local dev path below |
| Virtualization disabled | Enable Intel VT-x / AMD-V in BIOS |
| Hyper-V conflict | Docker Desktop → Settings → General → confirm WSL2 backend |
| Old Windows build | Need Windows 10 21H2+ or Windows 11 for WSL2 |

---

## Alternative: run MVP without Docker

If you cannot install WSL immediately, see [RUN-LOCAL-NO-DOCKER.md](./RUN-LOCAL-NO-DOCKER.md) — requires local PostgreSQL + RabbitMQ installs and `dotnet run` in multiple terminals.

---

## Quick diagnostic commands

```powershell
wsl --status
docker version
Get-Service com.docker.service
```

Healthy output:

- `wsl --status` → WSL version 2, default distro running
- `docker version` → Client **and** Server both listed
- `com.docker.service` → Running (optional; WSL backend may not use this service)
