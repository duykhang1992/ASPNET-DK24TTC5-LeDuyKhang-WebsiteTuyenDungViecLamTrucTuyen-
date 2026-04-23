namespace WebViecLam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogoPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "LogoPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "LogoPath");
        }
    }
}
