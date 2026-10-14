# LegacyShop.Business\LegacyShop.Business.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net47
- **Proposed Target Framework:** net10.0
- **SDK-style**: False
- **Project Kind:** ClassicClassLibrary
- **Dependencies**: 1
- **Dependants**: 2
- **Number of Files**: 9
- **Number of Files with Incidents**: 1
- **Lines of Code**: 526
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

## Related Projects

**Depends on (1)** — projects this one references:

- [C:\Users\ruchowdhury\OneDrive - Microsoft\Desktop\GHCP Demo\GHCP-Modernization\LegacyShop.Data\LegacyShop.Data.csproj](../projects/LegacyShop.Data.md)

**Depended on by (2)** — projects that reference this one:

- [C:\Users\ruchowdhury\OneDrive - Microsoft\Desktop\GHCP Demo\GHCP-Modernization\LegacyShop.Tests\LegacyShop.Tests.csproj](../projects/LegacyShop.Tests.md)
- [C:\Users\ruchowdhury\OneDrive - Microsoft\Desktop\GHCP Demo\GHCP-Modernization\LegacyShop.Web\LegacyShop.Web.csproj](../projects/LegacyShop.Web.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P1["<b>⚙️&nbsp;LegacyShop.Web.csproj</b><br/><small>net47</small>"]
        P4["<b>⚙️&nbsp;LegacyShop.Tests.csproj</b><br/><small>net47</small>"]
        click P1 "../projects/LegacyShop.Web.md"
        click P4 "../projects/LegacyShop.Tests.md"
    end
    subgraph current["LegacyShop.Business.csproj"]
        MAIN["<b>⚙️&nbsp;LegacyShop.Business.csproj</b><br/><small>net47</small>"]
        click MAIN "../projects/LegacyShop.Business.md"
    end
    subgraph downstream["Dependencies (1)"]
        P3["<b>⚙️&nbsp;LegacyShop.Data.csproj</b><br/><small>net47</small>"]
        click P3 "../projects/LegacyShop.Data.md"
    end
    P1 --> MAIN
    P4 --> MAIN
    MAIN --> P3

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 367 |  |
| ***Total APIs Analyzed*** | ***367*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| EntityFramework | 6.4.4 | 6.5.2 | 🟡 Potential | NuGet package upgrade is recommended |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

## Binding Redirect Configuration

| Rule | Severity | Details | Recommendation |
| :--- | :---: | :--- | :--- |
| AutoGenerateBindingRedirects not set and no manual redirects | 🟡 Potential | AutoGenerateBindingRedirects is not set in LegacyShop.Business.csproj, no manual redirects found | Explicitly enable <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects> or add manual binding redirects. |

