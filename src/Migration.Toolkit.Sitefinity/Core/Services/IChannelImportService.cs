﻿using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Service for importing channels/sites from Sitefinity to XbyK site
/// </summary>
internal interface IChannelImportService : IDataImportServiceWithDependencies<ChannelDependencies>
{
}
