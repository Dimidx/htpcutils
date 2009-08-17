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

        public static bool SaveFilmToDB(Film Film)
        {
            //StringBuilder _SQLUpdate = new StringBuilder();





            SQLiteConnection SqliteConnEx = new SQLiteConnection(_SqliteConnString);

            SQLiteCommand SqliteComEx = SqliteConnEx.CreateCommand();

            SqliteConnEx.Open();
            SqliteComEx.CommandText = "INSERT OR REPLACE INTO Films (Titre,Note,Path) VALUES (?,?,?);";
            SqliteComEx.Parameters.AddWithValue("Titre", Film.Titre);
            SqliteComEx.Parameters.AddWithValue("Note", Film.Note);
            SqliteComEx.Parameters.AddWithValue("Path", Film.Titre);
            SqliteComEx.ExecuteNonQuery();

            SqliteConnEx.Close();

            //_SQLUpdate.Append(@"INSERT INTO Films (Titre,TitreOriginal,ImdbID,AlloID,Annee,Accroche,Resume,Synopsis,Duree,Note,Votes,MPAA,Certification,Top250,Studio,DateSortie,Vu,Path,PathCover,PathFanart,PathNFO,PathBA) VALUES (" +
            //    "'" + Film.Titre + "'," +
            //    "'" + Film.TitreOriginal + "'," +
            //    "'" + Film.ID + "'," +
            //    "'" + Film.AlloID + "'," +
            //    "'" + Film.Annee + "'," +
            //    "'" + Film.Accroche + "'," +
            //    "'" + Film.Resume + "'," +
            //    "'" + Film.Synopsis + "'," +
            //    "'" + Film.Duree + "'," +
            //    "'" + Film.Note + "'," +
            //    "'" + Film.Votes + "'," +
            //    "'" + Film.MPAA + "'," +
            //    "'" + Film.Certification + "'," +
            //    "'" + Film.Top250 + "'," +
            //    "'" + Film.Studio + "'," +
            //    "'" + Film.DateSortie + "'," +
            //    "'" + Film.Vu + "'" +
            //    //"'" + Film.Path + "'," +
            //    //"'" + Film.PathCover + "'," +
            //    //"'" + Film.PathFanart + "'," +
            //    //"'" + Film.PathNFO + "'," +
            //    //"'" + Film.PathBA + "'," +
            //    ");");
            //ExecuteSQL(_SQLUpdate.ToString());
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
