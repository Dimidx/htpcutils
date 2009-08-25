using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using MediaManager.Library;

namespace MediaManager.Database
{
    public class Database
    {
        private static readonly string _SqliteConnString = "Data Source=MediaManager.sqlite;Version = 3";

        private static SQLiteConnection _sql_conn = new SQLiteConnection(_SqliteConnString);

        private static SQLiteCommand _sql_cmd = _sql_conn.CreateCommand();

        private static SQLiteDataReader _sql_datareader;

        /// <summary>
        /// Create the Database for iMedia in the Homefolder of iMedia
        /// </summary>
        public static void CreateDB()
        {
            SQLiteConnection sqlCn = new SQLiteConnection(_SqliteConnString);

            sqlCn.Open();

            SQLiteCommand sqlcom = sqlCn.CreateCommand();

            StringBuilder sbCreateTables = new StringBuilder();

            #region Films

                sbCreateTables.Append(@"CREATE TABLE Films (
                IDFilm INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
                Titre TEXT,
                TitreOriginal TEXT,
                ImdbID TEXT,
                AlloID TEXT,
                Annee TEXT,
                Accroche TEXT,
                Resume TEXT,
                Synopsis TEXT,
                Duree TEXT,
                Note FLOAT,
                Votes FLOAT,
                MPAA TEXT,
                Certification TEXT,
                Top250 TEXT,
                Studio TEXT,
                DateSortie DATETIME,
                Vu BOOL,
                Path TEXT,
                PathCover TEXT,
                PathFanart TEXT, 
                PathNFO TEXT,
                PathBA TEXT);");

            #endregion

                sbCreateTables.Append("CREATE UNIQUE INDEX UniquePath ON Films (Path);");

            sqlcom.CommandText = sbCreateTables.ToString();
            try
            {
                sqlcom.ExecuteNonQuery();
            }
            catch (Exception)
            {
                Console.WriteLine("Impossible de créer la base de données");
            }

            sqlCn.Close();
        }

        public static Film GetFilm(string Path)
        {
             

            return new Film();

        }

        public static long GetIDFilm(string _path)
        {
            long _IDFilm = 0;
            SQLiteConnection SqliteConnEx = new SQLiteConnection(_SqliteConnString);
            SQLiteCommand SqliteComEx = SqliteConnEx.CreateCommand();
            SqliteConnEx.Open();
            SqliteComEx.CommandText = "SELECT IDFilm FROM Films WHERE Path = @Path;";
            SqliteComEx.Parameters.AddWithValue("@Path", _path);
            SQLiteDataReader _SQLReader = SqliteComEx.ExecuteReader();
            if (!DBNull.Value.Equals(_SQLReader["IDFilm"]))
            {
                _IDFilm = Convert.ToInt64(_SQLReader["IDFilm"]);
            }

            SqliteConnEx.Close();
            return _IDFilm;

        }


        public static bool SaveFilmToDB(Film Film,string _Path)
        {
            //StringBuilder _SQLUpdate = new StringBuilder();

            SQLiteConnection SqliteConnEx = new SQLiteConnection(_SqliteConnString);

            SQLiteCommand SqliteComEx = SqliteConnEx.CreateCommand();

            SqliteConnEx.Open();
            SqliteComEx.CommandText = @"INSERT OR REPLACE INTO Films (
                IDFilm,
                Titre ,
                TitreOriginal ,
                ImdbID ,
                AlloID ,
                Annee ,
                Accroche ,
                Resume ,
                Synopsis ,
                Duree ,
                Note ,
                Votes ,
                MPAA ,
                Certification ,
                Top250 ,
                Studio ,
                DateSortie ,
                Vu ,
                Path,
                PathCover,
                PathFanart
                ) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
            long _IDFilm = GetIDFilm(_Path);
            if (_IDFilm == 0)
            {
                SqliteComEx.CommandText.Replace("IDFilm,", "").Replace("(?,", "(");
            }
            else
            {
                 SqliteComEx.Parameters.AddWithValue("IDFilm", _IDFilm);
            }
            SqliteComEx.Parameters.AddWithValue("Titre", Film.Titre);
            SqliteComEx.Parameters.AddWithValue("TitreOriginal", Film.TitreOriginal);
            SqliteComEx.Parameters.AddWithValue("ImdbID", Film.ID);
            SqliteComEx.Parameters.AddWithValue("AlloID", Film.AlloID);
            SqliteComEx.Parameters.AddWithValue("Annee", Film.Annee);
            SqliteComEx.Parameters.AddWithValue("Accroche", Film.Accroche);
            SqliteComEx.Parameters.AddWithValue("Resume", Film.Resume);
            SqliteComEx.Parameters.AddWithValue("Synopsis", Film.Synopsis);
            SqliteComEx.Parameters.AddWithValue("Duree", Film.Duree);
            SqliteComEx.Parameters.AddWithValue("Note", Film.Note);
            SqliteComEx.Parameters.AddWithValue("Votes", Film.Votes);
            SqliteComEx.Parameters.AddWithValue("MPAA", Film.MPAA);
            SqliteComEx.Parameters.AddWithValue("Certification", Film.Certification);
            SqliteComEx.Parameters.AddWithValue("Top250", Film.Top250);
            SqliteComEx.Parameters.AddWithValue("Studio", Film.Studio);
            SqliteComEx.Parameters.AddWithValue("DateSortie", Film.DateSortie);
            SqliteComEx.Parameters.AddWithValue("Vu", Film.Vu);
            SqliteComEx.Parameters.AddWithValue("Path", _Path);
            if (Film.Cover != null)
            {
                SqliteComEx.Parameters.AddWithValue("PathCover", Film.Cover.URLImage);
            }
            else
            {
                SqliteComEx.Parameters.AddWithValue("PathCover", "");
            }
            if (Film.Fanart != null)
            {
                SqliteComEx.Parameters.AddWithValue("PathFanart", Film.Fanart.URLImage);
            }
            else
            {
                SqliteComEx.Parameters.AddWithValue("PathFanart", "");
            }
            SqliteComEx.ExecuteNonQuery();

            SqliteConnEx.Close();

         
            return true;

        }

        /// <summary>
        /// Execute SQL Commands
        /// </summary>
        /// <param name="sqlCommand">SQL Command</param>
        private static void ExecuteSQL(string sqlCommand)
        {

            SQLiteConnection SqliteConnEx = new SQLiteConnection(_SqliteConnString);

            SQLiteCommand SqliteComEx = SqliteConnEx.CreateCommand();

            SqliteConnEx.Open();

            SqliteComEx.CommandText = sqlCommand;

            SqliteComEx.ExecuteNonQuery();
            
            SqliteConnEx.Close();

        }

    }
}
