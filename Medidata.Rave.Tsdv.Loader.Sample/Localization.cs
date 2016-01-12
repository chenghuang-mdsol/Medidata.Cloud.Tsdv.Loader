using System.Collections.Generic;
using System.Linq;
using Medidata.Interfaces.Localization;

namespace Medidata.Rave.Tsdv.Loader.Sample
{
    public class Localization : ILocalization
    {
        public Localization()
        {
            Locales = Enumerable.Empty<ILocale>();
        }

        public string GetLocalString(string key, string locale = null)
        {
            return "[" + key + "]";
        }

        public string GetLocalDataString(int stringId, string locale)
        {
            return "[" + stringId + "]";
        }

        public IEnumerable<ILocale> Locales { get; set; }
    }
}