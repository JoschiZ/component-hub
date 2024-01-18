using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComponentHub.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedComponentTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComponentPageComponentTag",
                columns: table => new
                {
                    PagesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentPageComponentTag", x => new { x.PagesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ComponentPageComponentTag_ComponentTag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "ComponentTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComponentPageComponentTag_Components_PagesId",
                        column: x => x.PagesId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ComponentTag",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "World of Warcraft" },
                    { 2, "Damage" },
                    { 3, "Tank" },
                    { 4, "Healer" },
                    { 5, "Death Knight" },
                    { 6, "Blood Death Knight" },
                    { 7, "Unholy Death Knight" },
                    { 8, "Frost Death Knight" },
                    { 9, "Demon Hunter" },
                    { 10, "Vengeance Demon Hunter" },
                    { 11, "Devastation Demon Hunter" },
                    { 12, "Druid" },
                    { 13, "Balance Druid" },
                    { 14, "Feral Druid" },
                    { 15, "Guardian Druid" },
                    { 16, "Restoration Druid" },
                    { 17, "Evoker" },
                    { 18, "Devastation Evoker" },
                    { 19, "Preservation Evoker" },
                    { 20, "Augmentation Evoker" },
                    { 21, "Hunter" },
                    { 22, "Beast Mastery Hunter" },
                    { 23, "Marksmanship Hunter" },
                    { 24, "Survival Hunter" },
                    { 25, "Mage" },
                    { 26, "Arcane Mage" },
                    { 27, "Fire Mage" },
                    { 28, "Frost Mage" },
                    { 29, "Monk" },
                    { 30, "Brewmaster Monk" },
                    { 31, "Mistweaver Monk" },
                    { 32, "Windwalker Monk" },
                    { 33, "Paladin" },
                    { 34, "Holy Paladin" },
                    { 35, "Protection Paladin" },
                    { 36, "Retribution Paladin" },
                    { 37, "Priest" },
                    { 38, "Discipline Priest" },
                    { 39, "Holy Priest" },
                    { 40, "Shadow Priest" },
                    { 41, "Rogue" },
                    { 42, "Assassination Rogue" },
                    { 43, "Outlaw Rogue" },
                    { 44, "Subtlety Rogue" },
                    { 45, "Shaman" },
                    { 46, "Elemental Shaman" },
                    { 47, "Enhancement Shaman" },
                    { 48, "Restoration Shaman" },
                    { 49, "Warlock" },
                    { 50, "Affliction Warlock" },
                    { 51, "Demonology Warlock" },
                    { 52, "Destruction Warlock" },
                    { 53, "Retail" },
                    { 54, "Amirdrassil" },
                    { 55, "Gnalroot" },
                    { 56, "Igira the Cruel" },
                    { 57, "Volcoross" },
                    { 58, "Larodar, Keeper of the Flame" },
                    { 59, "Council Of Dreams: Urctos, Aerwynn, Pip" },
                    { 60, "Nymue, Weaver of the Cycle" },
                    { 61, "Smolderon, the Firelord" },
                    { 62, "Tindral, Seer of the Flame" },
                    { 63, "Fyrakk, the Blazing" },
                    { 64, "Mythic+ DF Season 3" },
                    { 65, "Dawn of the Infinite" },
                    { 66, "Murozond's Rise" },
                    { 67, "Galakrond's Fall" },
                    { 68, "Waycrest Manor" },
                    { 69, "Darkheart Thicket" },
                    { 70, "Atal'Dazar" },
                    { 71, "Black Rook Hold" },
                    { 72, "The Everbloom" },
                    { 73, "Throne of the Tides" },
                    { 74, "World of Warcraft Classic" },
                    { 75, "Final Fantasy" },
                    { 76, "Elder Scrolls Online" },
                    { 77, "Star Wars the Old Republic" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentPageComponentTag_TagsId",
                table: "ComponentPageComponentTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentPageComponentTag");

            migrationBuilder.DropTable(
                name: "ComponentTag");
        }
    }
}
