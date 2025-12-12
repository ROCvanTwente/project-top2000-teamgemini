-- =============================================
-- BESTANDSNAAM: StatistiekenStoredProcedures.sql
-- BESCHRIJVING: 10 stored procedures om de statistieken procedures in te laden.
-- AUTEUR: Team Gemini
-- DATUM: 2025-12-12
-- =============================================

-- 1. DALERS
-- Toont de nummers die het meest zijn gedaald t.o.v. vorig jaar.
GO
CREATE OR ALTER PROCEDURE GetDalers
    @Jaar INT,
    @Aantal INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        t_current.Position AS Positie,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar,
        t_prev.Position AS VorigJaar,
        (t_current.Position - t_prev.Position) AS PlaatsenGedaald
    FROM Top2000Entries t_current
    INNER JOIN Top2000Entries t_prev ON t_current.SongId = t_prev.SongId
    INNER JOIN Songs s ON t_current.SongId = s.SongId
    INNER JOIN Artists a ON s.ArtistId = a.ArtistId
    WHERE t_current.Year = @Jaar AND t_prev.Year = (@Jaar - 1)
      AND t_current.Position > t_prev.Position
    ORDER BY (t_current.Position - t_prev.Position) DESC;
END;
GO

-- 2. STIJGERS
-- Toont de nummers die het meest zijn gestegen t.o.v. vorig jaar.
GO
CREATE OR ALTER PROCEDURE GetStijgers
    @Jaar INT,
    @Aantal INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        t_current.Position AS Positie,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar,
        t_prev.Position AS VorigJaar,
        (t_prev.Position - t_current.Position) AS PlaatsenGestegen
    FROM Top2000Entries t_current
    INNER JOIN Top2000Entries t_prev ON t_current.SongId = t_prev.SongId
    INNER JOIN Songs s ON t_current.SongId = s.SongId
    INNER JOIN Artists a ON s.ArtistId = a.ArtistId
    WHERE t_current.Year = @Jaar AND t_prev.Year = (@Jaar - 1)
      AND t_current.Position < t_prev.Position
    ORDER BY (t_prev.Position - t_current.Position) DESC;
END;
GO

-- 3. ALLE EDITIES
-- Toont nummers die in ELKE editie van de Top 2000 hebben gestaan.
GO
CREATE OR ALTER PROCEDURE GetAlleEdities
    @Aantal INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TotaalAantalJaren INT = (SELECT COUNT(DISTINCT Year) FROM Top2000Entries);

    SELECT TOP (@Aantal)
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar
    FROM Songs s
    INNER JOIN Artists a ON s.ArtistId = a.ArtistId
    INNER JOIN Top2000Entries t ON s.SongId = t.SongId
    GROUP BY s.Titel, a.Name, s.ReleaseYear
    HAVING COUNT(DISTINCT t.Year) = @TotaalAantalJaren
    ORDER BY s.Titel;
END;
GO

-- 4. NIEUWE BINNENKOMERS
-- Nummers die dit jaar in de lijst staan, maar vorig jaar (en nooit eerder) erin stonden.
GO
CREATE OR ALTER PROCEDURE GetNieuweBinnenkomers
    @Jaar INT,
    @Aantal INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        t.Position AS Positie,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar
    FROM Top2000Entries t
    INNER JOIN Songs s ON t.SongId = s.SongId
    INNER JOIN Artists a ON s.ArtistId = a.ArtistId
    WHERE t.Year = @Jaar
      AND NOT EXISTS (SELECT 1 FROM Top2000Entries t_prev WHERE t_prev.SongId = t.SongId AND t_prev.Year = (@Jaar - 1))
      AND NOT EXISTS (SELECT 1 FROM Top2000Entries t_hist WHERE t_hist.SongId = t.SongId AND t_hist.Year < (@Jaar - 1))
    ORDER BY t.Position ASC;
END;
GO

-- 5. VERDWENEN
-- Nummers die vorig jaar in de lijst stonden, maar dit jaar niet meer.
GO
CREATE OR ALTER PROCEDURE GetVerdwenen
    @Jaar INT,
    @Aantal INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        t_prev.Position AS PositieVorigJaar,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar
    FROM Top2000Entries t_prev
    INNER JOIN Songs s ON t_prev.SongId = s.SongId
    INNER JOIN Artists a ON s.ArtistId = a.ArtistId
    WHERE t_prev.Year = (@Jaar - 1)
      AND NOT EXISTS (SELECT 1 FROM Top2000Entries t_curr WHERE t_curr.SongId = t_prev.SongId AND t_curr.Year = @Jaar)
    ORDER BY t_prev.Position ASC;
