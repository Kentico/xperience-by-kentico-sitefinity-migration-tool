﻿using Migration.Tookit.Data.Models;

namespace Migration.Tookit.Data.Core.Providers;
public interface ITypeProvider
{
    IEnumerable<SitefinityType> GetDynamicModuleTypes();
    IEnumerable<SitefinityType> GetSitefinityTypes();
}
