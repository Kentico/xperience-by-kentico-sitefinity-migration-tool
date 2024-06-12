using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

namespace Migration.Tookit.Sitefinity.Core.Services;
internal interface IDataClassImportService : IDataImportService<DataClassModel>
{
    ImportStateObserver StartImport(ImportStateObserver observer);
};
