using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateJwtProject.Migrations
{
    public partial class AddTopXStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. DALERS
            migrationBuilder.Sql(@"
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
            ");

            // 2. STIJGERS
            migrationBuilder.Sql(@"
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
            ");

            // 3. ALLE EDITIES
            migrationBuilder.Sql(@"
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
            ");

            // 4. NIEUWE BINNENKOMERS
            migrationBuilder.Sql(@"
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
            ");

            // 5. VERDWENEN
            migrationBuilder.Sql(@"
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
            ");

            // 6. OPNIEUW BINNEN
            migrationBuilder.Sql(@"
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
            ");

            // 7. STABIEL
            migrationBuilder.Sql(@"
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
            ");

            // 8. AANSLUITENDE POSITIES
            migrationBuilder.Sql(@"
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
            ");

            // 9. EENMALIGE NOTERINGEN
            migrationBuilder.Sql(@"
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
            ");

            // 10. TOP ARTIESTEN PER JAAR
            migrationBuilder.Sql(@"
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
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetDalers");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetStijgers");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAlleEdities");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetNieuweBinnenkomers");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetVerdwenen");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetOpnieuwBinnen");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetStabiel");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAansluitendePosities");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetEenmaligeNoteringen");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTopArtiestenPerJaar");
        }
    }
}