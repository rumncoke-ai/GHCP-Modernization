# LegacyShop .NET 10 Upgrade Plan

Upgrade `LegacyShop.sln` from .NET Framework 4.7 to .NET 10 while preserving observable behavior, anonymous application access, SQL Server, and EF6 for the initial release. The assessment covers four non-SDK-style projects, 15 NuGet packages, 412 findings, and a high-risk ASP.NET MVC 5 web application that must move to ASP.NET Core rather than be retargeted in place.

## Upgrade Options

| Option | Selected | Why |
|--------|----------|-----|
| Upgrade Strategy | Bottom-Up | Four .NET Framework projects form a dependency chain that requires tier-by-tier migration and validation. |
| Web Projects | Side-by-side | LegacyShop.Web is high risk with 367 API issues and requires replacing ASP.NET MVC 5 and System.Web incrementally. |
| Class Libraries | Multi-targeting | LegacyShop.Data and LegacyShop.Business must support the existing Framework host and new ASP.NET Core host during migration. |
| Package Management | Per-Project (defer CPM to post-migration) | The projects use packages.config and need controlled package divergence while Framework and modern targets coexist. |
| Unsupported Packages | Resolve Inline | The single incompatible package can be resolved in its owning migration task without deferred stubs. |
| Unsupported API Handling | Fix Inline | All incompatible APIs will be resolved in their owning tasks so no compatibility stubs remain. |
| System.Web Adapters | Direct Migration to ASP.NET Core APIs | The approved approach avoids a compatibility shim and moves web behavior directly to native ASP.NET Core APIs. |
| Assembly Binding Redirects | Remove Binding Redirects | Modern .NET does not use Framework binding redirects. |
| Nullable Reference Types | Leave Disabled | Nullable warnings would compound an already high-risk web migration. |
| Entity Framework | Keep EF6 | Retaining EF6 separates the runtime upgrade from an ORM migration. |
| Test Coverage | Generate | Characterization coverage will protect the Data and Web projects before their high-risk changes. |

### Selected Strategy
**Bottom-Up (Dependency-First)** — Upgrade from leaf nodes to root applications, tier by tier.
**Rationale**: 4 projects with a 4-tier dependency graph.

## Dependency Order

```text
Tier 4: LegacyShop.Tests
          ↓
Tier 3: LegacyShop.Web
          ↓
Tier 2: LegacyShop.Business
          ↓
Tier 1: LegacyShop.Data
```

- **Tier 1 — Data**: `LegacyShop.Data` has no internal dependencies. It completes when its SDK conversion, EF6 package update, multi-targeting, configuration boundary, and focused tests validate without changing the shared SQL Server schema unexpectedly.
- **Tier 2 — Business**: `LegacyShop.Business` depends on Data. It completes when it builds and tests against the validated Data targets and unnecessary direct EF references have been assessed.
- **Tier 3 — Web**: `LegacyShop.Web` depends on Business and Data. Its side-by-side milestone completes when the ASP.NET Core host owns the migrated routes and behavior while the Framework host remains deployable.
- **Tier 4 — Tests**: `LegacyShop.Tests` depends on every production project. It completes with the Web migration's reference cleanup and passes against the modern project set.

### 01-generate-test-baseline: Generate behavior-locking test coverage

Generate a passing pre-upgrade baseline for `LegacyShop.Data` and `LegacyShop.Web`, using `LegacyShop.Tests` as the existing test project and adding only the coverage needed to protect current database behavior, routes, CRUD flows, validation, anti-forgery behavior, and anonymous access. The assessment recommends coverage for Data and Web, with Web carrying 367 incompatible API usages and the highest migration risk.

The baseline must capture externally observable behavior without introducing modernization changes. Record the baseline projects and results so the same suite can be replayed after the migration.

**Done when**: Generated and existing baseline tests pass on the original .NET Framework implementation, and their project paths and expected results are recorded for post-upgrade validation.

### 02-upgrade-prerequisites: Verify the .NET 10 toolchain and migration prerequisites

Verify that the repository can use the .NET 10 SDK, that any `global.json` is compatible, and that the Windows build environment can still restore and build the original .NET Framework 4.7 solution. Confirm the baseline test tooling, NuGet sources, SQL Server test environment, and branch state before structural changes begin.

This task establishes a reproducible starting point for a mixed Framework/modern solution. It must not alter application behavior or begin project conversion.

**Done when**: The required SDK and build tools are available, restore succeeds for the current solution, the original build and baseline tests are reproducible, and no prerequisite blocks the upgrade.

### 03-sdk-project-conversion: Convert eligible non-web projects to SDK style

Convert `LegacyShop.Data`, `LegacyShop.Business`, and `LegacyShop.Tests` to SDK-style projects while they remain on their current .NET Framework target. Replace their `packages.config` and assembly `HintPath` references with per-project `PackageReference` entries, preserving explicit content and compile behavior where SDK defaults differ. Do not convert the existing `LegacyShop.Web` project because the selected side-by-side approach keeps that Framework host intact.

All three projects are currently non-SDK-style, and their conversion must remain separate from target-framework and API changes. Preserve EF6 references, MSTest behavior, assembly metadata, and the existing dependency graph; binding redirects are removed when their modern target no longer requires them rather than copied forward.

