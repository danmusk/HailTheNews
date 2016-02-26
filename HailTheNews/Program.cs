using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SimpleFeedReader;

namespace HailTheNews
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            ParseRssFeed();
            Console.WriteLine("End");
            Console.ReadKey();

        }
        public static void ParseRssFeed()
        {
            
            var reader = new FeedReader();
            //var items = reader.RetrieveFeed("http://www.dagbladet.no/rss/nyheter/");
            //var items = reader.RetrieveFeed("http://www.vg.no/rss/feed/?limit=20&format=rss");
            //var items = reader.RetrieveFeed("https://www.tv2.no/rest/cms-feeds-dw-rest/v2/cms/article/");
            var items = reader.RetrieveFeed("http://rss.nettavisen.no/");

            using (HailDbContext dbContext = new HailDbContext())
            {
            }

            foreach (var i in items)
            {
                i.
                Console.WriteLine(string.Format("{0}\t{1}", i.Date.ToString("g"), i.Title));
            }
        }

    }
}
