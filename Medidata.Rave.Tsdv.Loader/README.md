# Medidata.Rave.Tsdv.Loader
`Medidata.Rave.Tsdv.Loader` is a NuGet package built on base of `Medidata.Cloud.ExcelLoader` for Rave TSDV report downloading and uploading.

# NuGet package
http://nuget.imedidata.net/feed/mdsol/package/nuget/Medidata.Rave.Tsdv.Loader
```powershell
PM> Install-Package Medidata.Rave.Tsdv.Loader
```

# Usage examples of `TsdvExcelLoader`
The usage of `TsdvExcelLoader` is the same as the one of `ExcelLoader` (find [usage example of `ExcelLoader` here](../Medidata.Cloud.ExcelLoader/README.md)), but with with a cover sheet and below predefined sheets in shown order.

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

By predefining these sheets, it can guarantee that empty sheets can be generated even we don't add any sheet model object to the sheets.

# `TranslateHeaderDecorator`
`TranslateHeaderDecorator` is a sheet builder decorator that uses header names as string keys and translates with the injected `Medidata.Interfaces.ILocalization` service.

# `AutoCopyrightCoveredExcelBuilder`
`AutoCopyrightCoveredExcelBuilder` can replace the copyright declaration text on the cover sheet with the the assembly copyright of the entry assembly. For Rave, the value is defined by the `AssemblyCopyrightAttribute` of the assembly like `Medidata.MedidataRave.dll`, `Medidata.Core.Service.dll` and so forth.

# `unity.config` example
```xml
<?xml version="1.0" encoding="utf-8"?>
<unity xmlns="http://schemas.microsoft.com/practices/2010/unity">
  <container>
    <register type="Medidata.Interfaces.Localization.ILocalization, Medidata.Interfaces"
              mapTo="Medidata.Rave.Tsdv.Loader.Sample.Localization, Medidata.Rave.Tsdv.Loader.Sample" />
    <register type="Medidata.Cloud.ExcelLoader.ISheetBuilderDecorator, Medidata.Cloud.ExcelLoader"
              mapTo="Medidata.Cloud.ExcelLoader.SheetDecorators.TextStyleSheetDecorator, Medidata.Cloud.ExcelLoader"
              name="textStyleDecorator">
      <constructor>
        <param name="styleName" value="Normal" />
      </constructor>
    </register>
    <register type="Medidata.Cloud.ExcelLoader.ISheetBuilderDecorator, Medidata.Cloud.ExcelLoader"
              mapTo="Medidata.Cloud.ExcelLoader.SheetDecorators.HeaderStyleSheetDecorator, Medidata.Cloud.ExcelLoader"
              name="headerStyleDecorator">
      <constructor>
        <param name="styleName" value="Output" />
      </constructor>
    </register>
    <register type="Medidata.Cloud.ExcelLoader.ICellTypeValueConverterManager, Medidata.Cloud.ExcelLoader"
              mapTo="Medidata.Cloud.ExcelLoader.CellTypeValueConverterManager, Medidata.Cloud.ExcelLoader">
      <lifetime type="singleton" />
    </register>
    <register type="Medidata.Cloud.ExcelLoader.ISheetBuilder, Medidata.Cloud.ExcelLoader"
              mapTo="Medidata.Cloud.ExcelLoader.SheetBuilder, Medidata.Cloud.ExcelLoader">
      <constructor>
        <param name="converterManager" dependencyType="Medidata.Cloud.ExcelLoader.ICellTypeValueConverterManager, Medidata.Cloud.ExcelLoader" />
        <param name="decorators">
          <array>
            <dependency type="Medidata.Cloud.ExcelLoader.SheetDecorators.HeaderSheetDecorator, Medidata.Cloud.ExcelLoader" />
            <dependency type="Medidata.Cloud.ExcelLoader.SheetDecorators.AutoFilterSheetDecorator, Medidata.Cloud.ExcelLoader" />
            <dependency name="textStyleDecorator" />
            <dependency name="headerStyleDecorator" />
            <dependency type="Medidata.Rave.Tsdv.Loader.TranslateHeaderDecorator, Medidata.Rave.Tsdv.Loader" />
            <dependency type="Medidata.Rave.Tsdv.Loader.MdsolVersionSheetDecorator, Medidata.Rave.Tsdv.Loader" />
            <dependency type="Medidata.Cloud.ExcelLoader.SheetDecorators.AutoFitColumnSheetDecorator, Medidata.Cloud.ExcelLoader" />
          </array>
        </param>
      </constructor>
    </register>
    <register type="Medidata.Cloud.ExcelLoader.ISheetParser, Medidata.Cloud.ExcelLoader"
              mapTo="Medidata.Cloud.ExcelLoader.SheetParser, Medidata.Cloud.ExcelLoader" />
    <register type="Medidata.Cloud.ExcelLoader.IExcelBuilder, Medidata.Cloud.ExcelLoader"
              mapTo="Medidata.Rave.Tsdv.Loader.AutoCopyrightCoveredExcelBuilder, Medidata.Rave.Tsdv.Loader" />
    <register type="Medidata.Cloud.ExcelLoader.IExcelParser, Medidata.Cloud.ExcelLoader"
              mapTo="Medidata.Cloud.ExcelLoader.ExcelParser, Medidata.Cloud.ExcelLoader" />
    <register type="Medidata.Cloud.ExcelLoader.IExcelLoader, Medidata.Cloud.ExcelLoader"
              mapTo="Medidata.Rave.Tsdv.Loader.TsdvExcelLoader, Medidata.Rave.Tsdv.Loader" />
  </container>
</unity>
```
