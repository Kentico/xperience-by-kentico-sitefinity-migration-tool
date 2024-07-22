using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Core.Providers;
/// <summary>
/// Provider for getting content items and pages from Sitefinity using Rest Sdk.
/// </summary>
public interface IContentProvider
{
    /// <summary>
    /// Gets Pages from Sitefinity.
    /// </summary>
    /// <param name="cultures">Cultures to query.</param>
    /// <returns>Pages from Sitefinity.</returns>
    public IEnumerable<Page> GetPages(IEnumerable<SystemCulture> cultures);

    /// <summary>
    /// Gets static and dynamic module Content Items from Sitefinity.
    /// </summary>
    /// <param name="typeDefinitions">All type definitions to query.</param>
    /// <param name="cultures">Cultures to query.</param>
    /// <returns>Static and dynamic module Content Items from Sitefinity.</returns>
    public IEnumerable<ContentItem> GetContentItems(IEnumerable<SitefinityTypeDefinition> typeDefinitions, IEnumerable<SystemCulture> cultures);
}
