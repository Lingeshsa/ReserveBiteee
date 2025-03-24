using Microsoft.AspNetCore.Mvc.RazorPages;
using ReserveBiteee.Models;

namespace ReserveBiteee.Service
{
    public interface IImageService
    {
        int UploadImage(CustomerPageImages image);
        List<CustomerPageImages> GetImages();

    }
}
