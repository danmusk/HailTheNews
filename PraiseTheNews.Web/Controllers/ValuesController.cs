using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PraiseTheNews.Db;
using PraiseTheNews.Db.Model;

namespace PraiseTheNews.Web.Controllers
{
    public class PraiseCaseController : ApiController
    {
        [Route("api/PraiseCases")]
        public IEnumerable<PraiseCase> Get()
        {
            using (PraiseDbContext context = new PraiseDbContext())
            {
                return context.PraiseCases.Include(x => x.Newspaper).OrderByDescending(x => x.AddedDate).ToList();
            }
        }
    }
}
