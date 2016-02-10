using System;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions
{
    [Flags]
    public enum TsdvLoaderSupportedVersion
    {
        Presentation = 0,
        V1 = 1
    }
}