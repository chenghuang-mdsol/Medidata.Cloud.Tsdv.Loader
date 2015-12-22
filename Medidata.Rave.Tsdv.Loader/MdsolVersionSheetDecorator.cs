using System.Reflection;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Rave.Tsdv.Loader
{
    public class MdsolVersionSheetDecorator : ISheetBuilderDecorator
    {
        private static readonly string AssemblyVersion;

        static MdsolVersionSheetDecorator()
        {
            AssemblyVersion = Assembly.GetCallingAssembly().GetName().Version.ToString();
        }

        public ISheetBuilder Decorate(ISheetBuilder target)
        {
            var originalFunc = target.BuildSheet;
            target.BuildSheet = (objects, sheetDefinition, doc) =>
            {
                originalFunc(objects, sheetDefinition, doc);

                var worksheet = doc.GetWorksheetByName(sheetDefinition.Name);
                worksheet.AddMdsolAttribute("version", AssemblyVersion);
            };

            return target;
        }
    }
}