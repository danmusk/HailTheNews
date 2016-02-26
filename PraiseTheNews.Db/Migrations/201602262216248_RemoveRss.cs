namespace PraiseTheNews.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PraiseCases", "AddedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.Newspapers", "RssUrl");
            DropColumn("dbo.PraiseCases", "PublishedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PraiseCases", "PublishedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Newspapers", "RssUrl", c => c.String());
            DropColumn("dbo.PraiseCases", "AddedDate");
        }
    }
}
