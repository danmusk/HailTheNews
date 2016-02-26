using System;
using System.Data.Entity;
using PraiseTheNews.Db.Model;

namespace PraiseTheNews.Db
{
    public class PraiseDbContext : DbContext
    {
        public IDbSet<PraiseCase> PraiseCases { get; set; }
        public IDbSet<Newspaper> Newspapers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }
    }
}
