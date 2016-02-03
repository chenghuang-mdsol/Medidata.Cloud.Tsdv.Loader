using System;

namespace Medidata.Rave.Tsdv.Loader
{
    [Flags]
    public enum TsdvLoaderSupportedVersion
    {
        OldFormat = 0,
        V1 = 1
    }
}