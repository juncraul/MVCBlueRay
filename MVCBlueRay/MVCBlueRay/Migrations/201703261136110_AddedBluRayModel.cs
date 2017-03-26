namespace MVCBlueRay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBluRayModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BluRays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ReleaseYear = c.Int(nullable: false),
                        Language = c.String(nullable: false),
                        RunTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BluRays");
        }
    }
}
