using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing;
using System.Threading;
using System.Xml.Serialization;
using MediaManager.Library;
using MediaManager.Plugins;


namespace MediaManager.Plugins
{


    /// <summary>
    /// Permet d'accéder aux informations de http://www.allocine.fr
    /// </summary>
    public class Allocine : IMMPluginScraper
    {

        public string URL { get { return "http://www.allocine.fr"; } }
        public string Name { get { return "Allocine"; } }
        public string Author { get { return "Danone-KiD"; } }
        public string Version { get { return "1.0"; } }
        public string Description { get { return "Récupère les informations sur Allocine"; } }


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

            Film MonFilm = null;

            MonFilm = new Film();
            string s = Utils.GetSourceHTML("http://www.allocine.fr/film/fichefilm_gen_cfilm=" + _Film.AlloID+ ".html");

            s = Regex.Replace(s, "[\r\n]", "");

            MonFilm.AlloID = _Film.AlloID;
            MonFilm.Titre = Utils.removeUnwantedChars(Regex.Match(s, "<h1[^>]*>(.*?)</h1>").ToString());
            MonFilm.TitreOriginal = Utils.removeUnwantedChars(Regex.Match(s, @"<h3 class=""SpProse"">Titre original : <i>(.*?)</i>").Groups[1].ToString());
            MonFilm.Annee = Utils.removeUnwantedChars(Regex.Match(s, "Année de production : (\\d{4})").Groups[1].ToString());
            //MonFilm.Realisateurs = Utils.removeUnwantedChars(Regex.Match(s, @"<h3 class=""SpProse"">Réalisé par(.*?)</h3>").Groups[1].ToString()).Replace(", ", ",").Split(',');
            #region Réalisateurs
            string[] _Réalisateurs;
            _Réalisateurs = Utils.removeUnwantedChars(Regex.Match(s, @"<h3 class=""SpProse"">Réalisé par(.*?)</h3>").Groups[1].ToString()).Replace(", ", ",").Split(',');
            foreach (string item in _Réalisateurs)
            {
                Personne _real = new Personne();
                _real.Nom = item;
                MonFilm.Realisateurs.Add(_real);
            }
            #endregion

            #region Genres
            MonFilm.Genres = Utils.removeUnwantedChars(Regex.Match(s, "Genre :(.*?)</h3>").Groups[1].ToString()).Replace(", ", ",").Split(',');
            #endregion

            #region Pays
            MonFilm.Pays = Utils.removeUnwantedChars(Regex.Match(s, "<h3 class=\"SpProse\">Film ([^:]*).&nbsp;</h3>").Groups[1].ToString()).Replace(", ", ",").Split(',');
            #endregion

            #region Durée
            MonFilm.DureeChaine = Utils.removeUnwantedChars(Regex.Match(s, @"<h3 class=""SpProse"">Durée : (.*?)\.").Groups[1].ToString());
            try
            {
                int Heures = Convert.ToInt32(Regex.Match(MonFilm.DureeChaine, @"(\d*)h (\d*)min").Groups[1].ToString());
                int Minutes = Convert.ToInt32(Regex.Match(MonFilm.DureeChaine, @"(\d*)h (\d*)min").Groups[2].ToString());
                MonFilm.DureeMin = (Heures * 60) + Minutes;

            }
            catch { }

            #endregion

            #region Date de sortie

            string sDateSortie = "";
            sDateSortie = Utils.removeUnwantedChars(Regex.Match(s, "agenda_gen_date=(.*?).html").Groups[1].ToString());
            try
            {
                MonFilm.DateSortie = DateTime.Parse(sDateSortie);
            }
            catch { }

            #endregion

            #region Note Presse

            string sNotePresse = Utils.removeUnwantedChars(Regex.Match(s, "Presse.*?class=['\"]etoile_(\\d)['\"]").Groups[1].ToString());
            try
            {
                MonFilm.NotePresse = (float)(System.Double.Parse(sNotePresse) * 2.5);
            }
            catch { }

