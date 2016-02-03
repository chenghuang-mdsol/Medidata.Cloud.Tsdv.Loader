using System;

namespace Medidata.Rave.Tsdv.Loader
{
    [Flags]
    public enum TsdvLoaderSupportedVersion
    {
        PresentationFormat = 0,
        V1 = 1
    }
}