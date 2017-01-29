namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedManyToOneRelToCustomerGroup : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "Group_Id1", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.AspNetUsers", name: "Group_Id", newName: "Group_Id1");
            RenameColumn(table: "dbo.AspNetUsers", name: "__mig_tmp__0", newName: "Group_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Group_Id1", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Group_Id", newName: "IX_Group_Id1");
            RenameIndex(table: "dbo.AspNetUsers", name: "__mig_tmp__0", newName: "IX_Group_Id");
            AddColumn("dbo.Customers", "Group_Id", c => c.Int());
            CreateIndex("dbo.Customers", "Group_Id");
            AddForeignKey("dbo.Customers", "Group_Id", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Customers", new[] { "Group_Id" });
            DropColumn("dbo.Customers", "Group_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Group_Id", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Group_Id1", newName: "IX_Group_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "__mig_tmp__0", newName: "IX_Group_Id1");
            RenameColumn(table: "dbo.AspNetUsers", name: "Group_Id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.AspNetUsers", name: "Group_Id1", newName: "Group_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "__mig_tmp__0", newName: "Group_Id1");
        }
    }
}
