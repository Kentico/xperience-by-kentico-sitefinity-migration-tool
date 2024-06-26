using Migration.Tookit.Data.Models;

namespace Migration.Tookit.Data.Core.Providers;
public interface IContentProvider
{
    public IEnumerable<Page> GetPages();
    public IEnumerable<ContentItem> GetContentItems(IEnumerable<SitefinityTypeDefinition> typeDefinitions);
}
