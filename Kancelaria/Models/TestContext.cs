using System.Data.Entity;
namespace Kancelaria.Models
{
}


namespace Kancelaria.Models
{
    public class testContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Kancelaria.Models.testContext>());

        public testContext() : base("name=testContext")
        {
        }

        public DbSet<Inwestycja> Inwestycjas { get; set; }
    }

    public class TestContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Kancelaria.Models.TestContext>());

        public TestContext() : base("name=TestContext")
        {
        }

        public DbSet<FakturaZakupu> FakturaZakupus { get; set; }
    }
}
