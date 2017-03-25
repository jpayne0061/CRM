namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerFiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerFiles", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.CustomerFiles", new[] { "Customer_Id" });
            DropTable("dbo.CustomerFiles");
        }
    }
}
