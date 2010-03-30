using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.IO;
using EmberAPI;
using EmberAPI.MediaContainers;

namespace CinePassion
{

    public class API
    {

        #region Enums
        public enum Query
        {
            Title = 1,
            IMDB = 2
        }

        public enum Lang
        {
            French = 1,
            English = 2
        }

        public enum Format
        {
            XML = 1,
        }
        #endregion


        const string API_KEY = "52c95b4ebfd66255aeea1ca131d93023";


        //Lance la recherche
        public FilmCollection Search(Movie MovieSearch, Query QueryType, Lang QueryLang, Format QueryFormat)
        {
            FilmCollection _Result = new FilmCollection();
            Film _Movie = new Film();
            string _QueryType = "Title";
            string _QueryLang = "fr";
            string _QueryFormat = "XML";
            string _Query = "";

            #region Options

            //Type de recherche
            switch (QueryType)
            {
                case Query.Title:
                    _QueryType = "Title";
                    _Query = MovieSearch.Title;
                    break;
                case Query.IMDB:
                    _QueryType = "IMDB";
                    _Query = MovieSearch.IMDBID;
                    break;
                default:
                    _QueryType = "Title";
                    _Query = MovieSearch.Title;
                    break;
            }

            //Langue
            switch (QueryLang)
            {
                case Lang.French:
                    _QueryLang = "fr";
                    break;
                case Lang.English:
                    _QueryLang = "en";
                    break;
                default:
                    _QueryLang = "en";
                    break;
            }

            //Format de retour
            switch (QueryFormat)
            {
                case Format.XML:
                    _QueryFormat = "XML";
                    break;
                default:
                    _QueryFormat = "XML";
                    break;
            }

            #endregion

            string _URLSearch = @"http://passion-xbmc.org/scraper/API/1/Movie.Search/" +
                _QueryType + "/" + _QueryLang + "/" + _QueryFormat + "/" + API_KEY + "/" + _Query;

            XmlDocument _XMLResult = Fetch(_URLSearch);
            XmlNodeList _ChildMovie = _XMLResult.GetElementsByTagName("movie");
            foreach (XmlNode _NodeMovie in _ChildMovie)
            {
                _Movie = new Film();
                _Movie.Id = _NodeMovie["id"].InnerText;
                _Movie.Id_Allocine = _NodeMovie["id_allocine"].InnerText;
                _Movie.Id_IMDB = _NodeMovie["id_imdb"].InnerText;
                _Movie.LastChange = _NodeMovie["last_change"].InnerText;
                _Movie.Url = _NodeMovie["url"].InnerText;
                _Movie.Title = _NodeMovie["title"].InnerText;
                _Movie.OriginalTitle = _NodeMovie["originaltitle"].InnerText;
                _Movie.Year = _NodeMovie["year"].InnerText;
                _Movie.Runtime = _NodeMovie["runtime"].InnerText;
                _Movie.Plot = _NodeMovie["plot"].InnerText;
                if (_NodeMovie["tagline"] != null) _Movie.Tagline = _NodeMovie["tagline"].InnerText;
                //_Movie.Title = _NodeMovie["runtime"].InnerText;
                //_Movie.Title = _NodeMovie["information"].InnerText;

                //Cover
                if (_NodeMovie["images"] != null)
                {
                    XmlNodeList _ChildImages = _NodeMovie["images"].ChildNodes;
                    foreach (XmlNode _NodeImage in _ChildImages)
                    {
                        Thumb _Image = new Thumb();

                        //if (_NodeImage.Attributes["type"].InnerText == "Poster" && _NodeImage.Attributes["size"].InnerText == "thumb")
                        //{
                        //    _Image.URLImage = _NodeImage.Attributes["url"].InnerText;
                        //    _Movie.ListeCover.Add(_Image);
                        //}
                        if (_NodeImage.Attributes["type"].InnerText == "Poster" && _NodeImage.Attributes["size"].InnerText == "preview")
                        {
                            _Image.URLImage = _NodeImage.Attributes["url"].InnerText;
                            _Movie.ListeCover.Add(_Image);
                        }
                        if (_NodeImage.Attributes["type"].InnerText == "Fanart" && _NodeImage.Attributes["size"].InnerText == "original")
                        {
                            _Image.URLImage = _NodeImage.Attributes["url"].InnerText;
                            _Movie.ListeFanart.Add(_Image);
                        }

                    }
                }

                _Result.Add(_Movie);
            }
       

            return _Result;
        }

        public FilmCollection Search(Movie _MovieSearch)
        {
            return Search(_MovieSearch, Query.Title, Lang.French, Format.XML);
        }

        //Télécharge le doc XML
        public static XmlDocument Fetch(string url)
        {
            int attempt = 0;
            while (attempt < 2)
            {
                attempt++;
                try
                {
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                    req.UserAgent = "EMM";
                    req.Timeout = 10000;
                    WebResponse resp = req.GetResponse();
                    try
                    {
                        using (Stream s = resp.GetResponseStream())
                        {
                            //string sSourceXML = "";

                            //StreamReader rea = new StreamReader(s, Encoding.UTF8);
                            //sSourceXML = rea.ReadToEnd();
                            XmlDocument doc = new XmlDocument();
                            doc.Load(s);
                            //rea.Close();
                            resp.Close();
                            s.Close();
                            return doc;
                        }
                    }
                    finally
                    {
                        resp.Close();
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
