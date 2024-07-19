using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Service for importing content types from Sitefinity to XbyK site.
/// </summary>
internal interface IDataClassImportService : IDataImportServiceWithDependencies<DataClassDependencies>
{
};
