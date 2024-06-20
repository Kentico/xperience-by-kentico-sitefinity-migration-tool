using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

namespace Migration.Toolkit.Sitefinity.Model
{
    public class SitefinityImportResult<TUmtModel> where TUmtModel : class, IUmtModel
    {
        public required ImportStateObserver Observer { get; set; }
        public required IDictionary<Guid, TUmtModel> ImportedModels { get; set; }
    }
}
