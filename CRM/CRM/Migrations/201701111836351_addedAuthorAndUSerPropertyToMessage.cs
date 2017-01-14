namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedAuthorAndUSerPropertyToMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "AuthorId", c => c.String(maxLength: 128));
            AddColumn("dbo.Messages", "AuthorName", c => c.String());
            CreateIndex("dbo.Messages", "AuthorId");
            AddForeignKey("dbo.Messages", "AuthorId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "AuthorId" });
            DropColumn("dbo.Messages", "AuthorName");
            DropColumn("dbo.Messages", "AuthorId");
        }
    }
}
