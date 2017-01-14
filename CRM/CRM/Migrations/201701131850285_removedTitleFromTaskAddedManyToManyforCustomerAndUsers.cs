namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedTitleFromTaskAddedManyToManyforCustomerAndUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUserCustomers",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Customer_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Customer_Id);
            
            DropColumn("dbo.Tasks", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "Title", c => c.String());
            DropForeignKey("dbo.ApplicationUserCustomers", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.ApplicationUserCustomers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserCustomers", new[] { "Customer_Id" });
            DropIndex("dbo.ApplicationUserCustomers", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserCustomers");
        }
    }
}