            #endregion

            #region Note Spectateurs

            string sNoteSpectateur = Utils.removeUnwantedChars(Regex.Match(s, "Spectateurs.*?class=['\"]etoile_(\\d)['\"]").Groups[1].ToString());
            try
            {
                MonFilm.NoteSpectateurs = (float)(System.Double.Parse(sNotePresse) * 2.5);
            }
            catch { }

            #endregion

            MonFilm.Synopsis = Utils.removeUnwantedChars(Regex.Match(s, "Synopsis(.*?)</h4></div>").Groups[1].ToString());
            MonFilm.Critique = Utils.removeUnwantedChars(Regex.Match(s, "<div align=\"justify\" style=\"padding: 5 0 5 0\"><h4>([^<]*)</h4>").Groups[1].ToString());

            #region Avis
            if (Regex.Match(s, "Film pour enfants à partir de 3 ans").Success)
            {
                MonFilm.Avis = "-3";
            }
            if (Regex.Match(s, "Film pour enfants à partir de 6 ans").Success)
            {
                MonFilm.Avis = "-6";
            }
            if (Regex.Match(s, "Film pour enfants à partir de 10 ans").Success)
            {
                MonFilm.Avis = "-10";
            }
            if (Regex.Match(s, "Interdit aux moins de 12 ans").Success)
            {
                MonFilm.Avis = "-12";
            }
            if (Regex.Match(s, "Interdit aux moins de 16 ans").Success)
            {
                MonFilm.Avis = "-16";
            }
            if (Regex.Match(s, "Des images ou des idées peuvent choquer").Success)
            {
                MonFilm.Avis = "-16";
            }
            if (Regex.Match(s, "Interdit aux moins de 18 ans").Success)
            {
                MonFilm.Avis = "-18";
            }

            #endregion

            #region Jaquette Allocine

            string strThumbURL = @"/film/galerievignette_gen_cfilm=" + _Film.AlloID+ ".html";
            if (strThumbURL != "")
            {
                strThumbURL = "http://www.allocine.fr" + strThumbURL;
                string strBodyThumb = Utils.GetSourceHTML(strThumbURL);
                if (strBodyThumb != null && strBodyThumb != "")
                {
                    strBodyThumb = Regex.Replace(strBodyThumb, "[\r\n]", "");
                    //L'affiche
                    strThumbURL = Utils.removeUnwantedChars(Regex.Match(strBodyThumb, "id=['\"]imgNormal['\"] class=['\"]photo['\"] src=['\"](.*?)['\"]").Groups[1].ToString());
                    Thumb _cover = new Thumb();
                    _cover.URLImage = strThumbURL;
                    MonFilm.ListeCover.Add(_cover);
                    
                    //Les autres images
                    Regex regImage = new Regex("\"fichier\":\"(.*?)\"");
                    Match matchImage = regImage.Match(strBodyThumb);
                    while (matchImage.Success)
                    {
                        Thumb _fanart = new Thumb();
                        _fanart.URLImage = "http://a69.g.akamai.net/n/69/10688/v1/img5.allocine.fr/acmedia/medias" + matchImage.Groups[1].ToString();
                        _fanart.URLMiniature = "http://a69.g.akamai.net/n/69/10688/v1/img5.allocine.fr/acmedia/crp/200/200/x/x/medias" + matchImage.Groups[1].ToString();

                        if (_fanart.URLImage != _cover.URLImage)
                        {
                            MonFilm.ListeFanart.Add(_fanart);
                            MonFilm.ListeCover.Add(_fanart);
                        }

                        matchImage = matchImage.NextMatch();
                    }
                }
            }

            #endregion

            #region Casting
            s = Utils.GetSourceHTML("http://www.allocine.fr/film/casting_gen_cfilm=" + _Film.AlloID + ".html");
            Regex regexObj = new Regex(@"<h5>([^&<]*)</h5></td>[^&<]*<[^&>]*><h5><a href=""[^&=]*.([^\""]*)\.[^>]*>([^<]*)<");
            Match matchResult = regexObj.Match(s);


