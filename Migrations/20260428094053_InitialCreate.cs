using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS_Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Transporteur",
                columns: table => new
                {
                    TransporteurId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transporteur", x => x.TransporteurId);
                });

            migrationBuilder.CreateTable(
                name: "DemandeLivraison",
                columns: table => new
                {
                    DemandeLivraisonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    TypeService = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionMarchandise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    PoidsKg = table.Column<int>(type: "int", nullable: false),
                    DateSouhaitee = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontantEstime = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CamionAffecteId = table.Column<int>(type: "int", nullable: true),
                    ChauffeurAffecteId = table.Column<int>(type: "int", nullable: true),
                    NumeroRecu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateValidation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommentaireValidation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandeLivraison", x => x.DemandeLivraisonId);
                    table.ForeignKey(
                        name: "FK_DemandeLivraison_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId");
                });

            migrationBuilder.CreateTable(
                name: "Camion",
                columns: table => new
                {
                    CamionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransporteurId = table.Column<int>(type: "int", nullable: false),
                    Marque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modele = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Immatriculation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapaciteKg = table.Column<int>(type: "int", nullable: false),
                    DateAcquisition = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstActif = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camion", x => x.CamionId);
                    table.ForeignKey(
                        name: "FK_Camion_Transporteur_TransporteurId",
                        column: x => x.TransporteurId,
                        principalTable: "Transporteur",
                        principalColumn: "TransporteurId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chauffeur",
                columns: table => new
                {
                    ChauffeurId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransporteurId = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroPermis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateEmbauche = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstActif = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chauffeur", x => x.ChauffeurId);
                    table.ForeignKey(
                        name: "FK_Chauffeur_Transporteur_TransporteurId",
                        column: x => x.TransporteurId,
                        principalTable: "Transporteur",
                        principalColumn: "TransporteurId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tournee",
                columns: table => new
                {
                    TourneeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransporteurId = table.Column<int>(type: "int", nullable: false),
                    CamionId = table.Column<int>(type: "int", nullable: false),
                    ChauffeurId = table.Column<int>(type: "int", nullable: false),
                    DateTournee = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatutTournee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoutTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontantEstime = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournee", x => x.TourneeId);
                    table.ForeignKey(
                        name: "FK_Tournee_Camion_CamionId",
                        column: x => x.CamionId,
                        principalTable: "Camion",
                        principalColumn: "CamionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tournee_Chauffeur_ChauffeurId",
                        column: x => x.ChauffeurId,
                        principalTable: "Chauffeur",
                        principalColumn: "ChauffeurId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tournee_Transporteur_TransporteurId",
                        column: x => x.TransporteurId,
                        principalTable: "Transporteur",
                        principalColumn: "TransporteurId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cout",
                columns: table => new
                {
                    CoutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourneeId = table.Column<int>(type: "int", nullable: false),
                    TypeCout = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DateCout = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cout", x => x.CoutId);
                    table.ForeignKey(
                        name: "FK_Cout_Tournee_TourneeId",
                        column: x => x.TourneeId,
                        principalTable: "Tournee",
                        principalColumn: "TourneeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Livraison",
                columns: table => new
                {
                    LivraisonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourneeId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PoidsKg = table.Column<int>(type: "int", nullable: false),
                    DateLivraison = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatutLivraison = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontantEstime = table.Column<decimal>(type: "decimal(18,2)", precision: 10, scale: 2, nullable: false),
                    DateRealisation = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livraison", x => x.LivraisonId);
                    table.ForeignKey(
                        name: "FK_Livraison_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Livraison_Tournee_TourneeId",
                        column: x => x.TourneeId,
                        principalTable: "Tournee",
                        principalColumn: "TourneeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Camion_TransporteurId",
                table: "Camion",
                column: "TransporteurId");

            migrationBuilder.CreateIndex(
                name: "IX_Chauffeur_TransporteurId",
                table: "Chauffeur",
                column: "TransporteurId");

            migrationBuilder.CreateIndex(
                name: "IX_Cout_TourneeId",
                table: "Cout",
                column: "TourneeId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandeLivraison_ClientId",
                table: "DemandeLivraison",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Livraison_ClientId",
                table: "Livraison",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Livraison_TourneeId",
                table: "Livraison",
                column: "TourneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournee_CamionId",
                table: "Tournee",
                column: "CamionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournee_ChauffeurId",
                table: "Tournee",
                column: "ChauffeurId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournee_TransporteurId",
                table: "Tournee",
                column: "TransporteurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cout");

            migrationBuilder.DropTable(
                name: "DemandeLivraison");

            migrationBuilder.DropTable(
                name: "Livraison");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Tournee");

            migrationBuilder.DropTable(
                name: "Camion");

            migrationBuilder.DropTable(
                name: "Chauffeur");

            migrationBuilder.DropTable(
                name: "Transporteur");
        }
    }
}
