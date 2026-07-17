using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagerApp.Migrations
{
    public partial class UpdatedSomeThings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Users"
                ALTER COLUMN "ReminderStartBefore" DROP DEFAULT;

                ALTER TABLE "Users"
                ALTER COLUMN "ReminderStartBefore" TYPE integer
                USING EXTRACT(EPOCH FROM "ReminderStartBefore") / 60;
            """);

                    migrationBuilder.Sql("""
                ALTER TABLE "Users"
                ALTER COLUMN "ReminderInterval" DROP DEFAULT;

                ALTER TABLE "Users"
                ALTER COLUMN "ReminderInterval" TYPE integer
                USING EXTRACT(EPOCH FROM "ReminderInterval") / 60;
            """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Users"
                ALTER COLUMN "ReminderStartBefore" TYPE interval
                USING ("ReminderStartBefore" * INTERVAL '1 minute');
            """);

            migrationBuilder.Sql("""
                ALTER TABLE "Users"
                ALTER COLUMN "ReminderInterval" TYPE interval
                USING ("ReminderInterval" * INTERVAL '1 minute');
            """);
        }
    }
}