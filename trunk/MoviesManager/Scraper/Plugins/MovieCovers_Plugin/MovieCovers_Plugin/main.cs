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
using WL;

namespace MovieCovers_Plugin
{
    public class main 
    {
        //public string PluginName { get { return "MovieCovers"; } }
        //public string Author { get { return "Danone-KiD"; } }
        //public string Version { get { return "1.0"; } }

   

        public PluginResultat[] SearchMovie(Movie TheFilm)
        {
            PluginResultat[] _TabResult = null;
            List<PluginResultat> _ListResult = null;

            //Recupère le resultats des films
            string strURL = "http://www.moviecovers.com/multicrit.html?titre=" + HttpUtility.UrlEncode(TheFilm.Title, Encoding.Default) + "&slow=1&tri=Titre";
            int _NbrFilms;
            
            string strBody = GetSourceHTML(strURL);

            if (strBody.Contains("D&eacute;sol&eacute;") == false)
            {
                _NbrFilms = Convert.ToInt32(Regex.Match(strBody, @".* ([0-9]+) film.*").Groups[1].ToString());

                MatchCollection myMatches = Regex.Matches(strBody, @".*/film/titre_(.*).html"">(.*)</A> \((.*)\)");

                foreach (Match movieCode in myMatches)
                {
                    //Le film trouvé
                    PluginResultat _result = null;

                    string strMovieCode = movieCode.ToString();

                    string strLink = Regex.Match(strMovieCode, @".*/film/titre_(.*).html"">(.*)</A> \((.*)\)").ToString();
                    _result.URL = @"http://www.moviecovers.com/film/titre_" + movieCode.Groups[1].ToString()+".html";
                    _result.ID = movieCode.Groups[1].ToString();
                    _result.Titre = movieCode.Groups[2].ToString();
                    _result.Annee = movieCode.Groups[3].ToString();

                    _ListResult.Add(_result);

                }
            }


            _TabResult = _ListResult.ToArray();

            return _TabResult;
        }


        public Movie GetMovie(PluginResultat ResultSelected)
        {
            Movie MonFilm = new Movie();
            string s = GetSourceHTML("http://www.moviecovers.com/film/titre_" + ResultSelected.ID + ".html");

            s = Regex.Replace(s, "[\r\n]", "");


           //La liste des covers
            List<Thumb> _ListeCovers = new List<Thumb>();

            string strThumbURL = "http://www.moviecovers.com/getjpg.html/" + ResultSelected.ID + ".jpg";
            Thumb _Cover = new Thumb();
            _Cover.URL = strThumbURL;
            _Cover.URLMiniature = strThumbURL;
            _ListeCovers.Add(_Cover);
            MonFilm.Thumbs = _ListeCovers.ToArray();

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
