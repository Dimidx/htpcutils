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
        public string Description { get { return "Récupère les informations avec le scraper de Passion-xbmc"; } }

        private MMPluginOptionCollection _Options;
        public MMPluginOptionCollection Options
        {
            get { return _Options; }
            set { _Options = value; }
        }
        public List<MMPluginOption> LoadOptions()
        {
            List<MMPluginOption> _OptionsDispo = new List<MMPluginOption>();
            return _OptionsDispo;
        }

        public Film GetMovie(Film _Film)
        {
            return GetMovie(_Film, false);

        }

        /// <summary>
        /// Récupère les infos du film en spécifiant si on les charge depuis le cache ou pas
        /// et si on récupère les images ou pas. Si le film n'est pas dans le cache on récupère
        /// depuis le site.
        /// </summary>
        /// <param name="IDFilm">ID du film</param>
        /// <returns>Infos du Film</returns>
        public Film GetMovie(Film _Film, bool _ModeIMDB)
        {

            Film MonFilm = new Film();
            string _source = String.Empty;
            if (!_ModeIMDB)
            {
                _source = Utils.GetSourceHTML(@"http://passion-xbmc.org/scraper/index.php?id=" + _Film.AlloID);
            }
            else
            {
                _source = Utils.GetSourceHTML(@"http://passion-xbmc.org/scraper/index.php?idimdb=" + _Film.ID.Replace("tt", ""));
            }

            _source = Regex.Replace(_source, "[\r\n]", "");

            if (Regex.Match(_source, "<title>(.*)</title>").Success)
                MonFilm.Titre = Regex.Match(_source, "<title>(.*)</title>").Groups[1].ToString();
            if (Regex.Match(_source, "<year>(.*)</year>").Success)
                MonFilm.Annee = Regex.Match(_source, "<year>(.*)</year>").Groups[1].ToString();
            if (Regex.Match(_source, "<director>(.*)</director>").Success)
                MonFilm.Realisateurs.Add(new Personne(Regex.Match(_source, "<director>(.*)</director>").Groups[1].ToString(), "Réalisateur"));
            if (Regex.Match(_source, "<runtime>(.*)</runtime>").Success)
                MonFilm.Duree = Regex.Match(_source, "<runtime>(.*)</runtime>").Groups[1].ToString();
            if (Regex.Match(_source, "<studio>(.*)</studio>").Success)
                MonFilm.Studio = Regex.Match(_source, "<studio>(.*)</studio>").Groups[1].ToString().Replace("France", "").Trim();
            if (Regex.Match(_source, "<outline>(.*)</outline>").Success)
                MonFilm.Resume = Regex.Match(_source, "<outline>(.*)</outline>").Groups[1].ToString();
            if (Regex.Match(_source, "<plot>(.*)</plot>").Success)
                MonFilm.Synopsis = Regex.Match(_source, "<plot>(.*)</plot>").Groups[1].ToString();
            if (Regex.Match(_source, "<mpaa>(.*)</mpaa>").Success)
                MonFilm.MPAA = Regex.Match(_source, "<mpaa>(.*)</mpaa>").Groups[1].ToString();
            if (Regex.Match(_source, "<trailer>(.*)</trailer>").Success)
                MonFilm.Trailer = Regex.Match(_source, "<trailer>(.*)</trailer>").Groups[1].ToString();
            if (Regex.Match(_source, "<originaltitle>(.*)</originaltitle>").Success)
                MonFilm.TitreOriginal = Regex.Match(_source, "<originaltitle>(.*)</originaltitle>").Groups[1].ToString();
            if (Regex.Match(_source, "<id>(.*)</id>").Success)
                MonFilm.ID = Regex.Match(_source, "<id>(.*)</id>").Groups[1].ToString();


            MonFilm.AlloID = _Film.AlloID;

            #region Accroche tagline
            if (Regex.Match(_source, "<tagline>(.*)</tagline>").Success)
            {
                MonFilm.Accroche = Regex.Match(_source, "<tagline>(.*)</tagline>").Groups[1].ToString();
                MonFilm.Accroche.ToLower().Contains("voir la critique");
                MonFilm.Accroche = string.Empty;
            }
            #endregion

            #region Certification
            if (Regex.Match(_source, "<certification>(.*)</certification>").Success)
            {

                MonFilm.Certification = Regex.Match(_source, "<certification>(.*)</certification>").Groups[1].ToString();
            }
            else
            {

                if (Regex.Match(_source, "Film pour enfants à partir de 3 ans").Success)
                {
                    MonFilm.Certification = "France:-3";
                }
                if (Regex.Match(_source, "Film pour enfants à partir de 6 ans").Success)
                {
                    MonFilm.Certification = "France:-6";
                }
                if (Regex.Match(_source, "Film pour enfants à partir de 10 ans").Success)
                {
                    MonFilm.Certification = "France:-10";
                }
                if (Regex.Match(_source, "Interdit aux moins de 12 ans").Success)
                {
                    MonFilm.Certification = "France:-12";
                }
                if (Regex.Match(_source, "Interdit aux moins de 16 ans").Success)
                {
                    MonFilm.Certification = "France:-16";
                }
                if (Regex.Match(_source, "Des images ou des idées peuvent choquer").Success)
                {
                    MonFilm.Certification = "France:-12";
                }
                if (Regex.Match(_source, "Interdit aux moins de 18 ans").Success)
                {
                    MonFilm.Certification = "France:-18";
                }



            }
            #endregion

            #region Notes
            if (Regex.Match(_source, "<rating>(.*)</rating>").Success)
            {
                try
                {
                    MonFilm.Note = (float)(System.Double.Parse(Regex.Match(_source, "<rating>(.*)</rating>").Groups[1].ToString()) * 2.5);
                }
                catch { }
            }
            #endregion

            #region Votes
            if (Regex.Match(_source, "<votes>(.*)</votes>").Success)
            {
                try
                {
                    MonFilm.Votes = (float)System.Double.Parse(Regex.Match(_source, "<votes>(.*)</votes>").Groups[1].ToString());
                }
                catch { }
            }
            #endregion

            #region Top250
            if (Regex.Match(_source, "<top250>(.*)</top250>").Success)
            {
                try
                {
                    MonFilm.Top250 = (int)System.Double.Parse(Regex.Match(_source, "<top250>(.*)</top250>").Groups[1].ToString());
                }
                catch { }
            }
            #endregion

            #region Genres
            if (Regex.Match(_source, "<genre>(.*)</genre>").Success)
            {
                MonFilm.Genres = Regex.Match(_source, "<genre>(.*)</genre>").Groups[1].ToString().Split('/');
                //Suppression des espaces
                for (int i = 0; i < MonFilm.Genres.Length; i++)
                {
                    MonFilm.Genres[i] = MonFilm.Genres[i].Trim();
                }
            }

            #endregion

            #region Acteurs

            string _Acteurs = Regex.Match(_source, @"<thumbs>(.*?)</thumbs>").Groups[1].ToString();
            MatchCollection _MatchesActeurs = Regex.Matches(_source, @"<actor>(.*?)</actor>");

            foreach (Match _Match in _MatchesActeurs)
            {
                string strMovieCode = _Match.ToString();
                Personne _act = new Personne();

                if (Regex.Match(strMovieCode, "<name>(.*?)</name>").Success)
                    _act.Nom = Regex.Match(strMovieCode, "<name>(.*?)</name>").Groups[1].ToString();
                if (Regex.Match(strMovieCode, "<role>(.*?)</role>").Success)
                    _act.Role = Regex.Match(strMovieCode, "<role>(.*?)</role>").Groups[1].ToString();
                if (Regex.Match(strMovieCode, "<thumb>(.*?)</thumb>").Success)
                    _act.Photo = new Thumb(Regex.Match(strMovieCode, "<thumb>(.*?)</thumb>").Groups[1].ToString());
                if (!String.IsNullOrEmpty(_act.Nom)) MonFilm.Acteurs.Add(_act);
            }

            #endregion

            #region Affiches

            string _Affiches = Regex.Match(_source, @"<thumbs>(.*?)</thumbs>").Groups[1].ToString();
            MatchCollection _MatchesAffiches = Regex.Matches(_Affiches, @"<thumb>(.*?)</thumb>");

            foreach (Match _Match in _MatchesAffiches)
            {
                string strMovieCode = _Match.ToString();
                if (Regex.Match(strMovieCode, "<thumb>(.*?)</thumb>").Success)
                    MonFilm.ListeCover.Add(new Thumb(_Match.Groups[1].ToString()));

            }

            #endregion

            #region Fanart

            string _Fanarts = Regex.Match(_source, @"<fanart>(.*?)</fanart>").Groups[1].ToString();
            MatchCollection _MatchesFanart = Regex.Matches(_Fanarts, @"<thumb>(.*?)</thumb>");
            foreach (Match _Match in _MatchesFanart)
            {

                string strMovieCode = _Match.ToString();
                if (Regex.Match(strMovieCode, "<thumb>(.*?)</thumb>").Success)
                    MonFilm.ListeFanart.Add(new Thumb(_Match.Groups[1].ToString()));

            }

            #endregion

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

            //Si l'id Allociné est renseigné on lance direct le get
            if (!String.IsNullOrEmpty(_Film.AlloID))
            {
                Film _temp = new Film();
                _temp = GetMovie(_Film);
                if (!String.IsNullOrEmpty(_temp.Titre))
                {
                    ListeFilm.Add(GetMovie(_Film));
                    return ListeFilm;
                }
            }

            //Si l'id IMDB est renseigné on lance direct le get en mode imdb
            if (!String.IsNullOrEmpty(_Film.ID))
            {
                Film _temp = new Film();
                _temp = GetMovie(_Film, true);
                if (!String.IsNullOrEmpty(_temp.Titre))
                {
                    ListeFilm.Add(GetMovie(_Film));
                    return ListeFilm;
                }
            }

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
