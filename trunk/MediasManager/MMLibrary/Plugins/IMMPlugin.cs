using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaManager.Plugins
{
    public interface IMMPlugin
    {
        /// <summary>
        /// Nom du Plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Auteur
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Version
        /// </summary>
        string Version { get; }

        List<MMPluginOption> GetOptions();






    }
}
