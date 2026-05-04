-- ========================================
-- TMS Database Creation Script
-- SQL Server
-- ========================================

-- Create Database
CREATE DATABASE TMSDatabase;
GO

USE TMSDatabase;
GO

-- ========================================
-- 1. Transporteur Table
-- ========================================
CREATE TABLE Transporteur (
    TransporteurId INT PRIMARY KEY IDENTITY(1,1),
    Nom NVARCHAR(255) NOT NULL,
    Adresse NVARCHAR(MAX),
    Telephone NVARCHAR(20),
    Email NVARCHAR(255),
    DateCreation DATETIME DEFAULT GETDATE()
);

-- ========================================
-- 2. Camion Table
-- ========================================
CREATE TABLE Camion (
    CamionId INT PRIMARY KEY IDENTITY(1,1),
    TransporteurId INT NOT NULL,
    Marque NVARCHAR(100),
    Modele NVARCHAR(100),
    Immatriculation NVARCHAR(20) NOT NULL UNIQUE,
    CapaciteKg INT,
    DateAcquisition DATETIME,
    EstActif BIT DEFAULT 1,
    FOREIGN KEY (TransporteurId) REFERENCES Transporteur(TransporteurId) ON DELETE CASCADE
);

-- ========================================
-- 3. Chauffeur Table
-- ========================================
CREATE TABLE Chauffeur (
    ChauffeurId INT PRIMARY KEY IDENTITY(1,1),
    TransporteurId INT NOT NULL,
    Nom NVARCHAR(100) NOT NULL,
    Prenom NVARCHAR(100),
    Telephone NVARCHAR(20),
    Email NVARCHAR(255),
    NumeroPermis NVARCHAR(50),
    DateEmbauche DATETIME,
    EstActif BIT DEFAULT 1,
    FOREIGN KEY (TransporteurId) REFERENCES Transporteur(TransporteurId) ON DELETE CASCADE
);

-- ========================================
-- 4. Client Table
-- ========================================
CREATE TABLE Client (
    ClientId INT PRIMARY KEY IDENTITY(1,1),
    Nom NVARCHAR(255) NOT NULL,
    Adresse NVARCHAR(MAX),
    Telephone NVARCHAR(20),
    Email NVARCHAR(255),
    DateCreation DATETIME DEFAULT GETDATE()
);

-- ========================================
-- 5. Tournee Table
-- ========================================
CREATE TABLE Tournee (
    TourneeId INT PRIMARY KEY IDENTITY(1,1),
    TransporteurId INT NOT NULL,
    CamionId INT NOT NULL,
    ChauffeurId INT NOT NULL,
    DateTournee DATETIME,
    StatutTournee NVARCHAR(50) DEFAULT 'PlanifiÃ©e',
    CoutTotal DECIMAL(10, 2) DEFAULT 0,
    FOREIGN KEY (TransporteurId) REFERENCES Transporteur(TransporteurId),
    FOREIGN KEY (CamionId) REFERENCES Camion(CamionId),
    FOREIGN KEY (ChauffeurId) REFERENCES Chauffeur(ChauffeurId)
);

-- ========================================
-- 6. Livraison Table
-- ========================================
CREATE TABLE Livraison (
    LivraisonId INT PRIMARY KEY IDENTITY(1,1),
    TourneeId INT NOT NULL,
    ClientId INT NOT NULL,
    Adresse NVARCHAR(MAX),
    PoidsKg INT,
    DateLivraison DATETIME,
    StatutLivraison NVARCHAR(50) DEFAULT 'En attente',
    DateRealisation DATETIME NULL,
    FOREIGN KEY (TourneeId) REFERENCES Tournee(TourneeId) ON DELETE CASCADE,
    FOREIGN KEY (ClientId) REFERENCES Client(ClientId)
);

