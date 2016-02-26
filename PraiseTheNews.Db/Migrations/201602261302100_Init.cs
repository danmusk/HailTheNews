namespace PraiseTheNews.Db.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Newspapers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        RssUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PraiseCases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Url = c.String(),
                        ImageUrl = c.String(),
                        NewspaperId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Newspapers", t => t.NewspaperId, cascadeDelete: true)
                .Index(t => t.NewspaperId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PraiseCases", "NewspaperId", "dbo.Newspapers");
            DropIndex("dbo.PraiseCases", new[] { "NewspaperId" });
            DropTable("dbo.PraiseCases");
            DropTable("dbo.Newspapers");
        }
    }
}
