namespace WebViecLam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        FullName = c.String(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            AddColumn("dbo.Jobs", "UserId", c => c.Int());
            CreateIndex("dbo.Jobs", "UserId");
            AddForeignKey("dbo.Jobs", "UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "UserId", "dbo.Users");
            DropIndex("dbo.Jobs", new[] { "UserId" });
            DropColumn("dbo.Jobs", "UserId");
            DropTable("dbo.Users");
        }
    }
}