**Done when**: Data, Business, and Tests use SDK-style project files on their existing Framework target, restore successfully with per-project package versions, and the unchanged Framework solution and tests still build and pass.

### 04-foundation-libraries: Multi-target Data and Business for .NET 10

Upgrade the Tier 1 and Tier 2 libraries, `LegacyShop.Data` and `LegacyShop.Business`, in strict dependency order by adding .NET 10 targets while retaining the Framework targets needed by the live web host. Resolve all package and API issues inline, update EF6 to a supported version, retain SQL Server, and externalize connection-string construction without changing mappings, migration history, cascade behavior, or seed semantics. `#skill:managing-shared-database-schema`

Data has six source-incompatible configuration API usages and EF6 initialization risks; Business has no incompatible APIs but depends directly on Data and may carry unnecessary EF references. Investigate EF6 initializers, automatic migrations, duplicate connection-string configuration, and public EF types before changes. Validate Data before Business and confirm the original Framework Web and Tests consumers remain buildable after each tier.

**Done when**: Data and Business build for their Framework and .NET 10 targets, EF6 behavior is validated against a disposable SQL Server database, package/API issues are resolved inline, focused tests pass, and existing Framework consumers remain buildable.

### 05-scaffold-web-host: Scaffold the side-by-side ASP.NET Core host

Create a new SDK-style ASP.NET Core MVC project targeting .NET 10 alongside `LegacyShop.Web`, reference the modern targets of Business and Data, and establish YARP routing so unmigrated traffic continues to reach the live Framework host. Configure native ASP.NET Core dependency injection and configuration without System.Web Adapters, preserve anonymous access, and connect both hosts to the existing SQL Server database without allowing the new host to take independent schema ownership. `#skill:migrating-webapi-odata` `#skill:managing-shared-database-schema`

The Framework host contains the active `LegacyShopContext` connection string and remains deployable during migration, so this task creates a second application over one live schema. Investigate connection-string ownership, EF6 startup initialization, route fallback behavior, and health/startup diagnostics before enabling traffic.

**Done when**: The new .NET 10 host builds and starts, returns a stub HTTP 200 response, proxies all unmigrated routes to the still-deployable Framework host, preserves anonymous access, and performs no automatic or conflicting database schema changes.

### 06-migrate-web-application: Migrate Web behavior and complete project references

Move the `LegacyShop.Web` application incrementally into the ASP.NET Core host using native ASP.NET Core APIs. The execution breakdown covers startup and middleware, conventional and area routing, filters, controllers, result semantics, model binding, anti-forgery, Razor views, static assets, configuration, and error handling; it must resolve all package and API incompatibilities inline and preserve current URLs, CRUD behavior, validation, and anonymous access. `LegacyShop.Data` and `LegacyShop.Business` remain in scope as dependencies and must be handled in dependency order. `#skill:migrating-webapi-odata` `#skill:managing-shared-database-schema`

The assessment identifies 365 binary-incompatible and two source-incompatible APIs in Web, plus unsupported MVC 5, Razor, WebPages, bundling, and System.Web startup packages. Research route precedence, Admin area discovery, controller construction, view helpers, static asset delivery, Newtonsoft.Json behavior, EF6 context lifetimes, and production migration policy. Final subtasks re-point `LegacyShop.Tests` to the new host, migrate it to .NET 10 with current MSTest/Moq dependencies, remove temporary library multi-targeting when no Framework consumer needs it, and clean obsolete references; the old Framework web project is not deleted.

**Done when**: All intended routes and assets are served by the .NET 10 host with behavior parity, database effects remain compatible with the live Framework host, migrated tests pass, references target the modern projects, temporary multi-targeting is removed when safe, and the old host remains deployable for rollback.

### 07-solution-validation: Validate the completed .NET 10 solution

Restore and build the complete modern project set, run all unit and integration tests, apply EF6 migrations only through the designated schema owner to a disposable SQL Server database, start the ASP.NET Core application, and exercise the primary routes, area routes, forms, validation, anti-forgery, CRUD, configuration, and error handling. Confirm no incompatible package, unresolved API, binding redirect, legacy System.Web runtime dependency, or unintended authentication requirement remains in the modern host.

Document production-readiness evidence and deferred follow-up work. The old Framework web project is removed only after the user confirms production readiness; EF Core, nullable reference types, and Central Package Management remain post-migration recommendations, with CPM now suitable because the modern projects are SDK-style and on a single TFM without `VersionOverride` friction.

**Done when**: The .NET 10 application restores, builds, starts, and passes its full test suite and smoke flows against SQL Server, all migration findings are resolved or explicitly documented, and rollback and deferred-cleanup guidance is recorded.

### 08-validate-test-baseline: Replay the behavior-locking baseline

Re-run the recorded generated baseline against the completed .NET 10 implementation, comparing the protected Data and Web behaviors with their pre-upgrade results. Investigate any difference in routes, result types, validation, anti-forgery, rendered behavior, database effects, migration/seed behavior, or anonymous access as an upgrade regression rather than silently accepting it.

This is the final planning checkpoint because it verifies the behavior preserved across every preceding task, including the side-by-side cutover and test-project migration.

**Done when**: Every recorded baseline test passes on the .NET 10 implementation, any intentional difference is explicitly approved and documented, and no unexplained behavioral regression remains.
