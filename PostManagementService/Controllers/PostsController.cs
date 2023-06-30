using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contracts.DataContracts;
using PostManagementService.Data;
using System.Net;
using PostManagementService.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient.Server;
using Newtonsoft.Json;

namespace PostManagementService.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostManagementServiceContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly string CacheListKey = "List";
        public PostsController(PostManagementServiceContext context,IConfiguration configurationManager,IDistributedCache cache)
        {
            _context = context;
            _httpClient = new HttpClient();
            _configuration = configurationManager;
            _cache = cache;
        }

        // GET: api/Posts
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostModel>>> GetPostModel()
        {
            var list = await _cache.GetRecordAsyng<List<PostModel>>(CacheListKey);
            if (list is null)
            {
                if (_context.PostModel == null)
                {
                    return NotFound();
                }
                var Posts=await _context.PostModel.ToListAsync();
                if (Posts != null)
                {
                    foreach (var Post in Posts)
                    {
                        Post.PostComments = await GetPostComments(Post.ID);
                    }
                }
                return Posts;
            }return list;
        }

        // GET: api/Posts/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostModel>> GetPostModel(int id)
        {
          if (_context.PostModel == null)
          {
              return NotFound();
          }
           var PostModel= await _cache.GetRecordAsyng<PostModel>(id.ToString());
            if (PostModel is null)
            {
                var postModel = await _context.PostModel.FindAsync(id);

                if (postModel == null)
                {
                    return NotFound();
                }
                postModel.PostComments = await GetPostComments(postModel.ID);
                return postModel;
            }
            return PostModel;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostModel(int id, [FromForm] PostModel postModel)
        {
            if (id != postModel.ID)
            {
                return BadRequest();
            }
            postModel.ImageData =await ResizeImage(postModel.Image);
                
            _context.Entry(postModel).State = EntityState.Modified;

            try
            {
                
                await _context.SaveChangesAsync();
                await _cache.SetRecordAsync<PostModel>(postModel.ID.ToString(), postModel);
                await _cache.AddItemsinListCache<PostModel>(CacheListKey, postModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostModelExists(id))
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

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostModel>> PostPostModel([FromForm] PostModel postModel)
        {
          if (_context.PostModel == null)
          {
              return Problem("Entity set 'PostManagementServiceContext.PostModel'  is null.");
          }
            postModel.ImageData = await ResizeImage(postModel.Image);
            _context.PostModel.Add(postModel);
            await _context.SaveChangesAsync();
            await _cache.SetRecordAsync<PostModel>(postModel.ID.ToString(), postModel);
            await _cache.AddItemsinListCache<PostModel>(CacheListKey, postModel);
            return CreatedAtAction("GetPostModel", new { id = postModel.ID }, postModel);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePostModel(int id)
        {
            if (_context.PostModel == null)
            {
                return NotFound();
            }
            var postModel = await _context.PostModel.FindAsync(id);
            if (postModel == null)
            {
                return NotFound();
            }

            _context.PostModel.Remove(postModel);
            _cache.Remove(id.ToString());
            await _cache.DeleteFromListAsyn(CacheListKey, postModel,p=>p.ID==postModel.ID);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<List<CommentModel>> GetPostComments(int PostId)
        {
            string CommentService = _configuration.GetSection("AppSettings:CommentManagementService").Value;
            HttpClient commentsClient = new HttpClient();
            commentsClient.BaseAddress = new Uri(CommentService);

            HttpResponseMessage response = await commentsClient.GetAsync("api/Comments/" + PostId.ToString());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<List<CommentModel>>(responseContent);
                return comments;
            }
            return null;
        }
        private async Task<byte[]> ResizeImage(IFormFile ImageFile)
        {
            
            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(ImageFile.OpenReadStream()), "imageFile","imageFile");
            string ImageService = _configuration.GetSection("AppSettings:ImageManagementService").Value;
            _httpClient.BaseAddress = new Uri(ImageService);

            HttpResponseMessage response = await _httpClient.PostAsync("api/Images", formData);
            if(response.StatusCode==HttpStatusCode.OK)
            {
                var imageStream = await response.Content.ReadAsStreamAsync();
                return ReadToEnd(imageStream);

            }
            return null;
        }
        private byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        private bool PostModelExists(int id)
        {
            return (_context.PostModel?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
