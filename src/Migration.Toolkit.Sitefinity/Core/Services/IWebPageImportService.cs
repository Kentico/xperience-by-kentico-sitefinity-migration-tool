using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Service for importing web pages from Sitefinity to XbyK site
/// </summary>
internal interface IWebPageImportService : IDataImportServiceWithDependencies<ContentDependencies, ContentItemSimplifiedModel>
{
}
