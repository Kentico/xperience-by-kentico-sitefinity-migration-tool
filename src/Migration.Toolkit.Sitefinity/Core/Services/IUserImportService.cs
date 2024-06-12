﻿using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

namespace Migration.Tookit.Sitefinity.Core.Services;
internal interface IUserImportService : IDataImportService<UserInfoModel>
{
    ImportStateObserver StartImport(ImportStateObserver observer);
    ImportStateObserver StartImport(ImportStateObserver observer, out IEnumerable<UserInfoModel> users);
}
