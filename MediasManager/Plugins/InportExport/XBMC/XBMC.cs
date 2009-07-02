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
                MonFilm.AlloID = Nfo.AlloId;
                MonFilm.Studio = Nfo.Studio;
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
                foreach (string dir in Nfo.Director.Split(','))
                {
                    MonFilm.Realisateurs.Add(new Personne(dir.Trim(), "Réalisateur"));
                }
                #endregion

                #region Acteurs
                foreach (Actor act in Nfo.Actor)
                {
                    MonFilm.Acteurs.Add(new Personne(act.Name.Trim(), act.Role));
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


        public bool Export(Film _Film, FileInfo _FileInfo)
        {
            NfoMovie Nfo = new NfoMovie();
            Nfo.Title = _Film.Titre;
            Nfo.OriginalTitle = _Film.TitreOriginal;
            Nfo.Year = _Film.Annee;
            Nfo.Mpaa = _Film.Avis;
            Nfo.Outline = _Film.Critique;
            Nfo.AlloId = _Film.AlloID;
            Nfo.Studio = _Film.Studio;
            #region Date de sortie
            try
            {
                Nfo.Premiered = _Film.DateSortie.ToString("d");
            }
            catch { }
            #endregion
            Nfo.Runtime = _Film.DureeChaine;
            #region Genres
            foreach (string item in _Film.Genres)
            {
                Nfo.Genre += item.Trim() + "/";
            }
            if (Nfo.Genre.Length > 0) Nfo.Genre = Nfo.Genre.Substring(0, Nfo.Genre.Length - 1);
            #endregion
            Nfo.Id = _Film.ID;
            Nfo.Rating = _Film.NotePresse.ToString();
            Nfo.Plot = _Film.Synopsis;
            #region Réalisateurs
            foreach (Personne real in _Film.Realisateurs)
            {
                Nfo.Director += real.Nom + ", ";
            }
            if (Nfo.Director.Length > 0) Nfo.Director = Nfo.Director.Substring(0, Nfo.Director.Length - 2);
            #endregion
            #region Acteurs
            foreach (Personne act in _Film.Acteurs)
            {
                Actor _act = new Actor();
                _act.Name = act.Nom;
                _act.Role = act.Role;
                //_act.Thumb = act.Photo.URLImage;
                Nfo.Actor.Add(_act);
            }
            #endregion

            #region Affiche
            try
            {
                string _PostersPath = _FileInfo.FullName.Replace(_FileInfo.Extension, ".tbn");
                if (_Film.Cover.IsCached)
                {
                    if (File.Exists(_PostersPath)) File.Delete(_PostersPath);
                    File.Copy(_Film.Cover.FichierCache, _PostersPath);
                }
                if (_Film.Cover.IsLocal && _Film.Cover.URLImage != _PostersPath)
                {
                    if (File.Exists(_PostersPath)) File.Delete(_PostersPath);
                    File.Copy(_Film.Cover.URLImage, _PostersPath);
                }
            }
            catch {}
            #endregion

            #region Fanart
            try
            {
                string _FanartPath = _FileInfo.FullName.Replace(_FileInfo.Extension, "-fanart.jpg");
                if (_Film.Fanart.IsCached)
                {
                    if (File.Exists(_FanartPath)) File.Delete(_FanartPath);
                    File.Copy(_Film.Fanart.FichierCache, _FanartPath);
                }
                if (_Film.Fanart.IsLocal && _Film.Fanart.URLImage != _FanartPath)
                {
                    if (File.Exists(_FanartPath)) File.Delete(_FanartPath);
                    File.Copy(_Film.Fanart.URLImage, _FanartPath);
                }
            }
            catch { }
            #endregion

            NfoFile.saveNfoMovie(Nfo, _FileInfo.FullName.Replace(_FileInfo.Extension, ".nfo"));
            return true;
        }



    }
}
