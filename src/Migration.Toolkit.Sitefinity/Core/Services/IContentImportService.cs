﻿using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services
{
    internal interface IContentImportService : IDataImportServiceWithDependencies<ContentDependencies, ContentItemSimplifiedModel>
    {
    }
}