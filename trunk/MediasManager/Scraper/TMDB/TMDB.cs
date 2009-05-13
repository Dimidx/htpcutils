using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaManager.Library;

namespace MediaManager.Library
{
    public class TMDB : MediaManager.Library.MovieScraper
    {
        public string URL { get { return "http://www.themoviedb.org"; } }
        public string Name { get { return "TheMovieDB"; } }
        public string Author { get { return "Danone-KiD"; } }

        public Film GetMovie(Film _Film)
        {

            Film MonFilm = new Film();
            MonFilm.Titre = "Test TMDB";
            Thumb _cover = new Thumb();
            _cover.URLImage = "file:///D:/20090311122354158_0001.jpg";

            MonFilm.ListeCover.Add(_cover);

            return MonFilm;


            //
        }


        public List<Film> SearchMovie(Film _Film)
        {
            List<Film> _results = new List<Film>();
            Film _film = new Film();
            _film.Titre = "Test TMDB";
            _results.Add(_film);
            return _results;
        }



    }
}
