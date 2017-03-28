namespace MVCBlueRay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIs3partyProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsRegisteredWith3rdParty", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsRegisteredWith3rdParty");
        }
    }
}
