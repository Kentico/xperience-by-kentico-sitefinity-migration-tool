﻿using Kentico.Xperience.UMT.Services;

namespace Migration.Tookit.Sitefinity.Core.Services;
public interface IDataImportService<out T> where T : class
{
    IEnumerable<T> Get();
    ImportStateObserver StartImport(ImportStateObserver observer);
}
