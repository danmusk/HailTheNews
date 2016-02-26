namespace PraiseTheNews.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublishedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PraiseCases", "PublishedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PraiseCases", "PublishedDate");
        }
    }
}
