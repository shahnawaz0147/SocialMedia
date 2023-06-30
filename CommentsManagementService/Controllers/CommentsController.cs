using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommentsManagementService.Data;
using Contracts.DataContracts;
using System.Xml.Linq;
using System.Linq;
namespace CommentsManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentsManagementServiceContext _context;

        public CommentsController(CommentsManagementServiceContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentModel>>> GetCommentModel()
        {
          if (_context.CommentModel == null)
          {
              return NotFound();
          }
            return await _context.CommentModel.ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommentModel>>> GetCommentModel(int id)
        {
          if (_context.CommentModel == null)
          {
              return NotFound();
          }
            var Query = (from CommentsSource in _context.CommentModel
                          where CommentsSource.PostID==id
                         select CommentsSource);


            var comments = Query;

            if (comments == null)
            {
                return NotFound();
            }

            return Query != null ? Query.ToList() : new List<CommentModel>();
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommentModel(int id, CommentModel commentModel)
        {
            if (id != commentModel.ID)
            {
                return BadRequest();
            }

            _context.Entry(commentModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommentModel>> PostCommentModel(CommentModel commentModel)
        {
          if (_context.CommentModel == null)
          {
              return Problem("Entity set 'CommentsManagementServiceContext.CommentModel'  is null.");
          }
            _context.CommentModel.Add(commentModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommentModel", new { id = commentModel.ID }, commentModel);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentModel(int id)
        {
            if (_context.CommentModel == null)
            {
                return NotFound();
            }
            var commentModel = await _context.CommentModel.FindAsync(id);
            if (commentModel == null)
            {
                return NotFound();
            }

            _context.CommentModel.Remove(commentModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentModelExists(int id)
        {
            return (_context.CommentModel?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
