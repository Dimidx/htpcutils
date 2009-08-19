CREATE TABLE "Acteurs" ("IDActeur" INT PRIMARY KEY  NOT NULL , "Nom" TEXT);
CREATE TABLE "ActeursFilms" ("IDFilm" INTEGER, "IDActeur" INTEGER);
CREATE TABLE Films (
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
                PathBA TEXT);
CREATE TABLE sqlite_sequence(name,seq);
CREATE TRIGGER DeleteFilm
AFTER DELETE ON Films
FOR EACH ROW BEGIN
    DELETE FROM ActeursFilms WHERE ActeursFilms.IDFilm = OLD.IDFilm;
END;
CREATE TRIGGER UpdateFilm
AFTER UPDATE ON Films
FOR EACH ROW BEGIN
    UPDATE ActeursFilms SET IDFilm = NEW.IDFilm WHERE ActeursFilms.IDFilm = OLD.IDFilm;
END;
CREATE UNIQUE INDEX UniquePath ON Films (Path);
