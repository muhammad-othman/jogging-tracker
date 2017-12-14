namespace JoggingTrackerServer.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingPassword : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
    }
}
