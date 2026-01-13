# Top 2000 API Project - Team Gemini

Een ASP.NET Core Web API voor het beheren en raadplegen van de Top 2000, de jaarlijkse lijst van beste nummers aller tijden zoals uitgezonden door NPO Radio 2.

**🎯 Gebouwd met .NET 10.0**

## 📋 Overzicht

Dit project biedt een complete RESTful API voor de Top 2000, inclusief:
- 🎵 Beheer van nummers en artiesten
- 📊 Statistieken en overzichten per jaar
- 👤 Gebruikersauthenticatie met JWT tokens
- 🔐 Role-based authorization (User & Admin)
- 📝 Persoonlijke lijsten voor gebruikers
- 🎨 Refresh tokens voor veilige sessies

## ✨ Features

### Core Functionaliteit
- ✅ Complete CRUD operaties voor nummers en artiesten
- ✅ Top 2000 entries per jaar met posities
- ✅ Gedetailleerde nummerinformatie (release jaar, lyrics, YouTube links)
- ✅ Statistieken en jaar overzichten
- ✅ Zoeken en filteren van nummers en artiesten

### Authenticatie & Autorisatie
- ✅ ASP.NET Core Identity met JWT Bearer tokens
- ✅ Refresh tokens voor automatische token renewal
- ✅ Role-based authorization (User & Admin rollen)
- ✅ Beveiligde endpoints met `[Authorize]` attribuut
- ✅ Token revocation & logout functionaliteit

### Database
- ✅ SQL Server database met Entity Framework Core
- ✅ Migraties voor database versie beheer
- ✅ Identity tabellen voor gebruikersbeheer
- ✅ Geoptimaliseerde relaties tussen Songs, Artists en Top2000Entries

## 🏗️ Project Structuur

```
project-top2000-teamgemini/
├── Project-Top2000-API/          # Hoofdproject (API)
│   ├── Controllers/              # API endpoints
│   │   ├── AuthController.cs           # Login/Register/Refresh
│   │   ├── AdminController.cs          # Admin-only endpoints
│   │   ├── NummersOverzichtController.cs    # Nummers overzicht
│   │   ├── ArtiestenOverzichtController.cs  # Artiesten overzicht
│   │   ├── JaarOverzichtController.cs       # Jaar overzichten
│   │   ├── SongDetailsController.cs         # Nummer details
│   │   ├── StatistiekenOverzichtController.cs  # Statistieken
│   │   ├── MijnLijstenOverzichtController.cs   # Persoonlijke lijsten
│   │   └── ContactController.cs        # Contact functionaliteit
│   ├── Models/                   # Data models
│   │   ├── Songs.cs              # Nummer model
│   │   ├── Artist.cs             # Artiest model
│   │   ├── Top2000Entry.cs       # Top 2000 entry (jaar + positie)
│   │   ├── ApplicationUser.cs    # Custom Identity User
│   │   └── DTOs/                 # Data Transfer Objects
│   ├── Services/                 # Business logic
│   │   ├── JwtService.cs         # JWT token generatie
│   │   ├── RefreshTokenService.cs # Refresh token beheer
│   │   └── RoleInitializer.cs    # Initialiseer rollen
│   ├── Data/                     # Database context
│   │   └── AppDbContext.cs       # EF Core DbContext
│   ├── Docs/                     # Documentatie
│   │   ├── README.md             # API documentatie
│   │   ├── REFRESH_TOKENS.md     # Refresh token guide
│   │   └── ADMIN_SETUP.md        # Admin setup guide
│   └── Program.cs                # App configuratie
├── Top2000.Testing/              # Test project
│   └── UnitTest1.cs              # Unit tests
└── README.md                     # Dit bestand

```

## 🚀 Quick Start

### Vereisten

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) of hoger
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, of Developer Edition)
- Een IDE zoals [Visual Studio 2022](https://visualstudio.microsoft.com/) of [Visual Studio Code](https://code.visualstudio.com/)

### Installatie

1. **Clone de repository**
   ```bash
   git clone https://github.com/ROCvanTwente/project-top2000-teamgemini.git
   cd project-top2000-teamgemini
   ```

2. **Configureer de database connection string**
   
   Pas `appsettings.json` aan in `Project-Top2000-API/`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=Top2000DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
     }
   }
   ```

3. **Run database migraties**
   ```bash
   cd Project-Top2000-API
   dotnet ef database update
   ```

4. **Start de applicatie**
   ```bash
   dotnet run
   ```

   De API is nu beschikbaar op: `https://localhost:7003`

### Database Seeding (Optioneel)

Om de database te vullen met Top 2000 data, kun je:
- Import scripts gebruiken (indien beschikbaar in `/Data` of `/Scripts`)
- Handmatig data toevoegen via de API endpoints
- Een externe data source gebruiken

## 🔑 Authenticatie

### Registreren

```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "gebruiker@example.com",
  "password": "Wachtwoord123!",
  "confirmPassword": "Wachtwoord123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "kYj3L8Xm9QwErTy...",
  "email": "gebruiker@example.com",
  "roles": ["User"],
  "expiresAt": "2024-01-17T13:37:08Z"
}
```

