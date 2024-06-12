using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

namespace Migration.Toolkit.Sitefinity.Core.Services;
internal interface IDataClassImportService : IDataImportService<DataClassModel>
{
    ImportStateObserver StartImport(ImportStateObserver observer);
};
