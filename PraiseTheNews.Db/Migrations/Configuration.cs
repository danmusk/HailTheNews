using PraiseTheNews.Db.Model;

namespace PraiseTheNews.Db.Migrations
{
    using System.Data.Entity.Migrations;

    public class Configuration : DbMigrationsConfiguration<PraiseDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PraiseDbContext context)
        {
            context.Newspapers.AddOrUpdate(
              n => n.Id,
              new Newspaper { Id = 1, Name = "Dagbladet", Url = "https://www.dagbladet.no", RssUrl = "http://www.dagbladet.no/rss/nyheter/" },
              new Newspaper { Id = 2, Name = "VG", Url = "http://www.vg.no", RssUrl = "http://www.vg.no/rss/feed/?limit=20&format=rss" },
              new Newspaper { Id = 3, Name = "Nettavisen", Url = "http://www.nettavisen.no", RssUrl = "http://rss.nettavisen.no/" },
              new Newspaper { Id = 4, Name = "TV2", Url = "http://www.tv2.no", RssUrl = "https://www.tv2.no/rest/cms-feeds-dw-rest/v2/cms/article/" },
              new Newspaper { Id = 5, Name = "NRK", Url = "http://www.nrk.no", RssUrl = "http://www.nrk.no/toppsaker.rss" }
            );
        }
    }
}

