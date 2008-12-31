using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web;
using System.Threading;
using Scraper;

namespace TestPlug
{
    public class Class1 : ScraperPlugin

    {
        public string PluginName { get { return "Allociné"; } }
        public string Author { get { return "Danone-KiD"; } }
        public string Version { get { return "1.0"; } }

        public Movie[] SearchMovie(string MovieName)
        {
            Movie[] _result = null;
            List<Movie> _ListeResult = new List<Movie>();
            

            //Recupère le resultats des films
            string strURL = "http://www.allocine.fr/recherche/?motcle=" + HttpUtility.UrlEncode(MovieName, Encoding.Default) + "&rub=1";
            int _NbrFilms;
            int _NbrPages;

            string strBody = GetSourceHTML(strURL);

            _NbrFilms = Convert.ToInt32(Regex.Match(strBody, @".*Films <h4>\((.*) réponse").Groups[1].ToString());
            _NbrPages = _NbrFilms / 20;

            for (int i = 1; i <= _NbrPages+1; i++)
            {
                strURL = "http://www.allocine.fr/recherche/default.html?motcle=" + HttpUtility.UrlEncode(MovieName, Encoding.Default) + "&rub=1&page=" + i.ToString();
                strBody = GetSourceHTML(strURL);
                MatchCollection myMatches = Regex.Matches(strBody, "<a href=\"/film/fichefilm_gen_cfilm=\\d{4,6}.html\">.*?</table>");

                foreach (Match movieCode in myMatches)
                {

                    //Le filme trouvé
                    Movie _Movie = new Movie();

                    //La liste des covers
                    List<ImageMovie>_ListeCovers = new List<ImageMovie>();

                    //La liste des fanart
                    List<ImageMovie>_ListeFanart = new List<ImageMovie>();

                    string strMovieCode = movieCode.ToString();

                    string strLink = Regex.Match(strMovieCode, "/film/fichefilm_gen_cfilm=\\d{4,6}.html").ToString();
                    _Movie.URLPage = "http://www.allocine.fr" + strLink;
                    _Movie.ID = removeUnwantedChars(Regex.Match(strLink, ".*?cfilm=(.*?).html").Groups[1].ToString());
                    string URLThumbnail = Regex.Match(strMovieCode, @".*src=""(.*gif|.*jpg)"".*").Groups[1].ToString();
                    if (URLThumbnail.Contains("empty") == false)
                    {
                        ImageMovie _Cover = new ImageMovie();
                        _Cover.URLImage = URLThumbnail;
                        _Cover.URLThumb = URLThumbnail;
                        _ListeCovers.Add(_Cover);
                    }
                    else
                    {
                        ImageMovie _Cover = new ImageMovie();
                        _Cover.URLImage = "";
                        _Cover.URLThumb = "";
                        _ListeCovers.Add(_Cover);
                    }

 
                    _Movie.Cover = _ListeCovers.ToArray();
                    _Movie.Title = removeUnwantedChars(Regex.Match(strMovieCode, "<h4>.*?</h4>").ToString());
 
                    //_Movie.TitreOriginal = removeUnwantedChars(Regex.Match(strMovieCode, "<h5>.*?</h5>").ToString());
                    _Movie.Year = removeUnwantedChars(Regex.Match(strMovieCode, ">(\\d{4})<").Groups[1].ToString());
                    _ListeResult.Add(_Movie);

                }
            }


            _result = _ListeResult.ToArray();

            return _result;
        }

 
        public Movie GetMovie(string id)
        {
            Movie MonFilm = new Movie();
            string s = GetSourceHTML("http://www.allocine.fr/film/fichefilm_gen_cfilm=" + id + ".html");



            s = Regex.Replace(s, "[\r\n]", "");

            MonFilm.ID = id;
            MonFilm.Title = removeUnwantedChars(Regex.Match(s, "<h1[^>]*>(.*?)</h1>").ToString());
            //MonFilm.TitreOriginal = removeUnwantedChars(Regex.Match(s, "<h4>Titre original : <i>(.*?)</i>").Groups[1].ToString());

            //MonFilm.DureeChaine = removeUnwantedChars(Regex.Match(s, "<h4>Durée : (.*?)</h4>").Groups[1].ToString());

            MonFilm.Year = removeUnwantedChars(Regex.Match(s, "Année de production : (\\d{4})").Groups[1].ToString());
            //MonFilm.Realisateurs = removeUnwantedChars(Regex.Match(s, "Réalisé par(.*?)</h4>").Groups[1].ToString()).Replace(", ", ",").Split(',');
            //MonFilm.Acteurs = removeUnwantedChars(Regex.Match(s, "<h4>Avec(.*?)</h4>").Groups[1].ToString()).Replace(", ", ",").Split(',');
            //MonFilm.Genre = removeUnwantedChars(Regex.Match(s, "Genre :(.*?)</h4>").Groups[1].ToString()).Replace(", ", ",").Split(',');


            //Note Presse
            string sNotePresse = removeUnwantedChars(Regex.Match(s, "Presse.*?class=['\"]etoile_(\\d)['\"]").Groups[1].ToString());
            try
            {
                //MonFilm.NotePresse = (float)(System.Double.Parse(sNotePresse) * 2.5);
            }
            catch { }

            //Note Spectateurs
            string sNoteSpectateur = removeUnwantedChars(Regex.Match(s, "Spectateurs.*?class=['\"]etoile_(\\d)['\"]").Groups[1].ToString());
            try
            {
                //MonFilm.NoteSpectateurs = (float)(System.Double.Parse(sNotePresse) * 2.5);
            }
            catch { }

            //MonFilm.Synopsis = removeUnwantedChars(Regex.Match(s, "Synopsis(.*?)</h4></div>").Groups[1].ToString());

            //Jaquette
  
            //La liste des covers
            List<ImageMovie> _ListeCovers = new List<ImageMovie>();
            //Allocine
            string strThumbURL = Regex.Match(s, "/film/galerievignette_gen_cfilm=.+?html").ToString();
            if (strThumbURL != "")
            {
                strThumbURL = "http://www.allocine.fr" + strThumbURL;
                string strBodyThumb = GetSourceHTML(strThumbURL);
                if (strBodyThumb != null && strBodyThumb != "")
                {
                    strBodyThumb = Regex.Replace(strBodyThumb, "[\r\n]", "");
                    strThumbURL = removeUnwantedChars(Regex.Match(strBodyThumb, "id=['\"]imgNormal['\"] class=['\"]photo['\"] src=['\"](.*?)['\"]").Groups[1].ToString());

                    
                        ImageMovie _Cover = new ImageMovie();
                        _Cover.URLImage = strThumbURL;
                        _Cover.URLThumb = strThumbURL;
                        _ListeCovers.Add(_Cover);

                }
            }
            MonFilm.Cover = _ListeCovers.ToArray();

            return MonFilm;
        }


        static string removeUnwantedChars(string tInput)
        {
            tInput = Regex.Replace(tInput, "<[^<]*>", "");
            tInput = Regex.Replace(tInput, "Plus.*?...", "");
            tInput = tInput.Replace("\\s{2,}", " ");
            tInput = tInput.Replace("&nbsp;", "").Trim();
            return tInput;
        }

        private static string GetSourceHTML(string url)
        {
            string sourceHTML = string.Empty;

            try
            {

                WebClient m_webClient = new WebClient();

                //Proxy
                WebProxy wProxy = new WebProxy("10.126.71.12", 80);
                wProxy.Credentials = new NetworkCredential("rfraftp", "Siberbo2000", "fr");
                m_webClient.Proxy = wProxy;
                //Proxy

                //m_webClient.Encoding = Encoding.UTF8;
                sourceHTML = m_webClient.DownloadString(url);

            }
            catch (Exception e)
            {
                throw new Exception("An exception occured when you tried to download the file: " + e.Message);
            }

            return sourceHTML;

        }

    }
}
