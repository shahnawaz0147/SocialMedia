using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
namespace ImageProcessingsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        [HttpPost]
        public IActionResult ResizeAndOptimize([FromForm] IFormFile imageFile)
        {
            int lIdealImageWidth = 600;
            int lIdealImageHeight = 800;

            using (var image = Image.Load(imageFile.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(lIdealImageWidth, lIdealImageHeight),
                    Mode = ResizeMode.Max


                })); ;
                var stream = new MemoryStream();
                image.Save(stream, new JpegEncoder
                {
                    Quality = 75
                }) ;
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "image/jpeg");
            }
        
        }
    }
}
