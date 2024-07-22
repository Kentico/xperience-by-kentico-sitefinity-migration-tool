using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Service for importing Content items from Sitefinity to XbyK site
/// </summary>
internal interface IContentItemImportService : IDataImportServiceWithDependencies<ContentDependencies, ContentItemSimplifiedModel>
{
}
