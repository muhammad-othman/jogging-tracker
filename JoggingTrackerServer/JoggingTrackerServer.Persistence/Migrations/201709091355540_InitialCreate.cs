namespace JoggingTrackerServer.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Distance = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 20, unicode: false),
                        Age = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 20, unicode: false),
                        Token = c.String(),
                        TokenExpiration = c.DateTime(nullable: false),
                        Permission = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jogs", "UserID", "dbo.Users");
            DropIndex("dbo.Jogs", new[] { "UserID" });
            DropTable("dbo.Users");
            DropTable("dbo.Jogs");
        }
    }
}
