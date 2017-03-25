namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFileNameTOCustomerFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerFiles", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerFiles", "FileName");
        }
    }
}
