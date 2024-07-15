using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

namespace Migration.Toolkit.Sitefinity.Model
{
    /// <summary>
    /// Result of the import process. Contains imported models and observer.
    /// </summary>
    public class SitefinityImportResult
    {
        /// <summary>
        /// Observer used in Universal Migration Toolkit import service.
        /// </summary>
        public required ImportStateObserver Observer { get; set; }
        /// <summary>
        /// Dictionary of imported models.
        /// </summary>
        public required IDictionary<Guid, IUmtModel> ImportedModels { get; set; }
    }

    /// <summary>
    /// Result of the import process. Contains imported models and observer.
    /// </summary>
    /// <typeparam name="TUmtModel">IUmtModel that was imported.</typeparam>
    public class SitefinityImportResult<TUmtModel> where TUmtModel : class, IUmtModel
    {
        /// <summary>
        /// Observer used in Universal Migration Toolkit import service.
        /// </summary>
        public required ImportStateObserver Observer { get; set; }
        /// <summary>
        /// Dictionary of imported models.
        /// </summary>
        public required IDictionary<Guid, TUmtModel> ImportedModels { get; set; }
    }
}
