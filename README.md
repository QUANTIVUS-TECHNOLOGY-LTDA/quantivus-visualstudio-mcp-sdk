# Quantivus Visual Studio MCP SDK

Versioned contracts, schemas and compatibility checks shared by the Quantivus Visual Studio extension and external MCP hosts.

## Packages

- `Quantivus.VisualStudio.Mcp.Contracts` targets .NET Standard 2.0 so it can be consumed by Visual Studio VSIX projects on .NET Framework 4.8 and by modern .NET hosts.
- `Quantivus.VisualStudio.Mcp.Contracts.Tests` is an executable compatibility suite with no test-framework dependency.

## Build and validate

```powershell
dotnet restore Quantivus.VisualStudio.Mcp.Sdk.sln
dotnet build Quantivus.VisualStudio.Mcp.Sdk.sln -c Release --no-restore
dotnet run --project Quantivus.VisualStudio.Mcp.Contracts.Tests -c Release --no-build
dotnet pack Quantivus.VisualStudio.Mcp.Contracts -c Release --no-build
```

## Compatibility policy

- Tool and envelope changes require a protocol-version decision.
- New optional fields are backward compatible.
- Renaming or removing tools, fields or error codes requires a new protocol version.
- Consumers must reject unsupported protocol versions rather than guessing.

## Security

The contracts set a four-megabyte message limit. Implementations must additionally enforce pipe ACLs, timeouts, cancellation and explicit authorization for write or execution tools.

Licensed under Apache-2.0.
