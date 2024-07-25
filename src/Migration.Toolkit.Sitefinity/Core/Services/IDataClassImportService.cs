using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Service for importing content types from Sitefinity to XbyK site
/// </summary>
internal interface IDataClassImportService : IDataImportService<DataClassModel>
{
    ImportStateObserver StartImport(ImportStateObserver observer);
};
