using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using ReserveBiteee.Models;
using ReserveBiteee.Service;

namespace ReserveBiteee.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public IActionResult UploadImage(IFormFile file, string name, string description)
        {

            if (file == null || file.Length == 0)
            {
                return BadRequest("Please upload a valid image file.");
            }

            string base64String;

            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                byte[] imageBytes = ms.ToArray();
                base64String = Convert.ToBase64String(imageBytes);
            }


            var image = new CustomerPageImages
            {
                Name = name,
                Description = description,
                ImageBase64 = base64String
            };
            int result = _imageService.UploadImage(image);

            if (result > 0 || result == 0)
            {
                return Ok("Image uploaded and saved successfully!");
            }
            else
            {
                return StatusCode(500, "Failed to save the image.");
            }

        }


    }
}
