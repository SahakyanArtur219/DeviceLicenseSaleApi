using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeviceLicenseSaleApi.Migrations
{
    public partial class AddUserRoleAndAuthConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[Users]', N'U') IS NULL
                    THROW 50000, 'Table [Users] does not exist.', 1;

                IF OBJECT_ID(N'[UserProfiles]', N'U') IS NULL
                    THROW 50000, 'Table [UserProfiles] does not exist.', 1;
                """);

            migrationBuilder.Sql(
                """
                UPDATE [Users]
                SET [CreatedAt] = SYSUTCDATETIME()
                WHERE [CreatedAt] IS NULL;

                UPDATE [Users]
                SET [IsActive] = 1
                WHERE [IsActive] IS NULL;

                IF EXISTS (
                    SELECT [Email]
                    FROM [Users]
                    GROUP BY [Email]
                    HAVING COUNT(*) > 1
                )
                    THROW 50001, 'Duplicate values exist in Users.Email. Clean them up before applying the unique index.', 1;

                IF EXISTS (
                    SELECT [Username]
                    FROM [Users]
                    GROUP BY [Username]
                    HAVING COUNT(*) > 1
                )
                    THROW 50002, 'Duplicate values exist in Users.Username. Clean them up before applying the unique index.', 1;

                IF EXISTS (
                    SELECT [UserId]
                    FROM [UserProfiles]
                    GROUP BY [UserId]
                    HAVING COUNT(*) > 1
                )
                    THROW 50003, 'Duplicate values exist in UserProfiles.UserId. Clean them up before applying the unique index.', 1;

                UPDATE [UserProfiles]
                SET [Phone] = ''
                WHERE [Phone] IS NULL;

                UPDATE [UserProfiles]
                SET [Address] = ''
                WHERE [Address] IS NULL;
                """);

            migrationBuilder.Sql(
                """
                IF COL_LENGTH('Users', 'Role') IS NULL
                BEGIN
                    ALTER TABLE [Users]
                    ADD [Role] nvarchar(20) NOT NULL
                        CONSTRAINT [DF_Users_Role] DEFAULT N'User';
                END
                """);

            migrationBuilder.Sql(
                """
                DECLARE @sql nvarchar(max);

                SELECT @sql = STRING_AGG(
                    'ALTER TABLE [Users] DROP CONSTRAINT [' + kc.name + '];',
                    ' ')
                FROM sys.key_constraints kc
                INNER JOIN sys.index_columns ic
                    ON ic.object_id = kc.parent_object_id
                   AND ic.index_id = kc.unique_index_id
                INNER JOIN sys.columns c
                    ON c.object_id = ic.object_id
                   AND c.column_id = ic.column_id
                WHERE kc.parent_object_id = OBJECT_ID(N'[Users]')
                  AND kc.type = 'UQ'
                  AND c.name IN ('Username', 'Email');

                IF @sql IS NOT NULL
                    EXEC sp_executesql @sql;

                SELECT @sql = STRING_AGG(
                    'ALTER TABLE [UserProfiles] DROP CONSTRAINT [' + kc.name + '];',
                    ' ')
                FROM sys.key_constraints kc
                INNER JOIN sys.index_columns ic
                    ON ic.object_id = kc.parent_object_id
                   AND ic.index_id = kc.unique_index_id
                INNER JOIN sys.columns c
                    ON c.object_id = ic.object_id
                   AND c.column_id = ic.column_id
                WHERE kc.parent_object_id = OBJECT_ID(N'[UserProfiles]')
                  AND kc.type = 'UQ'
                  AND c.name = 'UserId';

                IF @sql IS NOT NULL
                    EXEC sp_executesql @sql;

                ALTER TABLE [Users] ALTER COLUMN [Username] nvarchar(50) NOT NULL;
                ALTER TABLE [Users] ALTER COLUMN [Email] nvarchar(256) NOT NULL;
                ALTER TABLE [Users] ALTER COLUMN [PasswordHash] nvarchar(200) NOT NULL;
                ALTER TABLE [Users] ALTER COLUMN [IsActive] bit NOT NULL;
                ALTER TABLE [Users] ALTER COLUMN [CreatedAt] datetime2 NOT NULL;

                ALTER TABLE [UserProfiles] ALTER COLUMN [FirstName] nvarchar(100) NOT NULL;
                ALTER TABLE [UserProfiles] ALTER COLUMN [LastName] nvarchar(100) NOT NULL;
                ALTER TABLE [UserProfiles] ALTER COLUMN [Phone] nvarchar(30) NOT NULL;
                ALTER TABLE [UserProfiles] ALTER COLUMN [Address] nvarchar(300) NOT NULL;
                """);

            migrationBuilder.Sql(
                """
                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.default_constraints dc
                    INNER JOIN sys.columns c
                        ON c.object_id = dc.parent_object_id
                       AND c.column_id = dc.parent_column_id
                    WHERE dc.parent_object_id = OBJECT_ID(N'[Users]')
                      AND c.name = 'IsActive'
                )
                BEGIN
                    ALTER TABLE [Users]
                    ADD CONSTRAINT [DF_Users_IsActive] DEFAULT ((1)) FOR [IsActive];
                END
                """);

            migrationBuilder.Sql(
                """
                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_Users_Email'
                      AND object_id = OBJECT_ID(N'[Users]')
                )
                BEGIN
                    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
                END

                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_Users_Username'
                      AND object_id = OBJECT_ID(N'[Users]')
                )
                BEGIN
                    CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);
                END

                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_UserProfiles_UserId'
                      AND object_id = OBJECT_ID(N'[UserProfiles]')
                )
                BEGIN
                    CREATE UNIQUE INDEX [IX_UserProfiles_UserId] ON [UserProfiles] ([UserId]);
                END
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_UserProfiles_UserId'
                      AND object_id = OBJECT_ID(N'[UserProfiles]')
                )
                    DROP INDEX [IX_UserProfiles_UserId] ON [UserProfiles];

                IF EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_Users_Email'
                      AND object_id = OBJECT_ID(N'[Users]')
                )
                    DROP INDEX [IX_Users_Email] ON [Users];

                IF EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_Users_Username'
                      AND object_id = OBJECT_ID(N'[Users]')
                )
                    DROP INDEX [IX_Users_Username] ON [Users];

                IF COL_LENGTH('Users', 'Role') IS NOT NULL
                BEGIN
                    DECLARE @roleConstraintName nvarchar(128);

                    SELECT @roleConstraintName = dc.name
                    FROM sys.default_constraints dc
                    INNER JOIN sys.columns c
                        ON c.object_id = dc.parent_object_id
                       AND c.column_id = dc.parent_column_id
                    WHERE dc.parent_object_id = OBJECT_ID(N'[Users]')
                      AND c.name = 'Role';

                    IF @roleConstraintName IS NOT NULL
                        EXEC('ALTER TABLE [Users] DROP CONSTRAINT [' + @roleConstraintName + ']');

                    ALTER TABLE [Users] DROP COLUMN [Role];
                END
                """);
        }
    }
}
