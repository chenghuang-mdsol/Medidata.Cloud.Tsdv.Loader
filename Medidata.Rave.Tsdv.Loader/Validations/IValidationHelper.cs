using System.Collections.Generic;

namespace Medidata.Rave.Tsdv.Loader.Validations
{
    public interface IValidationHelper
    {
        bool ExistsFolderOid(string folderOid, IDictionary<string, object> context);
        bool ExistsFormOid(string formOid, IDictionary<string, object> context);
        bool ExistsFormField(string formOid, string fieldOid, IDictionary<string, object> context);
    }
}