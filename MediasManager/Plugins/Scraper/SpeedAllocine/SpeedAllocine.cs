using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;
using MediaManager.Library;
using MediaManager.Plugins;

namespace MediaManager.Plugins
{
    public class SpeedAllocine : IMMPluginScraper
    {
        public string URL { get { return "http://passion-xbmc.org/scraper-alocine/scraper-speedallocine-v2/"; } }
        public string Name { get { return "SpeedAllocine"; } }
        public string Author { get { return "Danone-KiD"; } }
        public string Version { get { return "1.0"; } }
        public string Description { get { return "Récupère les informations avec le scraper de Passion-xbmc (Merci à l8tig)"; } }


        public List<MMPluginOption> GetOptions()
        {
            List<MMPluginOption> options = new List<MMPluginOption>();
            return options;
        }


        /// <summary>
        /// Récupère les infos du film en spécifiant si on les charge depuis le cache ou pas
        /// et si on récupère les images ou pas. Si le film n'est pas dans le cache on récupère
        /// depuis le site.
        /// </summary>
        /// <param name="IDFilm">ID du film</param>
        /// <returns>Infos du Film</returns>
        public Film GetMovie(Film _Film)
        {

            Film MonFilm = new Film();
            string _source = Utils.GetSourceHTML(@"http://passion-xbmc.org/scraper/index.php?id=" + _Film.AlloID);
            _source = Regex.Replace(_source, "[\r\n]", "");

            if (Regex.Match(_source, "<title>(.*)</title>").Success)        
                MonFilm.Titre = Regex.Match(_source, "<title>(.*)</title>").Groups[1].ToString();
            if (Regex.Match(_source, "<year>(.*)</year>").Success)
                MonFilm.Annee = Regex.Match(_source, "<year>(.*)</year>").Groups[1].ToString();


            //Affiches
            string _Affiches = Regex.Match(_source, @"<thumbs>(.*?)</thumbs>").Groups[1].ToString();
            MatchCollection _MatchesAffiches = Regex.Matches(_source, @"<thumb>(.*?)</thumb>");

            foreach (Match _Match in _MatchesAffiches)
            {
                
                string strMovieCode = _Match.ToString();
                if (Regex.Match(strMovieCode, "<thumb>(.*?)</thumb>").Success)
                    MonFilm.ListeCover.Add(new Thumb(_Match.Groups[1].ToString()));
                
            }

            //Fanart

            string _Fanarts = Regex.Match(_source, @"<fanart>(.*?)</fanart>").Groups[1].ToString();
            MatchCollection _MatchesFanart = Regex.Matches(_Fanarts, @"<thumb>(.*?)</thumb>");
            foreach (Match _Match in _MatchesFanart)
            {

                string strMovieCode = _Match.ToString();
                if (Regex.Match(strMovieCode, "<thumb>(.*?)</thumb>").Success)
                    MonFilm.ListeFanart.Add(new Thumb(_Match.Groups[1].ToString()));

            }

            return MonFilm;

        }
  
        /// <summary>
        /// Recherche un film sur Allociné
        /// </summary>
        /// <param name="TitreFilm">Titre du film à rechercher</param>
        /// <param name="LoadImage"><c>true</c> pour télécharger les miniatures, <c>false</c> dans le cas contraire</param>
        /// <returns>Film</returns>
        public List<Film> SearchMovie(Film _Film)
        {
            List<Film> ListeFilm = new List<Film>();
            string _source = Utils.GetSourceHTML(@"http://passion-xbmc.org/scraper/index.php?search=" + HttpUtility.UrlEncode(_Film.Titre, Encoding.Default));
            MatchCollection myMatches = Regex.Matches(_source, @"<title>(.*) \( (.*) \)</title><url>(.*)</url><id>(.*)</id>");
            foreach (Match movieCode in myMatches)
            {
                Film MonFilm = new Film();
                string strMovieCode = movieCode.ToString();

                MonFilm.Titre = movieCode.Groups[1].ToString();
                MonFilm.Annee = movieCode.Groups[2].ToString();
                MonFilm.AlloID = movieCode.Groups[4].ToString();
                ListeFilm.Add(MonFilm);
            }

           return ListeFilm;

        }

    }
}
