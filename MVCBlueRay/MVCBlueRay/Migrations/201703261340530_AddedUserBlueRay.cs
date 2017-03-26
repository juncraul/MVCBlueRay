namespace MVCBlueRay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserBlueRay : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserBlueRays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BluRayId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BluRays", t => t.BluRayId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BluRayId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserBlueRays", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserBlueRays", "BluRayId", "dbo.BluRays");
            DropIndex("dbo.UserBlueRays", new[] { "UserId" });
            DropIndex("dbo.UserBlueRays", new[] { "BluRayId" });
            DropTable("dbo.UserBlueRays");
        }
    }
}
