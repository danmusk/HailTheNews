using System;
using System.Collections.Generic;
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
            var tv2Praises = ScrapeTV2WebPage();
            var dagbladetPraises = ScrapeDagbladetWebPage();
            var vgPraises = ScrapeVGWebPage();
            var nettavisenPraises = ScrapeNettavisenWebPage();
            var allPraises = tv2Praises.Concat(dagbladetPraises).Concat(vgPraises).Concat(nettavisenPraises).ToList();
            AddNewPraises(allPraises);

            Console.WriteLine("End");
        }

        private static void AddNewPraises(List<PraiseCase> allPraises)
        {
            int newPraiseCasesFound = 0;
            using (var dbContext = new PraiseDbContext())
            {
                foreach (var praise in allPraises)
                {
                    var praiseExistsInDb = dbContext.PraiseCases.Any(x => x.Url == praise.Url.ToString());
                    if (praiseExistsInDb == false)
                    {
                        praise.AddedDate = DateTimeOffset.UtcNow;
                        dbContext.PraiseCases.Add(praise);
                        newPraiseCasesFound++;
                    }
                }
                dbContext.SaveChanges();
            }
            Console.WriteLine("New praises found: " + newPraiseCasesFound);
        }

        private static List<PraiseCase> ScrapeTV2WebPage()
        {
            var result = new List<PraiseCase>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("http://www.tv2.no");
            var rootArticleNodes = document.DocumentNode.SelectNodes("//article");
            var praiseArticles = new List<HtmlNode>();

            foreach (var article in rootArticleNodes)
            {
                var articleHasPraise = article.ChildNodes.Any(x => x.InnerText.ToLower().Contains("hylles"));
                if (articleHasPraise)
                    praiseArticles.Add(article);
            }

            foreach (var praiseArticle in praiseArticles)
            {
                var slug = praiseArticle.SelectNodes(".//h3")?.FirstOrDefault()?.InnerText;
                var title = praiseArticle.SelectNodes(".//h2")?.FirstOrDefault()?.InnerText;
                var text = praiseArticle.SelectNodes(".//p")?.FirstOrDefault()?.InnerText;
                var fullTitle = (slug + " " + title + " " + text).Trim();
                var imgUrl = praiseArticle.SelectNodes(".//img")?.FirstOrDefault()?.GetAttributeValue("data-src", ""); //.GetAttributeValue("src"); //?.FirstOrDefault()?.InnerText;
                var articleUrl = praiseArticle.SelectNodes(".//a")?.FirstOrDefault()?.GetAttributeValue("href", ""); //.GetAttributeValue("src"); //?.FirstOrDefault()?.InnerText;
                if (!articleUrl.Contains("http://www.tv2.no"))
                    articleUrl = "http://www.tv2.no" + articleUrl;

                var praiseCase = new PraiseCase();
                praiseCase.Title = fullTitle;
                praiseCase.Url = articleUrl;
                praiseCase.ImageUrl = imgUrl;
                praiseCase.NewspaperId = 4; // TV2
                result.Add(praiseCase);
            }
            return result;
        }
        private static List<PraiseCase> ScrapeDagbladetWebPage()
        {
            var result = new List<PraiseCase>();
            var baseUrl = "http://www.dagbladet.no";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(baseUrl);
            var rootArticleNodes = document.DocumentNode.SelectNodes("//article");
            var praiseArticles = new List<HtmlNode>();

            foreach (var article in rootArticleNodes)
            {
                var articleHasPraise = article.ChildNodes.Any(x => x.InnerText.ToLower().Contains("hylles"));
                if (articleHasPraise)
                    praiseArticles.Add(article);
            }

            foreach (var praiseArticle in praiseArticles)
            {
                var title = praiseArticle.SelectNodes(".//h1")?.FirstOrDefault()?.InnerText;
                var fullTitle = title;
                var imgUrl = praiseArticle.SelectNodes(".//img")?.FirstOrDefault()?.GetAttributeValue("data-srcset", "");
                var articleUrl = praiseArticle.SelectNodes(".//a")?.FirstOrDefault()?.GetAttributeValue("href", "");
                
                if (!string.IsNullOrEmpty(imgUrl))
                    imgUrl = "http:" + imgUrl;

                var praiseCase = new PraiseCase();
                praiseCase.Title = fullTitle;
                praiseCase.Url = HtmlEntity.DeEntitize(articleUrl);
                praiseCase.ImageUrl = HtmlEntity.DeEntitize(imgUrl);
                praiseCase.NewspaperId = 1; // Dagbladet
                result.Add(praiseCase);
            }
            return result;
        }
        private static List<PraiseCase> ScrapeVGWebPage()
        {
            var result = new List<PraiseCase>();
            var baseUrl = "http://www.vg.no";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(baseUrl);
            var rootArticleNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'article-extract')]");

            var praiseArticles = new List<HtmlNode>();
            foreach (var article in rootArticleNodes)
            {
                var articleHasPraise = article.ChildNodes.Any(x => x.InnerText.ToLower().Contains("hylles"));
                if (articleHasPraise)
                    praiseArticles.Add(article);
            }

            foreach (var praiseArticle in praiseArticles)
            {
                var title = praiseArticle.SelectNodes(".//a")?.FirstOrDefault()?.GetAttributeValue("title", "");
                var fullTitle = title;
                var imgUrl = praiseArticle.SelectNodes(".//img")?.FirstOrDefault()?.GetAttributeValue("data-src", "");
                var articleUrl = praiseArticle.SelectNodes(".//a")?.FirstOrDefault()?.GetAttributeValue("href", "");

                if (!string.IsNullOrEmpty(articleUrl))
                    articleUrl = baseUrl + articleUrl;
                if (!string.IsNullOrEmpty(imgUrl))
                    imgUrl = "http:" + imgUrl;

                var praiseCase = new PraiseCase();
                praiseCase.Title = fullTitle;
                praiseCase.Url = HtmlEntity.DeEntitize(articleUrl);
                praiseCase.ImageUrl = HtmlEntity.DeEntitize(imgUrl);
                praiseCase.NewspaperId = 2; // VG
                result.Add(praiseCase);
            }
            return result;
        }
        private static List<PraiseCase> ScrapeNettavisenWebPage()
        {
            var result = new List<PraiseCase>();
            var baseUrl = "http://www.nettavisen.no";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(baseUrl);
            var rootArticleNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'df-article-content')]");

            var praiseArticles = new List<HtmlNode>();
            foreach (var article in rootArticleNodes)
            {
                var articleHasPraise = article.ChildNodes.Any(x => x.InnerText.ToLower().Contains("hylles"));
                if (articleHasPraise)
                    praiseArticles.Add(article);
            }

            foreach (var praiseArticle in praiseArticles)
            {
                var title = praiseArticle.SelectNodes(".//h3").First().SelectNodes(".//span").First().InnerText;
                var imgUrl = praiseArticle.SelectNodes(".//img")?.FirstOrDefault()?.GetAttributeValue("src", "");
                var articleUrl = praiseArticle.SelectNodes(".//a")?.FirstOrDefault()?.GetAttributeValue("href", "");

                var praiseCase = new PraiseCase();
                praiseCase.Title = title;
                praiseCase.Url = HtmlEntity.DeEntitize(articleUrl);
                praiseCase.ImageUrl = HtmlEntity.DeEntitize(imgUrl);
                praiseCase.NewspaperId = 3; // Nettavisen
                result.Add(praiseCase);
            }
            return result;
        }
    }
}
