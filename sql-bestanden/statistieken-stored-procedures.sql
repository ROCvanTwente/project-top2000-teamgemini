   STAP 1: STANDAARD AANTALLEN VERHOGEN (DE "UNCAPPED" VERSIES)

-- 1. DALERS
GO
CREATE OR ALTER PROCEDURE GetDalers
    @Jaar INT,
    @Aantal INT = 2000
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        s.SongId,
        a.ArtistId,
        t_current.Position AS Positie,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar,
        t_prev.Position AS VorigJaar,
        (t_current.Position - t_prev.Position) AS PlaatsenGedaald
    FROM Top2000Entry t_current
    INNER JOIN Top2000Entry t_prev ON t_current.SongId = t_prev.SongId
    INNER JOIN Songs s ON t_current.SongId = s.SongId
    INNER JOIN Artist a ON s.ArtistId = a.ArtistId
    WHERE t_current.Year = @Jaar AND t_prev.Year = (@Jaar - 1)
      AND t_current.Position > t_prev.Position
    ORDER BY (t_current.Position - t_prev.Position) DESC;
END;
GO

-- 2. STIJGERS
GO
CREATE OR ALTER PROCEDURE GetStijgers
    @Jaar INT,
    @Aantal INT = 2000
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        s.SongId,
        a.ArtistId,
        t_current.Position AS Positie,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar,
        t_prev.Position AS VorigJaar,
        (t_prev.Position - t_current.Position) AS PlaatsenGestegen
    FROM Top2000Entry t_current
    INNER JOIN Top2000Entry t_prev ON t_current.SongId = t_prev.SongId
    INNER JOIN Songs s ON t_current.SongId = s.SongId
    INNER JOIN Artist a ON s.ArtistId = a.ArtistId
    WHERE t_current.Year = @Jaar AND t_prev.Year = (@Jaar - 1)
      AND t_current.Position < t_prev.Position
    ORDER BY (t_prev.Position - t_current.Position) DESC;
END;
GO

-- 3. EVERGREENS
GO
CREATE OR ALTER PROCEDURE GetEvergreens
    @Aantal INT = 2000
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TotaalAantalJaren INT = (SELECT COUNT(DISTINCT Year) FROM Top2000Entry);

    SELECT TOP (@Aantal)
        s.SongId,
        a.ArtistId,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar
    FROM Songs s
    INNER JOIN Artist a ON s.ArtistId = a.ArtistId
    INNER JOIN Top2000Entry t ON s.SongId = t.SongId
    GROUP BY s.SongId, a.ArtistId, s.Titel, a.Name, s.ReleaseYear
    HAVING COUNT(DISTINCT t.Year) = @TotaalAantalJaren
    ORDER BY s.Titel;
END;
GO

-- 4. NIEUWE BINNENKOMERS
GO
CREATE OR ALTER PROCEDURE GetNieuweBinnenkomers
    @Jaar INT,
    @Aantal INT = 2000
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        s.SongId,
        a.ArtistId,
        t.Position AS Positie,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar
    FROM Top2000Entry t
    INNER JOIN Songs s ON t.SongId = s.SongId
    INNER JOIN Artist a ON s.ArtistId = a.ArtistId
    WHERE t.Year = @Jaar
      AND NOT EXISTS (
          SELECT 1 FROM Top2000Entry t_prev 
          WHERE t_prev.SongId = t.SongId AND t_prev.Year = (@Jaar - 1)
      )
    ORDER BY t.Position ASC;
END;
GO

-- 5. VERDWENEN UIT DE LIJST
GO
CREATE OR ALTER PROCEDURE GetVerdwenen
    @Jaar INT,
    @Aantal INT = 2000
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        s.SongId,
        a.ArtistId,
        t_prev.Position AS PositieVorigJaar,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar
    FROM Top2000Entry t_prev
    INNER JOIN Songs s ON t_prev.SongId = s.SongId
    INNER JOIN Artist a ON s.ArtistId = a.ArtistId
    WHERE t_prev.Year = (@Jaar - 1)
      AND NOT EXISTS (
          SELECT 1 FROM Top2000Entry t_cur 
          WHERE t_cur.SongId = t_prev.SongId AND t_cur.Year = @Jaar
      )
    ORDER BY t_prev.Position ASC;
END;
GO

-- 9. EENMALIGE NOTERINGEN (aantal 5000)
GO
CREATE OR ALTER PROCEDURE GetEenmaligeNoteringen
    @Aantal INT = 5000
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        s.SongId,
        a.ArtistId,
        a.Name AS Artiest,
        s.Titel,
        s.ReleaseYear AS Uitgiftejaar,
        MAX(t.Position) AS Positie,
        MAX(t.Year) AS Top2000Jaar
    FROM Songs s
    INNER JOIN Artist a ON s.ArtistId = a.ArtistId
    INNER JOIN Top2000Entry t ON s.SongId = t.SongId
    GROUP BY s.SongId, a.ArtistId, s.Titel, a.Name, s.ReleaseYear
    HAVING COUNT(t.Year) = 1
    ORDER BY MAX(t.Year) DESC, MAX(t.Position) ASC;
END;
GO

-- 10. TOP ARTIESTEN PER JAAR
GO
CREATE OR ALTER PROCEDURE GetTopArtiestenPerJaar
    @Jaar INT,
    @Aantal INT = 2000
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        a.ArtistId,
        a.Name AS Naam,
        COUNT(t.SongId) AS AantalLiedjes,
        AVG(CAST(t.Position AS DECIMAL(10,2))) AS GemiddeldePositie,
        MIN(t.Position) AS HoogsteNotering
    FROM Top2000Entry t
    INNER JOIN Songs s ON t.SongId = s.SongId
    INNER JOIN Artist a ON s.ArtistId = a.ArtistId
    WHERE t.Year = @Jaar
    GROUP BY a.ArtistId, a.Name
    ORDER BY COUNT(t.SongId) DESC, MIN(t.Position) ASC;
END;
GO