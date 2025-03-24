using Microsoft.AspNetCore.Mvc;
using ReserveBiteee.Models;
using ReserveBiteee.Service;

namespace ReserveBiteee.Controllers
{
    public class CustomerController : Controller
    {

        private readonly IImageService _imageService;

        public CustomerController(IImageService imageService)
        {
            _imageService = imageService;
        }

        public IActionResult CustomerIndex()
        {
            return View();
        }

        public IActionResult CustomerHome()
        {
            List<CustomerPageImages> images = _imageService.GetImages();

            return View(images);
        }

    }
}