-- ========================================
-- 7. Cout Table
-- ========================================
CREATE TABLE Cout (
    CoutId INT PRIMARY KEY IDENTITY(1,1),
    TourneeId INT NOT NULL,
    TypeCout NVARCHAR(100),
    Montant DECIMAL(10, 2),
    DateCout DATETIME DEFAULT GETDATE(),
    Description NVARCHAR(MAX),
    FOREIGN KEY (TourneeId) REFERENCES Tournee(TourneeId) ON DELETE CASCADE
);

-- ========================================
-- Create Indexes for Performance
-- ========================================
CREATE INDEX idx_Camion_TransporteurId ON Camion(TransporteurId);
CREATE INDEX idx_Chauffeur_TransporteurId ON Chauffeur(TransporteurId);
CREATE INDEX idx_Tournee_TransporteurId ON Tournee(TransporteurId);
CREATE INDEX idx_Tournee_CamionId ON Tournee(CamionId);
CREATE INDEX idx_Tournee_ChauffeurId ON Tournee(ChauffeurId);
CREATE INDEX idx_Livraison_TourneeId ON Livraison(TourneeId);
CREATE INDEX idx_Livraison_ClientId ON Livraison(ClientId);
CREATE INDEX idx_Cout_TourneeId ON Cout(TourneeId);

-- ========================================
-- Sample Data for Testing
-- ========================================

-- Insert Transporteur
INSERT INTO Transporteur (Nom, Adresse, Telephone, Email) VALUES
('Express Transport', '123 Rue de la Paix, Paris', '01-23-45-67-89', 'contact@expresstransport.fr'),
('Logistique Plus', '456 Avenue des Routes, Lyon', '04-56-78-90-12', 'info@logistiqueplus.fr'),
('Transco Solutions', '789 Boulevard Commerce, Marseille', '04-91-12-34-56', 'contact@transco.fr');

-- Insert Camion
INSERT INTO Camion (TransporteurId, Marque, Modele, Immatriculation, CapaciteKg, DateAcquisition, EstActif) VALUES
(1, 'Volvo', 'FH16', 'AB-123-CD', 25000, '2022-01-15', 1),
(1, 'Mercedes', 'Actros', 'EF-456-GH', 20000, '2021-06-20', 1),
(2, 'Renault', 'T480', 'IJ-789-KL', 18000, '2023-03-10', 1),
(3, 'DAF', 'XF', 'MN-012-OP', 24000, '2020-11-05', 1);

-- Insert Chauffeur
INSERT INTO Chauffeur (TransporteurId, Nom, Prenom, Telephone, Email, NumeroPermis, DateEmbauche, EstActif) VALUES
(1, 'Dupont', 'Jean', '06-11-22-33-44', 'jean.dupont@expresstransport.fr', 'FR12345678', '2019-09-01', 1),
(1, 'Martin', 'Pierre', '06-55-66-77-88', 'pierre.martin@expresstransport.fr', 'FR87654321', '2020-03-15', 1),
(2, 'Bernard', 'Michel', '06-99-88-77-66', 'michel.bernard@logistiqueplus.fr', 'FR11223344', '2021-01-10', 1),
(3, 'Moreau', 'Luc', '06-44-55-66-77', 'luc.moreau@transco.fr', 'FR55667788', '2018-07-20', 1);

-- Insert Client
INSERT INTO Client (Nom, Adresse, Telephone, Email) VALUES
('SupermarchÃ© Central', '100 Rue Commerce, Paris', '01-45-67-89-01', 'delivery@supermarche-central.fr'),
('Petite Ã‰picerie Local', '50 Rue Principale, Lyon', '04-12-34-56-78', 'contact@epicerie-local.fr'),
('Hyper Magasin', '200 Avenue CommerÃ§ant, Marseille', '04-98-76-54-32', 'logistique@hypermagasin.fr'),
('Magasin SpÃ©cialisÃ©', '75 Boulevard Niche, Toulouse', '05-61-22-33-44', 'achat@specialise.fr');

