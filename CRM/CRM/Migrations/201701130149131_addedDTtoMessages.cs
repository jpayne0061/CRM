namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedDTtoMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "DateTime");
        }
    }
}
