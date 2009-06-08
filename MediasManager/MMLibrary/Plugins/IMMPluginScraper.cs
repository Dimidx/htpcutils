using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaManager.Library;

namespace MediaManager.Plugins
{
    public interface IMMPluginScraper :IMMPlugin
    {

        /// <summary>
        /// URL du site
        /// </summary>
        string URL { get; }

        List<Film> SearchMovie(Film _film);

        Film GetMovie(Film _film);

    }
}
