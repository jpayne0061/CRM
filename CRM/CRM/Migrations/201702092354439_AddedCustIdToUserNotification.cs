namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCustIdToUserNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserNotifications", "CustomerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserNotifications", "CustomerId");
        }
    }
}
