using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using MediaManager.Library;
using MediaManager.Plugins;

namespace MediaManager.Plugins
{
    public class TMDB : IMMPluginScraper
    {
        public string URL { get { return "http://www.themoviedb.org"; } }
        public string Name { get { return "TheMovieDB"; } }
        public string Author { get { return "Danone-KiD"; } }
        public string Version { get { return "1.0"; } }
        public string Description { get { return ""; } }


        private static String APIKey = "1a9efd23fff9c2ed07c90358e2b3d280";
        private static String osUri = "http://a9.com/-/spec/opensearch/1.1/";
        private String urlSearchMovie = @"http://api.themoviedb.org/2.0/Movie.search?api_key=" + APIKey + "&title=";
        private String urlMovieInfo = @"http://api.themoviedb.org/2.0/Movie.getInfo?api_key=" + APIKey + "&id=";
        //private String defaultCacheDir = System.Environment.CurrentDirectory + @"\Cache\MovieCache\";
        //private DateTime dtDefaultCache = DateTime.Now.Subtract(new TimeSpan(14, 0, 0, 0));

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

            //Film MonFilm = new Film();
            //MonFilm.Titre = "Test TMDB";
            //Thumb _cover = new Thumb();
            ////_cover.URLImage = "file:///D:/20090311122354158_0001.jpg";

            //MonFilm.ListeCover.Add(_cover);

            //return MonFilm;



            XmlNode node;
            XmlDocument xdoc = new XmlDocument();

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xdoc.NameTable);
            nsMgr.AddNamespace("opensearch", osUri);
            //if (File.Exists(defaultCacheDir + "\\" + MovieID + ".xml") && (DateTime.Compare(File.GetLastWriteTime(defaultCacheDir + "\\" + MovieID + ".xml"), dtDefaultCache) > 0))
            //{
            //    xdoc.Load(defaultCacheDir + "\\" + MovieID + ".xml");
            //}
            //else
            //{
            xdoc.Load(urlMovieInfo + _Film.ID);
            //}
            node = xdoc.DocumentElement;
            XmlNodeList nlMovie = node.SelectNodes("/results/moviematches/movie");

            if (nlMovie[0].FirstChild == null)
                throw new Exception("no results");

            Film m = new Film();

            if (nlMovie[0].SelectSingleNode("release") != null)
                try
                {
                    m.Annee = nlMovie[0].SelectSingleNode("release").InnerText.Substring(0, 4);
                }
                catch (Exception ex)
                {

                }
            if (nlMovie[0].SelectSingleNode("short_overview") != null)
                m.Synopsis = nlMovie[0].SelectSingleNode("short_overview").InnerText;

            if (nlMovie[0].SelectSingleNode("runtime") != null)
                m.Duree = nlMovie[0].SelectSingleNode("runtime").InnerText;

            #region Affiches

            XmlNodeList xnl = nlMovie[0].SelectNodes("poster[@size='original']");
            int n = 0;
            foreach (XmlNode x in xnl)
            {
                XmlNode th = nlMovie[0].SelectNodes("poster[@size='thumb']")[n];
                Thumb p = new Thumb();
                p.URLImage = x.InnerText;
                //if (th != null) p.URLMiniature = th.InnerText;
                m.ListeCover.Add(p);
                n++;
            }
	        #endregion

            #region Fanarts

            XmlNodeList xnlfan = nlMovie[0].SelectNodes("backdrop[@size='original']");
            int j = 0;
            foreach (XmlNode x in xnlfan)
            {
                XmlNode th = nlMovie[0].SelectNodes("backdrop[@size='thumb']")[j];
                Thumb p = new Thumb();
                p.URLImage = x.InnerText;
                //if (th != null ) p.URLMiniature = th.InnerText;
 
                m.ListeFanart.Add(p);
                j++;
            }
            #endregion

            //if (nlMovie[0].SelectSingleNode("rating") != null)
            //    m.NoteSpectateurs = Convert.ToDouble([0].SelectSingleNode("rating").InnerText);

            m.Titre = nlMovie[0].SelectSingleNode("title").InnerText;
            m.ID = (nlMovie[0].SelectSingleNode("id") == null) ? nlMovie[0].SelectSingleNode("TMDbId").InnerText : nlMovie[0].SelectSingleNode("id").InnerText;

            //Get the actors/directors/etc
            XmlNodeList nlActors = node.SelectNodes("/results/moviematches/movie/people/person");
            foreach (XmlNode x in nlActors)
            {
                m.Acteurs.Add(new Personne()
                {
                    Nom = x.SelectSingleNode("name").InnerText,
                    //Type = x.Attributes["job"].Value,
                    Role = (x.SelectSingleNode("role").InnerText == null) ? "" : x.SelectSingleNode("role").InnerText.Trim()
                });
            }

            //Get the genres
            XmlNodeList nlGenres = node.SelectNodes("/results/moviematches/movie/categories/category");
                            List<string> _genres = new List<string>();
            foreach (XmlNode x in nlGenres)
            {
                _genres.Add(x.SelectSingleNode("name").InnerText);
            }
            m.Genres = _genres.ToArray();
            //Cache metadata
            //try
            //{
            //    // Message("Caching Metadata", MediaScoutMessage.MessageType.ProcessSeries, DateTime.Now);
            //    xdoc.Save(defaultCacheDir + "\\" + MovieID + ".xml");
            //}
            //catch (Exception ex)
            //{
            //    //Message("Error caching metadata: " + ex.Message, MediaScoutMessage.MessageType.Error, DateTime.Now);
            //}

            return m;



            //
        }


        public List<Film> SearchMovie(Film _Film)
        {
            List<Film> _results = new List<Film>();

            //TheMovieDB doesn't handle & very well, so convert to "AND"

            string MovieName = "";
            if (_Film.TitreOriginal != null)
            {
                MovieName = _Film.TitreOriginal.Replace("&", "and");
            }
            else
            {
                MovieName = _Film.Titre.Replace("&", "and");
            }


            XmlDocument xdoc = new XmlDocument();
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xdoc.NameTable);
            nsMgr.AddNamespace("opensearch", osUri);
            xdoc.Load(urlSearchMovie + _Film.Titre);
            XmlNode node = xdoc.DocumentElement;
            XmlNodeList xnl = node.SelectNodes("/results/moviematches/movie");

            for (int i = 0; i < xnl.Count; i++)
            {
                Film m = new Film();
                if (xnl[i]["title"] != null)
                    m.Titre = xnl[i]["title"].InnerText;

                if (xnl[i]["id"] != null)
                    m.ID = xnl[i]["id"].InnerText;

                if (xnl[i]["short_overview"] != null)
                    m.Synopsis = xnl[i]["short_overview"].InnerText;

                if (xnl[i]["release"] != null)
                {
                    m.DateSortie = DateTime.Parse(xnl[i]["release"].InnerText);
                    m.Annee = xnl[i]["release"].InnerText.Substring(0,4);
                
                }


                Thumb _affiche = new Thumb();

                if (xnl[i].SelectSingleNode("poster[@size='original']") != null)
                    _affiche.URLImage = xnl[i].SelectSingleNode("poster[@size='original']").InnerText;

                if (xnl[i].SelectSingleNode("poster[@size='thumb']") != null)
                    //_affiche.URLMiniature = xnl[i].SelectSingleNode("poster[@size='thumb']").InnerText;

                _results.Add(m);
            }

            return _results;
 
        }



    }
}
