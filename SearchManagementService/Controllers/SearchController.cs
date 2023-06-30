using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contracts.DataContracts;
using SearchManagementService.Data;
using System.Linq;
namespace SearchManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly SearchManagementServiceContext _context;

        public SearchController(SearchManagementServiceContext context)
        {
            _context = context;
        }

       
        // GET: api/Search/Term
        [HttpGet("{Term}")]
        public async Task<ActionResult<IEnumerable<PostModel>>> GetSearch(string Term)
        {
         
            var Query =(from Posts in _context.PostModel
                         where Posts.Description!=null && Posts.Description.Contains(Term)
                         select  Posts);


            var postModel = Query;

            if (postModel == null)
            {
                return NotFound();
            }

            return Query != null ? Query.ToList() : new List<PostModel>();
        }

    

       
    }
}
