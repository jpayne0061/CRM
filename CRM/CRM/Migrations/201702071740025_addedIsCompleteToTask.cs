namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedIsCompleteToTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "IsComplete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "IsComplete");
        }
    }
}
