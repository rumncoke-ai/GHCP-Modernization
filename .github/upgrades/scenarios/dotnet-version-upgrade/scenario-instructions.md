# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: .NET 10 (`net10.0`)
- **Data Access Strategy**: Retain EF6 for the initial .NET 10 release; defer EF Core
- **Database Platform**: Retain SQL Server
- **Application Authentication**: Preserve anonymous access

## Source Control
- **Source Branch**: legacy-update
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Upgrade Options

### Strategy
- Upgrade Strategy: Bottom-Up

### Project Structure
- Web Projects: Side-by-side
- Class Libraries: Multi-targeting
- Package Management: Per-Project (defer CPM to post-migration)

### Compatibility
- Unsupported Packages: Resolve Inline (1 incompatible package)
- Unsupported API Handling: Fix Inline
- System.Web Adapters: Direct Migration to ASP.NET Core APIs

### Modernization
- Assembly Binding Redirects: Remove Binding Redirects
- Nullable Reference Types: Leave Disabled
- Entity Framework: Keep EF6

### Reliability
- Test Coverage: Generate

## Test Baseline
- Status: enabled
- Projects:
  - LegacyShop.Data\LegacyShop.Data.csproj
  - LegacyShop.Web\LegacyShop.Web.csproj
- Test projects:
  - LegacyShop.Tests\LegacyShop.Tests.csproj

## Strategy
**Selected**: Bottom-Up
**Rationale**: Four .NET Framework 4.7 projects form a four-tier dependency graph, and the Framework-to-.NET 10 boundary requires dependency-first validation.

### Execution Constraints
- Follow strict dependency ordering: Data, Business, the side-by-side Web migration, then Tests; do not start a higher tier until the preceding tier validates.
- Convert eligible non-web projects to SDK style on their current TFM before changing target frameworks; keep the existing Framework web project old-style.
- Validate restore, build, and relevant tests at every tier, and confirm remaining Framework consumers still build before proceeding.
- Resolve the incompatible package and all API changes inline; do not create compatibility stubs or deferred resolution tasks.
- Keep per-project package versions during migration and retain EF6, SQL Server, anonymous application access, and nullable-disabled behavior.

### Side-by-Side Web Migration Constraints
- Scaffold task must complete and validate (builds, stub 200 response) before migrate starts
- Old Framework project remains live and deployable throughout entire migrate phase
- Migrate task will be broken into subtasks at execution time — load migrating-aspnet-framework-to-core skill
- Libraries in migrate task scope are handled in dependency order before web layer assets
- Reference cleanup (test projects, multi-targeting) is part of migrate, not a separate task
- Old project is NOT deleted by the agent — documented as post-upgrade step for user
- If both hosts share one database, schema changes are governed by managing-shared-database-schema: one toolchain owns schema deployment, changes stay additive, and the EF6 `Migrations/` folder is not deleted while the old host is live
- Background jobs that write shared state run on one executor at a time across both hosts while its lease is live, governed by migrating-inprocess-background-jobs: the Framework copy takes the shared guard before the Core copy is enabled, and delivery is at-least-once, so each job's effects are idempotent or fenced
