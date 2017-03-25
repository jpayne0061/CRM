namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUploadedByandDateTimeToCustomerFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerFiles", "DateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.CustomerFiles", "UploadedBy_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.CustomerFiles", "UploadedBy_Id");
            AddForeignKey("dbo.CustomerFiles", "UploadedBy_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerFiles", "UploadedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.CustomerFiles", new[] { "UploadedBy_Id" });
            DropColumn("dbo.CustomerFiles", "UploadedBy_Id");
            DropColumn("dbo.CustomerFiles", "DateTime");
        }
    }
}
