using System;
using System.Data.Entity;
using System.Linq;
using HtmlAgilityPack;
using PraiseTheNews.Db;
using PraiseTheNews.Db.Migrations;
using PraiseTheNews.Db.Model;
using SimpleFeedReader;

namespace PraiseTheNews.ConsoleParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PraiseDbContext, Configuration>());
            //ParseAllNewspaperRssFeeds();
            ParseWebPages();
            Console.WriteLine("End");
        }

        private static void ParseWebPages()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("http://www.tv2.no");
            var h2Nodes = document.DocumentNode.SelectNodes("//h2");
            foreach (var h2 in h2Nodes)
            {
                Console.WriteLine(h2.InnerText);
            }
            Console.ReadKey();
        }

        public static void ParseAllNewspaperRssFeeds()
        {
            int newPraiseCasesFound = 0;
            using (var dbContext = new PraiseDbContext())
            {
                var newspapers = dbContext.Newspapers.ToList();
                foreach (var newspaper in newspapers)
                {
                    var reader = new FeedReader();
                    var items = reader.RetrieveFeed(newspaper.RssUrl);
                    foreach (var i in items)
                    {
                        if (i.Title.ToLower().Contains("hylles") || i.Summary.ToLower().Contains("hylles"))
                        {
                            var caseExistsInDb = dbContext.PraiseCases.Any(x => x.Url == i.Uri.ToString());
                            if (caseExistsInDb == false)
                            {
                                var praiseCase = new PraiseCase();
                                praiseCase.NewspaperId = newspaper.Id;
                                praiseCase.Title = i.Title;
                                praiseCase.Url = i.Uri.ToString();
                                praiseCase.PublishedDate = i.PublishDate;

                                dbContext.PraiseCases.Add(praiseCase);
                                newPraiseCasesFound++;
                            }
                        }
                    }
                    dbContext.SaveChanges();
                }
            }
            Console.WriteLine("New praisecases found: " + newPraiseCasesFound);
        }
    }
}
