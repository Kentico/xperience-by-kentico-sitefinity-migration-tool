using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Core.Providers;
public interface IContentProvider
{
    public IEnumerable<Page> GetPages();
    public IEnumerable<ContentItem> GetContentItems(IEnumerable<SitefinityTypeDefinition> typeDefinitions, IEnumerable<string?> cultures);
}