            while (matchResult.Success)
            {
                Personne _acteur = new Personne();
                _acteur.Role = matchResult.Groups[1].ToString();
                _acteur.ID = matchResult.Groups[2].ToString();
                _acteur.Nom = matchResult.Groups[3].ToString();
                MonFilm.Acteurs.Add(_acteur);
                matchResult = matchResult.NextMatch();
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

            //Recupère le resultats des films
            string strURL = "http://iphone.allocine.fr/recherche/default.html?motcle=" + HttpUtility.UrlEncode(_Film.Titre, Encoding.Default) + "&rub=1";
            int _NbrFilms;
            int _NbrPages;

            string strBody = Utils.GetSourceHTML(strURL);
            if (strBody.Contains("Films (0)") == false)
            {
                _NbrFilms = Convert.ToInt32(Regex.Match(strBody, @"<b>Film \((.*)\)").Groups[1].ToString());
                _NbrPages = _NbrFilms / 10;

                for (int i = 1; i <= _NbrPages + 1; i++)
                {
                    strURL = "http://iphone.allocine.fr/recherche/default.html?motcle=" + HttpUtility.UrlEncode(_Film.Titre, Encoding.Default) + "&rub=1&page=" + i.ToString();
                    strBody = Utils.GetSourceHTML(strURL);
                    strBody = Regex.Replace(strBody, "[\r\n]", "");

                    MatchCollection myMatches = Regex.Matches(strBody, @"<a href=""/film/fichefilm_gen_cfilm=[0-9]+.*?</tr>");

                    foreach (Match movieCode in myMatches)
                    {
                        Film MonFilm = new Film();

                        string strMovieCode = movieCode.ToString();

                        MonFilm.AlloID = Regex.Match(strMovieCode, "/film/fichefilm_gen_cfilm=([0-9]+).html").Groups[1].ToString();

                        #region Cover
                        string URLThumbnail = Regex.Match(strMovieCode, @".*src=""(.*gif|.*jpg)"".*").Groups[1].ToString();
                        if (URLThumbnail.Contains("novignette") == false)
                        {
                            Thumb _cover = new Thumb();
                            _cover.URLMiniature = URLThumbnail;
                            MonFilm.ListeCover.Add(_cover);
                            
                        }
                        #endregion

                        MonFilm.Titre = Utils.removeUnwantedChars(Regex.Match(strMovieCode, @".*.html"">(.*?)</a>").Groups[1].ToString());
                        MonFilm.TitreOriginal = Utils.removeUnwantedChars(Regex.Match(strMovieCode, @"&nbsp;\((.*?)\)").Groups[1].ToString());
                        MonFilm.Annee = Utils.removeUnwantedChars(Regex.Match(strMovieCode, ">(\\d{4})<").Groups[1].ToString());

                        #region Réalisateurs
                        string[] _Réalisateurs;
                        _Réalisateurs = Utils.removeUnwantedChars(Regex.Match(strMovieCode, ".*de (.*?)<").Groups[1].ToString()).Replace(", ", ",").Split(',');
                        foreach (string item in _Réalisateurs)
                        {
                            Personne _real = new Personne();
                            _real.Nom = item;
                            MonFilm.Realisateurs.Add(_real);
                        }
                        #endregion

                        #region Acteurs
                        string[] _Acteurs;
                        _Acteurs = Utils.removeUnwantedChars(Regex.Match(strMovieCode, ".*avec (.*?)<").Groups[1].ToString()).Replace(", ", ",").Split(',');
                        foreach (string item in _Acteurs)
                        {
                            Personne _act = new Personne();
                            _act.Nom = item;
                            MonFilm.Acteurs.Add(_act);
                        }
                        #endregion

                        ListeFilm.Add(MonFilm);

                    }
                }
            }
            return ListeFilm;

        }



 
    }




}
