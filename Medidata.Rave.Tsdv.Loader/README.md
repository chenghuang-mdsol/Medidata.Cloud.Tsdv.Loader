# Medidata.Rave.Tsdv.Loader
`Medidata.Rave.Tsdv.Loader` is a NuGet package built on base of `Medidata.Cloud.ExcelLoader` for Rave TSDV report downloading and uploading.

# NuGet package
http://nuget.imedidata.net/feed/mdsol/package/nuget/Medidata.Rave.Tsdv.Loader
```powershell
PM> Install-Package Medidata.Rave.Tsdv.Loader
```

# Usage examples of `TsdvExcelLoaderFactory`
Use `Create()` method of `TsdvExcelLoaderFactory` (can be singleton) to create a TSDV excel loader. This loader has a cover sheet and below predefined sheets in shown order.

| Sheet name | sheet model class |
| --- | --- |
| BlockPlans | `BlockPlan` |
| BlockPlanSettings | `BlockPlanSetting` |
| CustomTiers | `CustomTier` |
| TierFields | `TierField` |
| TierForms | `TierForm` |
| TierFolders | `TierFolder` |
| ExcludedStatuses | `ExcludedStatus` |
| Rules | `Rule` |

Use dependency injector to get an instance of `ITsdvExcelLoaderFactory` or `TsdvExcelLoaderFactory`.
```cs
var loader = DiContainer.Resolve<TsdvExcelLoaderFactory>().Create();

// Or resolve ITsdvExcelLoaderFactory, by which you can control the lifetime to be singleton.
var loader = DiContainer.Resolve<ITsdvExcelLoaderFactory>().Create();
```

# `unity.config` example
```xml
<?xml version="1.0" encoding="utf-8"?>
<unity xmlns="http://schemas.microsoft.com/practices/2010/unity">
  <container>
    <register type="Medidata.Interfaces.Localization.ILocalization, Medidata.Interfaces"
              mapTo="Medidata.Rave.Tsdv.Loader.Sample.Localization, Medidata.Rave.Tsdv.Loader.Sample" />
    <register type="Medidata.Rave.Tsdv.Loader.ITsdvExcelLoaderFactory, Medidata.Rave.Tsdv.Loader"
              mapTo="Medidata.Rave.Tsdv.Loader.TsdvExcelLoaderFactory, Medidata.Rave.Tsdv.Loader" >
      <lifetime type="singleton" />
    </register>
  </container>
</unity>
```