### Inloggen

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "gebruiker@example.com",
  "password": "Wachtwoord123!"
}
```

### Beveiligde Endpoints Gebruiken

Voeg de JWT token toe aan de Authorization header:

```http
GET /api/nummersoverzicht
Authorization: Bearer {your-jwt-token}
```

## 📡 API Endpoints

### Publieke Endpoints
- `POST /api/auth/register` - Nieuwe gebruiker registreren
- `POST /api/auth/login` - Inloggen

### Authenticatie Vereist
- `GET /api/nummersoverzicht` - Overzicht van alle nummers
- `GET /api/artiestenoverz icht` - Overzicht van alle artiesten
- `GET /api/jaaroverzicht` - Top 2000 per jaar
- `GET /api/songdetails/{id}` - Details van een specifiek nummer
- `GET /api/statistiekenoverzicht` - Statistieken
- `GET /api/mijnlijstenoverzicht` - Persoonlijke lijsten (User rol)

### Admin Endpoints
- `GET /api/admin/users` - Alle gebruikers ophalen
- `POST /api/admin/assign-role` - Rol toewijzen aan gebruiker
- `POST /api/admin/remove-role` - Rol verwijderen van gebruiker

Voor gedetailleerde API documentatie, zie [Project-Top2000-API/Docs/README.md](Project-Top2000-API/Docs/README.md)

## 🔐 Refresh Tokens

De API ondersteunt refresh tokens voor langdurige sessies zonder herhaaldelijk inloggen:

- **Access Token**: 60 minuten (kort voor beveiliging)
- **Refresh Token**: 7 dagen (lang voor gebruikerservaring)

**Endpoints:**
- `POST /api/auth/refresh-token` - Nieuwe access token verkrijgen
- `POST /api/auth/revoke-token` - Token intrekken
- `POST /api/auth/logout-all` - Uitloggen van alle apparaten

Zie [Project-Top2000-API/Docs/REFRESH_TOKENS.md](Project-Top2000-API/Docs/REFRESH_TOKENS.md) voor implementatie voorbeelden.

## 👨‍💼 Admin Setup

Om een admin gebruiker aan te maken:

**Optie 1: Via Database**
```sql
-- Vind de User ID en Admin Role ID
SELECT u.Id as UserId, r.Id as RoleId
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'admin@example.com' AND r.Name = 'Admin'

-- Voeg de koppeling toe
INSERT INTO AspNetUserRoles (UserId, RoleId) 
VALUES ('user-id-hier', 'admin-role-id-hier')
```

**Optie 2: Via Code** - Zie [Project-Top2000-API/Docs/ADMIN_SETUP.md](Project-Top2000-API/Docs/ADMIN_SETUP.md)

## 🧪 Testing

Run de unit tests:

```bash
cd Top2000.Testing
dotnet test
```

## 🛠️ Development

### Database Migraties

```bash
# Nieuwe migratie aanmaken
dotnet ef migrations add MigrationName

# Database updaten
dotnet ef database update

# Migratie verwijderen (laatste)
dotnet ef migrations remove
```

### CORS Configuratie

De API ondersteunt CORS voor frontend integratie. Standaard toegestane origins:
- `http://localhost:1234`
- `http://localhost:3000` (React)
- `http://localhost:5173` (Vite)
- `http://localhost:4200` (Angular)

Pas `CorsSettings` aan in `appsettings.json` voor productie URLs.

### Password Vereisten

- Minimaal 6 karakters
- Minimaal 1 cijfer
- Minimaal 1 hoofdletter
- Minimaal 1 kleine letter

## 📦 Dependencies

- **Microsoft.AspNetCore.Identity.EntityFrameworkCore** (10.0.0)
- **Microsoft.EntityFrameworkCore.SqlServer** (10.0.0)
- **Microsoft.EntityFrameworkCore.Tools** (10.0.0)
- **Microsoft.AspNetCore.Authentication.JwtBearer** (10.0.0)
- **Microsoft.AspNetCore.OpenApi** (10.0.0)

## 🎯 Over de Top 2000

De Top 2000 is een jaarlijkse lijst van de 2000 beste nummers aller tijden, zoals gekozen door luisteraars van NPO Radio 2 in Nederland. Sinds 1999 wordt deze lijst elk jaar tussen Kerst en Oud & Nieuw uitgezonden.

Dit project biedt een API om:
- Historische Top 2000 data te raadplegen
- Posities van nummers door de jaren heen te volgen
- Statistieken en trends te analyseren
- Persoonlijke lijsten te maken

## 👥 Team Gemini

Dit project is ontwikkeld door Team Gemini als onderdeel van een school project voor ROC van Twente.

## 📄 Licentie

Dit project is ontwikkeld voor educatieve doeleinden.

## 🤝 Contributing

Dit is een school project. Neem contact op met de projecteigenaren voor informatie over bijdragen.

## 📞 Contact

Voor vragen of problemen, gebruik de contact functionaliteit in de API of neem contact op via de project repository.

---

**Happy coding! 🎵✨**
