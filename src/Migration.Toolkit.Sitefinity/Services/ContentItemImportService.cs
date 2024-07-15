using CMS.Helpers;

using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services
{
    internal class ContentItemImportService(IImportService kenticoImportService,
                                            IDataClassImportService dataClassImportService,
                                            IMediaImportService mediaImportService,
                                            IUserImportService userImportService,
                                            IContentProvider contentProvider,
                                            IUmtAdapterWithDependencies<ContentItem, ContentDependencies, ContentItemSimplifiedModel> adapter) : IContentItemImportService
    {
        private readonly string[] relatedColumnTypes = ["contentitemreference", "assets"];

        public IEnumerable<ContentItemSimplifiedModel> Get(ContentDependencies dependenciesModel)
        {
            var typeDefinitions = new List<SitefinityTypeDefinition>();

            foreach (var dataClass in dependenciesModel.DataClasses.Select(x => x.Value))
            {
                string? typeName = dataClass.CustomProperties["SitefinityTypeName"]?.ToString();

                if (typeName == null)
                {
                    continue;
                }

                var dataClassGuid = ValidationHelper.GetGuid(dataClass.ClassGUID, Guid.Empty);

                if (dataClassGuid.Equals(Guid.Empty))
                {
                    continue;
                }

                var relatedFields = new List<string>();

                foreach (var field in dataClass.Fields)
                {
                    if (!relatedColumnTypes.Any(z => z.Equals(field.ColumnType)) || field.Column == null)
                    {
                        continue;
                    }

                    relatedFields.Add(field.Column);
                }

                typeDefinitions.Add(new SitefinityTypeDefinition
                {
                    SitefinityTypeName = typeName,
                    DataClassGuid = dataClassGuid,
                    RelatedFields = relatedFields
                });
            }

            var contentItems = contentProvider.GetContentItems(typeDefinitions);

            return adapter.Adapt(contentItems, dependenciesModel);
        }
        public SitefinityImportResult<ContentItemSimplifiedModel> StartImport(ImportStateObserver observer)
        {
            var mediaFiles = mediaImportService.StartImport(observer);

            var users = userImportService.StartImport(mediaFiles.Observer);

            var dataClasses = dataClassImportService.StartImport(users.Observer);

            var dependencies = new ContentDependencies
            {
                MediaFiles = mediaFiles.ImportedModels,
                Users = users.ImportedModels,
                DataClasses = dataClasses.ImportedModels
            };

            var contentItems = Get(dependencies);

            return new SitefinityImportResult<ContentItemSimplifiedModel>
            {
                ImportedModels = contentItems.ToDictionary(x => x.ContentItemGUID),
                Observer = observer// kenticoImportService.StartImport(pages, dataClasses.Observer)
            };
        }
        public SitefinityImportResult<ContentItemSimplifiedModel> StartImportWithDependencies(ImportStateObserver observer, ContentDependencies dependenciesModel)
        {
            var contentItems = Get(dependenciesModel);

            return new SitefinityImportResult<ContentItemSimplifiedModel>
            {
                ImportedModels = contentItems.ToDictionary(x => x.ContentItemGUID),
                Observer = observer// kenticoImportService.StartImport(pages, dataClasses.Observer)
            };
        }
    }
}