-- Insert Tournee
INSERT INTO Tournee (TransporteurId, CamionId, ChauffeurId, DateTournee, StatutTournee, CoutTotal) VALUES
(1, 1, 1, '2024-01-15 08:00:00', 'TerminÃ©e', 450.50),
(1, 2, 2, '2024-01-16 08:30:00', 'En cours', 350.00),
(2, 3, 3, '2024-01-17 07:00:00', 'PlanifiÃ©e', 0),
(3, 4, 4, '2024-01-18 09:00:00', 'TerminÃ©e', 520.75);

-- Insert Livraison
INSERT INTO Livraison (TourneeId, ClientId, Adresse, PoidsKg, DateLivraison, StatutLivraison, DateRealisation) VALUES
(1, 1, '100 Rue Commerce, Paris', 5000, '2024-01-15 10:30:00', 'LivrÃ©e', '2024-01-15 10:35:00'),
(1, 2, '50 Rue Principale, Lyon', 3000, '2024-01-15 16:00:00', 'LivrÃ©e', '2024-01-15 16:05:00'),
(2, 3, '200 Avenue CommerÃ§ant, Marseille', 4500, '2024-01-16 14:00:00', 'En attente', NULL),
(3, 4, '75 Boulevard Niche, Toulouse', 2000, '2024-01-17 11:30:00', 'En attente', NULL),
(4, 1, '100 Rue Commerce, Paris', 6000, '2024-01-18 15:00:00', 'LivrÃ©e', '2024-01-18 15:20:00');

-- Insert Cout
INSERT INTO Cout (TourneeId, TypeCout, Montant, Description) VALUES
(1, 'Carburant', 250.00, 'Essence pour trajet Paris-Lyon'),
(1, 'PÃ©age', 150.00, 'PÃ©age autoroute A6'),
(1, 'Maintenance', 50.50, 'Remplissage AdBlue'),
(2, 'Carburant', 200.00, 'Diesel pour trajet Lyon-Marseille'),
(2, 'PÃ©age', 150.00, 'PÃ©age autoroute A7'),
(4, 'Carburant', 350.00, 'Essence pour trajet long'),
(4, 'Maintenance', 100.50, 'RÃ©vision mineure'),
(4, 'PÃ©age', 70.25, 'PÃ©age autoroute');

-- ========================================
-- View for Total Cost by Transporter
-- ========================================
GO
CREATE VIEW vw_CoutParTransporteur AS
SELECT 
    t.Nom AS NomTransporteur,
    SUM(co.Montant) AS CoutTotal,
    COUNT(DISTINCT trn.TourneeId) AS NombreTournees,
    COUNT(DISTINCT l.LivraisonId) AS NombreLivraisons
FROM Transporteur t
LEFT JOIN Tournee trn ON t.TransporteurId = trn.TransporteurId
LEFT JOIN Cout co ON trn.TourneeId = co.TourneeId
LEFT JOIN Livraison l ON trn.TourneeId = l.TourneeId
GROUP BY t.TransporteurId, t.Nom;

-- ========================================
-- View for Current Active Tours
-- ========================================
GO
CREATE VIEW vw_TourneeActives AS
SELECT 
    t.TourneeId,
    tr.Nom AS Transporteur,
    c.Immatriculation AS Camion,
    CONCAT(ch.Prenom, ' ', ch.Nom) AS Chauffeur,
    t.DateTournee,
    t.StatutTournee,
    COUNT(l.LivraisonId) AS NombreLivraisons,
    SUM(co.Montant) AS CoutActuel
FROM Tournee t
JOIN Transporteur tr ON t.TransporteurId = tr.TransporteurId
JOIN Camion c ON t.CamionId = c.CamionId
JOIN Chauffeur ch ON t.ChauffeurId = ch.ChauffeurId
LEFT JOIN Livraison l ON t.TourneeId = l.TourneeId
LEFT JOIN Cout co ON t.TourneeId = co.TourneeId
WHERE t.StatutTournee IN ('PlanifiÃ©e', 'En cours')
GROUP BY t.TourneeId, tr.Nom, c.Immatriculation, ch.Prenom, ch.Nom, t.DateTournee, t.StatutTournee;

GO
print 'Database TMSDatabase created successfully with sample data!'

