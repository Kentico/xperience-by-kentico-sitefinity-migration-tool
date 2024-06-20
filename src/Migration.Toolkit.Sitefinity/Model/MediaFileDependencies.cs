using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Model
{
    internal class MediaFileDependencies : IImportDependencies
    {
        public required IEnumerable<MediaLibraryModel> MediaLibraries { get; set; }
    }
}
