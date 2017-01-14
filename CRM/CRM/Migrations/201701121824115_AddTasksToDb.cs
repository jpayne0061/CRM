namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTasksToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        CustomerId = c.Int(nullable: false),
                        AssignedToId = c.String(maxLength: 128),
                        AssignedById = c.String(maxLength: 128),
                        Deadline = c.DateTime(nullable: false),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AssignedById)
                .ForeignKey("dbo.AspNetUsers", t => t.AssignedToId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.AssignedToId)
                .Index(t => t.AssignedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Tasks", "AssignedToId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tasks", "AssignedById", "dbo.AspNetUsers");
            DropIndex("dbo.Tasks", new[] { "AssignedById" });
            DropIndex("dbo.Tasks", new[] { "AssignedToId" });
            DropIndex("dbo.Tasks", new[] { "CustomerId" });
            DropTable("dbo.Tasks");
        }
    }
}
