using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticaga.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAuthentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "normalized_display_name",
                table: "users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "password_hash",
                table: "users",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_normalized_display_name",
                table: "users",
                column: "normalized_display_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rooms_host_user_id",
                table: "rooms",
                column: "host_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_rooms_users_host_user_id",
                table: "rooms",
                column: "host_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_rooms_users_host_user_id",
                table: "rooms");

            migrationBuilder.DropIndex(
                name: "ix_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_normalized_display_name",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_rooms_host_user_id",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "email",
                table: "users");

            migrationBuilder.DropColumn(
                name: "normalized_display_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_hash",
                table: "users");
        }
    }
}
