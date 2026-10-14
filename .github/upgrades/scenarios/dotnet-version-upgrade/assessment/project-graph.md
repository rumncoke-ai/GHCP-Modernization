# Projects Relationship Graph

[← Back to the assessment index](../assessment.md)

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>⚙️&nbsp;LegacyShop.Web.csproj</b><br/><small>net47</small>"]
    P2["<b>⚙️&nbsp;LegacyShop.Business.csproj</b><br/><small>net47</small>"]
    P3["<b>⚙️&nbsp;LegacyShop.Data.csproj</b><br/><small>net47</small>"]
    P4["<b>⚙️&nbsp;LegacyShop.Tests.csproj</b><br/><small>net47</small>"]
    P1 --> P2
    P1 --> P3
    P2 --> P3
    P4 --> P2
    P4 --> P3
    P4 --> P1
    click P1 "projects/LegacyShop.Web.md"
    click P2 "projects/LegacyShop.Business.md"
    click P3 "projects/LegacyShop.Data.md"
    click P4 "projects/LegacyShop.Tests.md"

```

