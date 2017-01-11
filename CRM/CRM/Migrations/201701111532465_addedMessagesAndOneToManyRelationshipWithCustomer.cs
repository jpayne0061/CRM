namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedMessagesAndOneToManyRelationshipWithCustomer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Messages", new[] { "CustomerId" });
            DropTable("dbo.Messages");
        }
    }
}
