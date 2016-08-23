using System.Data.Entity.Migrations;

namespace Ctrip.Component
{
    public sealed class EFConfiguration : DbMigrationsConfiguration<AuthContext>
    {
        public EFConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "OAuthPractice.ProtectedApi.Auth.AuthContext";
        }

        protected override void Seed(AuthContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