END;
GO

-- 6. OPNIEUW BINNEN
-- Nummers die dit jaar erin staan, vorig jaar niet, maar daarvoor wel ooit.
GO
CREATE OR ALTER PROCEDURE GetOpnieuwBinnen
    @Jaar INT,
    @Aantal INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        t.Position AS Positie,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar
    FROM Top2000Entries t
    INNER JOIN Songs s ON t.SongId = s.SongId
    INNER JOIN Artists a ON s.ArtistId = a.ArtistId
    WHERE t.Year = @Jaar
      AND NOT EXISTS (SELECT 1 FROM Top2000Entries t_prev WHERE t_prev.SongId = t.SongId AND t_prev.Year = (@Jaar - 1))
      AND EXISTS (SELECT 1 FROM Top2000Entries t_old WHERE t_old.SongId = t.SongId AND t_old.Year < (@Jaar - 1))
    ORDER BY t.Position ASC;
END;
GO

-- 7. STABIEL
-- Nummers die op exact dezelfde positie staan als vorig jaar.
GO
CREATE OR ALTER PROCEDURE GetStabiel
    @Jaar INT,
    @Aantal INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        t_current.Position AS Positie,
        s.Titel,
        a.Name AS Artiest,
        s.ReleaseYear AS Uitgiftejaar
    FROM Top2000Entries t_current
    INNER JOIN Top2000Entries t_prev ON t_current.SongId = t_prev.SongId
    INNER JOIN Songs s ON t_current.SongId = s.SongId
    INNER JOIN Artists a ON s.ArtistId = a.ArtistId
    WHERE t_current.Year = @Jaar AND t_prev.Year = (@Jaar - 1) AND t_current.Position = t_prev.Position
    ORDER BY t_current.Position ASC;
END;
GO

-- 8. AANSLUITENDE POSITIES
-- Artiesten die met meerdere nummers direct achter elkaar staan (bijv. plek 87 en 88).
GO
CREATE OR ALTER PROCEDURE GetAansluitendePosities
    @Jaar INT,
    @Aantal INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DISTINCT TOP (@Aantal)
        t1.Position AS Positie,
        s1.Titel,
        a.Name AS Artiest,
        s1.ReleaseYear AS Uitgiftejaar
    FROM Top2000Entries t1
    INNER JOIN Songs s1 ON t1.SongId = s1.SongId
    INNER JOIN Artists a ON s1.ArtistId = a.ArtistId
    INNER JOIN Top2000Entries t2 ON t1.Year = t2.Year
    INNER JOIN Songs s2 ON t2.SongId = s2.SongId
    WHERE t1.Year = @Jaar
      AND s1.ArtistId = s2.ArtistId
      AND t1.SongId <> t2.SongId
      AND (t1.Position = t2.Position - 1 OR t1.Position = t2.Position + 1)
    ORDER BY t1.Position ASC;
END;
GO

-- 9. EENMALIGE NOTERINGEN
-- Nummers die in de hele geschiedenis maar 1 keer in de lijst hebben gestaan.
GO
CREATE OR ALTER PROCEDURE GetEenmaligeNoteringen
    @Aantal INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        a.Name AS Artiest,
        s.Titel,
        s.ReleaseYear AS Uitgiftejaar,
        MAX(t.Position) AS Positie,
        MAX(t.Year) AS Top2000Jaar
    FROM Songs s
    INNER JOIN Artists a ON s.ArtistId = a.ArtistId
    INNER JOIN Top2000Entries t ON s.SongId = t.SongId
    GROUP BY s.Titel, a.Name, s.ReleaseYear
    HAVING COUNT(t.Year) = 1
    ORDER BY a.Name, s.Titel;
END;
GO

-- 10. TOP ARTIESTEN PER JAAR
-- De artiesten met de meeste noteringen in een specifiek jaar.
GO
CREATE OR ALTER PROCEDURE GetTopArtiestenPerJaar
    @Jaar INT,
    @Aantal INT = 3
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Aantal)
        a.Name AS Naam,
        COUNT(t.SongId) AS AantalLiedjes,
        AVG(t.Position) AS GemiddeldePositie,
        MIN(t.Position) AS HoogsteNotering
    FROM Top2000Entries t
    INNER JOIN Songs s ON t.SongId = s.SongId
    INNER JOIN Artists a ON s.ArtistId = a.ArtistId
    WHERE t.Year = @Jaar
    GROUP BY a.Name
    ORDER BY COUNT(t.SongId) DESC, MIN(t.Position) ASC;
END;
GO