using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TMS_Project.Models;

namespace TMS_Project.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            TmsDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();
            EnsureDemandeLivraisonSchema(context);
            EnsureIdentitySchema(context);

            await SeedRolesAndUsersAsync(userManager, roleManager);
            SeedBusinessData(context);
        }

        private static async Task SeedRolesAndUsersAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in new[] { ApplicationRoles.Admin, ApplicationRoles.User })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            await EnsureUserAsync(userManager, "admin@translog.local", "Admin@12345", ApplicationRoles.Admin);
            await EnsureUserAsync(userManager, "user@translog.local", "User@12345", ApplicationRoles.User);
        }

        private static async Task EnsureUserAsync(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(" | ", result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }

        private static void SeedBusinessData(TmsDbContext context)
        {
            if (context.Transporteurs.Any())
            {
                return;
            }

            var transporteurs = new List<Transporteur>
            {
                new Transporteur { Nom = "Express Transport", Adresse = "Casablanca", Telephone = "0600000001", Email = "contact@express.ma", DateCreation = DateTime.Now },
                new Transporteur { Nom = "Logistique Plus", Adresse = "Rabat", Telephone = "0600000002", Email = "info@logistique.ma", DateCreation = DateTime.Now },
                new Transporteur { Nom = "Transco Solutions", Adresse = "Tanger", Telephone = "0600000003", Email = "contact@transco.ma", DateCreation = DateTime.Now }
            };
            context.Transporteurs.AddRange(transporteurs);
            context.SaveChanges();

            var camions = new List<Camion>
            {
                new Camion { TransporteurId = 1, Marque = "Volvo", Modele = "FH16", Immatriculation = "123ABC456", CapaciteKg = 25000, DateAcquisition = new DateTime(2022, 1, 15), EstActif = true },
                new Camion { TransporteurId = 2, Marque = "Renault", Modele = "T480", Immatriculation = "789GHI012", CapaciteKg = 18000, DateAcquisition = new DateTime(2023, 3, 10), EstActif = true }
            };
            context.Camions.AddRange(camions);
            context.SaveChanges();

            var chauffeurs = new List<Chauffeur>
            {
                new Chauffeur { TransporteurId = 1, Nom = "Zaari", Prenom = "Ali", Telephone = "0611223344", Email = "ali@express.ma", NumeroPermis = "MA123456", DateEmbauche = new DateTime(2019, 9, 1), EstActif = true },
                new Chauffeur { TransporteurId = 2, Nom = "Idrissi", Prenom = "Aziz", Telephone = "0699887766", Email = "aziz@logistique.ma", NumeroPermis = "MA456789", DateEmbauche = new DateTime(2021, 1, 10), EstActif = true }
            };
            context.Chauffeurs.AddRange(chauffeurs);
            context.SaveChanges();

            var clients = new List<Client>
            {
                new Client { Nom = "Supermarche Central", Adresse = "Casablanca", Telephone = "0601010101", Email = "client1@demo.ma", DateCreation = DateTime.Now },
                new Client { Nom = "Magasin Local", Adresse = "Rabat", Telephone = "0602020202", Email = "client2@demo.ma", DateCreation = DateTime.Now }
            };
            context.Clients.AddRange(clients);
            context.SaveChanges();

            var tournees = new List<Tournee>
            {
                new Tournee { TransporteurId = 1, CamionId = 1, ChauffeurId = 1, DateTournee = DateTime.Today, StatutTournee = "Planifiee", CoutTotal = 0m },
                new Tournee { TransporteurId = 2, CamionId = 2, ChauffeurId = 2, DateTournee = DateTime.Today.AddDays(1), StatutTournee = "En cours", CoutTotal = 0m }
            };
            context.Tournees.AddRange(tournees);
            context.SaveChanges();
        }

        private static void EnsureDemandeLivraisonSchema(TmsDbContext context)
        {
            context.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[DemandesLivraison]', N'U') IS NULL
BEGIN
    CREATE TABLE [DemandesLivraison](
        [DemandeLivraisonId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [ClientId] INT NULL,
        [TypeService] NVARCHAR(100) NOT NULL DEFAULT 'Standard',
        [DescriptionMarchandise] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [Adresse] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [Quantite] INT NOT NULL DEFAULT 1,
        [PoidsKg] INT NOT NULL DEFAULT 0,
        [DateSouhaitee] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [Statut] NVARCHAR(100) NOT NULL DEFAULT 'En attente',
        [MontantEstime] DECIMAL(10,2) NOT NULL DEFAULT 0,
        [CamionAffecteId] INT NULL,
        [ChauffeurAffecteId] INT NULL,
        [NumeroRecu] NVARCHAR(100) NOT NULL DEFAULT '',
        [DateValidation] DATETIME2 NULL,
        [CommentaireValidation] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE()
    )
END");

            context.Database.ExecuteSqlRaw(@"
IF COL_LENGTH('DemandesLivraison', 'TypeService') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [TypeService] NVARCHAR(100) NOT NULL DEFAULT 'Standard';
IF COL_LENGTH('DemandesLivraison', 'DescriptionMarchandise') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [DescriptionMarchandise] NVARCHAR(MAX) NOT NULL DEFAULT '';
IF COL_LENGTH('DemandesLivraison', 'Quantite') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [Quantite] INT NOT NULL DEFAULT 1;
IF COL_LENGTH('DemandesLivraison', 'NumeroRecu') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [NumeroRecu] NVARCHAR(100) NOT NULL DEFAULT '';
IF COL_LENGTH('DemandesLivraison', 'DateValidation') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [DateValidation] DATETIME2 NULL;
IF COL_LENGTH('DemandesLivraison', 'CommentaireValidation') IS NULL
    ALTER TABLE [DemandesLivraison] ADD [CommentaireValidation] NVARCHAR(MAX) NOT NULL DEFAULT '';
");
        }

        private static void EnsureIdentitySchema(TmsDbContext context)
        {
            context.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[AspNetRoles]', N'U') IS NULL
BEGIN
    CREATE TABLE [AspNetRoles](
        [Id] NVARCHAR(450) NOT NULL CONSTRAINT [PK_AspNetRoles] PRIMARY KEY,
        [Name] NVARCHAR(256) NULL,
        [NormalizedName] NVARCHAR(256) NULL,
        [ConcurrencyStamp] NVARCHAR(MAX) NULL
    );
    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles]([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
END

IF OBJECT_ID(N'[AspNetUsers]', N'U') IS NULL
BEGIN
    CREATE TABLE [AspNetUsers](
        [Id] NVARCHAR(450) NOT NULL CONSTRAINT [PK_AspNetUsers] PRIMARY KEY,
        [UserName] NVARCHAR(256) NULL,
        [NormalizedUserName] NVARCHAR(256) NULL,
        [Email] NVARCHAR(256) NULL,
        [NormalizedEmail] NVARCHAR(256) NULL,
        [EmailConfirmed] BIT NOT NULL,
        [PasswordHash] NVARCHAR(MAX) NULL,
        [SecurityStamp] NVARCHAR(MAX) NULL,
        [ConcurrencyStamp] NVARCHAR(MAX) NULL,
        [PhoneNumber] NVARCHAR(MAX) NULL,
        [PhoneNumberConfirmed] BIT NOT NULL,
        [TwoFactorEnabled] BIT NOT NULL,
        [LockoutEnd] DATETIMEOFFSET NULL,
        [LockoutEnabled] BIT NOT NULL,
        [AccessFailedCount] INT NOT NULL
    );
    CREATE INDEX [EmailIndex] ON [AspNetUsers]([NormalizedEmail]);
    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers]([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
END

IF OBJECT_ID(N'[AspNetRoleClaims]', N'U') IS NULL
BEGIN
    CREATE TABLE [AspNetRoleClaims](
        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY,
        [RoleId] NVARCHAR(450) NOT NULL,
        [ClaimType] NVARCHAR(MAX) NULL,
        [ClaimValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [AspNetRoles]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims]([RoleId]);
END

IF OBJECT_ID(N'[AspNetUserClaims]', N'U') IS NULL
BEGIN
    CREATE TABLE [AspNetUserClaims](
        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY,
        [UserId] NVARCHAR(450) NOT NULL,
        [ClaimType] NVARCHAR(MAX) NULL,
        [ClaimValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims]([UserId]);
END

IF OBJECT_ID(N'[AspNetUserLogins]', N'U') IS NULL
BEGIN
    CREATE TABLE [AspNetUserLogins](
        [LoginProvider] NVARCHAR(450) NOT NULL,
        [ProviderKey] NVARCHAR(450) NOT NULL,
        [ProviderDisplayName] NVARCHAR(MAX) NULL,
        [UserId] NVARCHAR(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins]([UserId]);
END

IF OBJECT_ID(N'[AspNetUserRoles]', N'U') IS NULL
BEGIN
    CREATE TABLE [AspNetUserRoles](
        [UserId] NVARCHAR(450) NOT NULL,
        [RoleId] NVARCHAR(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [AspNetRoles]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles]([RoleId]);
END

IF OBJECT_ID(N'[AspNetUserTokens]', N'U') IS NULL
BEGIN
    CREATE TABLE [AspNetUserTokens](
        [UserId] NVARCHAR(450) NOT NULL,
        [LoginProvider] NVARCHAR(450) NOT NULL,
        [Name] NVARCHAR(450) NOT NULL,
        [Value] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE
    );
END
");
        }
    }
}
