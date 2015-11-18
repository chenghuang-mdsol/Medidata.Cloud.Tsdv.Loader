using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Medidata.Interfaces.Localization;

namespace Medidata.Cloud.Tsdv.Loader.Localization
{
    public class EchoLocalizer: ILocalization
    {
        public string GetLocalString(string key, string locale = null)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"tsdv_BlockPlanName", "Name"},
                {"tsdv_BlockPlanType", "Block Plan Type"}
            };
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return "["+key+"]";
        }

        public string GetLocalDataString(int stringId, string locale)
        {
            return "["+stringId+"]";
        }

        public IEnumerable<ILocale> Locales { get; private set; }
    }

    

    
}
