namespace MVCBlueRay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDataTypeRange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BluRays", "Title", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.BluRays", "Language", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.BluRays", "Language", c => c.String(nullable: false));
            AlterColumn("dbo.BluRays", "Title", c => c.String(nullable: false));
        }
    }
}
