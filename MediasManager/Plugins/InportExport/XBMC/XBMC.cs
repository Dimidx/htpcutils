using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MediaManager.Library;
using MediaManager.Plugins;
using XBMC;

namespace MediaManager.Plugins
{
    public class XBMC : IMMPluginImportExport
    {

        public string Name { get { return "XBMC"; } }
        public string Author { get { return "Danone-KiD"; } }
        public string Version { get { return "1.0"; } }
        public string Description { get { return "Gestion des fichiers NFO de XBMC"; } }


        public List<MMPluginOption> GetOptions()
        {
            List<MMPluginOption> options = new List<MMPluginOption>();
            return options;
        }

        public Film Import(FileInfo _FileInfo)
        {
            Film MonFilm = new Film();

            #region Chargement du NFO

            string _pathNFO = "";
            if ((File.Exists(_FileInfo.DirectoryName + "/movie.nfo")))
            {
                _pathNFO = _FileInfo.DirectoryName + "/movie.nfo";

            }
            if (File.Exists(_FileInfo.FullName.Replace(_FileInfo.Extension, ".nfo")))
            {
                _pathNFO = _FileInfo.FullName.Replace(_FileInfo.Extension, ".nfo");

            }

            #endregion


            NfoMovie Nfo = NfoFile.getNfoMovie(_pathNFO);
            if (Nfo != null)
            {
                MonFilm.Titre = Nfo.Title;
                MonFilm.TitreOriginal = Nfo.Title;
                MonFilm.Annee = Nfo.Year;
                MonFilm.Avis = Nfo.Mpaa;
                MonFilm.Critique = Nfo.Outline;

                #region Date de sortie
                try
                {
                    MonFilm.DateSortie = DateTime.Parse(Nfo.Premiered);
                }
                catch { }
                #endregion

                MonFilm.DureeChaine = Nfo.Runtime;
                MonFilm.Genres = Nfo.Genre.Split('/');
                MonFilm.ID = Nfo.Id;

                #region Note
                try
                {
                    MonFilm.NotePresse = Convert.ToInt32(Nfo.Rating);
                }
                catch { }
                #endregion

                MonFilm.Synopsis = Nfo.Plot;

                #region Réalisateurs
                foreach (string dir in Nfo.Director.Split('/'))
                {
                    MonFilm.Realisateurs.Add(new Personne(dir, "Réalisateur"));
                }
                #endregion

                #region Acteurs
                foreach (Actor act in Nfo.Actor)
                {
                    MonFilm.Acteurs.Add(new Personne(act.Name, act.Role));
                }
                #endregion

            }

            #region Chargement des posters
            string[] _PathsPosters = new string[7]; //Chemin possible des posters
            _PathsPosters[0] = _FileInfo.FullName.Replace(_FileInfo.Extension, ".tbn");
            _PathsPosters[1] = _FileInfo.FullName.Replace(_FileInfo.Extension, ".jpg");
            _PathsPosters[2] = _FileInfo.DirectoryName + "/movie.tbn";
            _PathsPosters[3] = _FileInfo.DirectoryName + "/movie.jpg";
            _PathsPosters[4] = _FileInfo.DirectoryName + "/poster.tbn";
            _PathsPosters[5] = _FileInfo.DirectoryName + "/poster.jpg";
            _PathsPosters[6] = _FileInfo.DirectoryName + "/folder.jpg";

            string _pathPoster = "";
            foreach (string item in _PathsPosters)
            {
                if (File.Exists(item)) _pathPoster = item;
            }
            if (_pathPoster != "")
            {
                MonFilm.ListeCover.Add(new Thumb(_pathPoster));
            }
            #endregion

            #region Chargement des Fanarts
            string[] _PathsFanarts = new string[4]; //Chemin possible des fanarts
            _PathsFanarts[0] = _FileInfo.FullName.Replace(_FileInfo.Extension, "-fanart.jpg");
            _PathsFanarts[1] = _FileInfo.FullName.Replace(_FileInfo.Extension, ".fanart.jpg");
            _PathsFanarts[2] = _FileInfo.DirectoryName + "/fanart.jpg";
            _PathsFanarts[3] = _FileInfo.DirectoryName + "/backdrop.jpg";

            string _pathFanart = "";
            foreach (string item in _PathsFanarts)
            {
                if (File.Exists(item)) _pathFanart = item;
            }
            if (_pathFanart != "")
            {
                MonFilm.ListeFanart.Add(new Thumb(_pathFanart));
            }

            #endregion


            return MonFilm;

        }


        public bool Export(Film _Film)
        {
            return true;
        }



    }
}
