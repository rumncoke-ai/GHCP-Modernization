using System.Data.Entity.Migrations;

namespace LegacyShop.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<StoreDbContext>
    {
        public Configuration()
        {
            // Allow EF to apply non-destructive pending schema changes automatically
            // when the model has drifted from the last applied migration.
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(StoreDbContext context)
        {
        }
    }
}
