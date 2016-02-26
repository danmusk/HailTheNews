using System;

namespace PraiseTheNews.Db.Model
{
    public class PraiseCase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }

        public virtual Newspaper Newspaper { get; set; }
        public int NewspaperId { get; set; }

        public DateTimeOffset AddedDate { get; set; }
    }
}