using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaManager.Library
{
    public interface MovieScraper
    {
        /// <summary>
        /// Nom du scraper
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// URL du site
        /// </summary>
        string URL { get; }

        /// <summary>
        /// Auteur du scraper
        /// </summary>
        string Author { get; }

        List<Film> SearchMovie(Film _film);

        Film GetMovie(Film _film);
    }
}
