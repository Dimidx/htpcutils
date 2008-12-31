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

namespace MovieCovers_Plugin
{
    public class main : ScraperPlugin
    {
        public string PluginName { get { return "MovieCovers"; } }
        public string Author { get { return "Danone-KiD"; } }
        public string Version { get { return "1.0"; } }

        public Movie[] SearchMovie(string MovieName)
        {
            Movie[] _result = null;
            List<Movie> _ListeResult = new List<Movie>();


   
            //Recupère le resultats des films
            string strURL = "http://www.moviecovers.com/multicrit.html?titre=" + HttpUtility.UrlEncode(MovieName, Encoding.Default) + "&slow=1&tri=Titre";
            int _NbrFilms;
            
            string strBody = GetSourceHTML(strURL);

            if (strBody.Contains("D&eacute;sol&eacute;") == false)
            {
                _NbrFilms = Convert.ToInt32(Regex.Match(strBody, @".* ([0-9]+) film.*").Groups[1].ToString());

                MatchCollection myMatches = Regex.Matches(strBody, @".*/film/titre_(.*).html"">(.*)</A> \((.*)\)");

                foreach (Match movieCode in myMatches)
                {

                    //Le filme trouvé
                    Movie _Movie = new Movie();

                    //La liste des covers
                    List<ImageMovie> _ListeCovers = new List<ImageMovie>();

                    //La liste des fanart
                    List<ImageMovie> _ListeFanart = new List<ImageMovie>();

                    string strMovieCode = movieCode.ToString();

                    string strLink = Regex.Match(strMovieCode, @".*/film/titre_(.*).html"">(.*)</A> \((.*)\)").ToString();
                    _Movie.URLPage = @"http://www.moviecovers.com/film/titre_" + movieCode.Groups[1].ToString()+".html";
                    _Movie.ID = movieCode.Groups[1].ToString();
                    _Movie.Title = movieCode.Groups[2].ToString();
                    _Movie.Year = movieCode.Groups[3].ToString();


                    string _URLCover = @"http://www.moviecovers.com/DATA/thumbs/films-" + movieCode.Groups[2].ToString().Substring(0, 1).ToLower() + "/" + movieCode.Groups[1].ToString() + ".jpg";

                        ImageMovie _Cover = new ImageMovie();
                        _Cover.URLImage = _URLCover;
                        _Cover.URLThumb = _URLCover;
                        _ListeCovers.Add(_Cover);


                    _Movie.Cover = _ListeCovers.ToArray();
                    _ListeResult.Add(_Movie);

                }
            }


            _result = _ListeResult.ToArray();

            return _result;
        }


        public Movie GetMovie(string id)
        {
            Movie MonFilm = new Movie();
            string s = GetSourceHTML("http://www.moviecovers.com/film/titre_" + id + ".html");

            s = Regex.Replace(s, "[\r\n]", "");

           //La liste des covers
            List<ImageMovie> _ListeCovers = new List<ImageMovie>();

            string strThumbURL = "http://www.moviecovers.com/getjpg.html/" + id + ".jpg";
            ImageMovie _Cover = new ImageMovie();
            _Cover.URLImage = strThumbURL;
            _Cover.URLThumb = strThumbURL;
            _ListeCovers.Add(_Cover);
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
