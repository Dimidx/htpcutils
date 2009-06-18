using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MediaManager.Library;

namespace MediaManager.Plugins
{
    public interface IMMPluginImportExport :IMMPlugin
    {
        Film Import(FileInfo _FileInfo);

        bool Export(Film _Film, FileInfo _FileInfo);
    }
}
