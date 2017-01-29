namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedChatMessage : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserCustomers", newName: "CustomerApplicationUsers");
            RenameColumn(table: "dbo.AspNetUsers", name: "Group_Id1", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.AspNetUsers", name: "Group_Id", newName: "Group_Id1");
            RenameColumn(table: "dbo.AspNetUsers", name: "__mig_tmp__0", newName: "Group_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Group_Id1", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Group_Id", newName: "IX_Group_Id1");
            RenameIndex(table: "dbo.AspNetUsers", name: "__mig_tmp__0", newName: "IX_Group_Id");
            DropPrimaryKey("dbo.CustomerApplicationUsers");
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.String(maxLength: 128),
                        ReceiverId = c.String(maxLength: 128),
                        Body = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverId)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            AddPrimaryKey("dbo.CustomerApplicationUsers", new[] { "Customer_Id", "ApplicationUser_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatMessages", "ReceiverId", "dbo.AspNetUsers");
            DropIndex("dbo.ChatMessages", new[] { "ReceiverId" });
            DropIndex("dbo.ChatMessages", new[] { "SenderId" });
            DropPrimaryKey("dbo.CustomerApplicationUsers");
            DropTable("dbo.ChatMessages");
            AddPrimaryKey("dbo.CustomerApplicationUsers", new[] { "ApplicationUser_Id", "Customer_Id" });
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Group_Id", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Group_Id1", newName: "IX_Group_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "__mig_tmp__0", newName: "IX_Group_Id1");
            RenameColumn(table: "dbo.AspNetUsers", name: "Group_Id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.AspNetUsers", name: "Group_Id1", newName: "Group_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "__mig_tmp__0", newName: "Group_Id1");
            RenameTable(name: "dbo.CustomerApplicationUsers", newName: "ApplicationUserCustomers");
        }
    }
}
