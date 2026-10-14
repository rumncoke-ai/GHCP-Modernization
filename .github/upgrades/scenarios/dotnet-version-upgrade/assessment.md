# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

Detailed findings live alongside this file in `assessment/`. This page is the index: read it first, then open only the documents you need.

## Table of Contents

- [Executive Summary](#executive-summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
  - [Binding Redirect Configuration](#binding-redirect-configuration)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Detailed Reports](#detailed-reports)
  - [Projects Relationship Graph](assessment/project-graph.md)
  - [Aggregate NuGet packages details](assessment/nuget/aggregate-packages.md)
  - [Most Frequent API Issues (complete list)](assessment/api-issues/most-frequent-api-issues.md)
  - [Project Details](#project-details)

## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 4 | All require upgrade |
| Total NuGet Packages | 15 | 4 need upgrade |
| Total Code Files | 79 |  |
| Total Code Files with Incidents | 22 |  |
| Total Lines of Code | 2828 |  |
| Total Number of Issues | 412 |  |
| Proposed Target Framework | net10.0 |  |
| Estimated LOC to modify | 374+ | at least 13.2% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Test Coverage | Package Issues | API Issues | Binding Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| [LegacyShop.Business\LegacyShop.Business.csproj](assessment/projects/LegacyShop.Business.md) | net47 | 🟢 Low | — | 1 | 0 | 1 |  | ClassicClassLibrary, Sdk Style = False |
| [LegacyShop.Data\LegacyShop.Data.csproj](assessment/projects/LegacyShop.Data.md) | net47 | 🟢 Low | 🧪 Recommended | 1 | 6 | 1 | 6+ | ClassicClassLibrary, Sdk Style = False |
| [LegacyShop.Tests\LegacyShop.Tests.csproj](assessment/projects/LegacyShop.Tests.md) | net47 | 🟢 Low | — | 5 | 1 | 3 | 1+ | ClassicClassLibrary, Sdk Style = False |
| [LegacyShop.Web\LegacyShop.Web.csproj](assessment/projects/LegacyShop.Web.md) | net47 | 🔴 High | 🧪 Recommended | 8 | 367 | 1 | 367+ | Wap, Sdk Style = False |

🧪 **Test Coverage** — projects risky enough to add behavior-locking tests before upgrading, to catch regressions the upgrade may introduce. Requires the **dotnet-test** plugin.

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 11 | 73.3% |
| ⚠️ Incompatible | 1 | 6.7% |
| 🔄 Upgrade Recommended | 3 | 20.0% |
| ***Total NuGet Packages*** | ***15*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 366 | High - Require code changes |
| 🟡 Source Incompatible | 8 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1931 |  |
| ***Total APIs Analyzed*** | ***2305*** |  |

### Binding Redirect Configuration

| Severity | Count | Description |
| :--- | :---: | :--- |
| 🟡Potential | 6 | May cause issues in certain scenarios |
| ***Total Binding Issues*** | ***6*** | ***Across 4 project(s)*** |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |
| ASP.NET Framework (System.Web) | 368 | 98.4% | Legacy ASP.NET Framework APIs for web applications (System.Web.*) that don't exist in ASP.NET Core due to architectural differences. ASP.NET Core represents a complete redesign of the web framework. Migrate to ASP.NET Core equivalents or consider System.Web.Adapters package for compatibility. |
| Legacy Configuration System | 6 | 1.6% | Legacy XML-based configuration system (app.config/web.config) that has been replaced by a more flexible configuration model in .NET Core. The old system was rigid and XML-based. Migrate to Microsoft.Extensions.Configuration with JSON/environment variables; use System.Configuration.ConfigurationManager NuGet package as interim bridge if needed. |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |
| T:System.Web.Mvc.ActionResult | 36 | 9.6% | Binary Incompatible |
| T:System.Web.Mvc.ViewResult | 34 | 9.1% | Binary Incompatible |
| M:System.Web.Mvc.Controller.View(System.Object) | 31 | 8.3% | Binary Incompatible |
| P:System.Web.Mvc.Controller.ModelState | 20 | 5.3% | Binary Incompatible |
| T:System.Web.Mvc.ModelStateDictionary | 20 | 5.3% | Binary Incompatible |
| M:System.Web.Mvc.Controller.HttpNotFound | 18 | 4.8% | Binary Incompatible |
| T:System.Web.Mvc.HttpNotFoundResult | 18 | 4.8% | Binary Incompatible |
| M:System.Web.Mvc.ModelStateDictionary.AddModelError(System.String,System.String) | 13 | 3.5% | Binary Incompatible |
| M:System.Web.Mvc.Controller.#ctor | 12 | 3.2% | Binary Incompatible |
| M:System.Web.Mvc.HttpPostAttribute.#ctor | 12 | 3.2% | Binary Incompatible |

The table above is the top 10. See [the complete list](assessment/api-issues/most-frequent-api-issues.md) for every affected API.

## Detailed Reports

- [Projects Relationship Graph](assessment/project-graph.md)
- [Aggregate NuGet packages details](assessment/nuget/aggregate-packages.md)
- [Most Frequent API Issues (complete list)](assessment/api-issues/most-frequent-api-issues.md)

### Project Details

- [LegacyShop.Business\LegacyShop.Business.csproj](assessment/projects/LegacyShop.Business.md)
- [LegacyShop.Data\LegacyShop.Data.csproj](assessment/projects/LegacyShop.Data.md)
- [LegacyShop.Tests\LegacyShop.Tests.csproj](assessment/projects/LegacyShop.Tests.md)
- [LegacyShop.Web\LegacyShop.Web.csproj](assessment/projects/LegacyShop.Web.md)


