namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifiedPropsOfCustomerandUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tasks", "ApplicationUser_Id");
            AddForeignKey("dbo.Tasks", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Tasks", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Tasks", "ApplicationUser_Id");
        }
    }
}
