using Microsoft.AspNetCore.Mvc;
using ReserveBiteee.Models;
using ReserveBiteee.Service;

namespace ReserveBiteee.Controllers
{
    public class ReservationsController : Controller
    {

        public readonly IReservationService _reservationService;
        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SuccessPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BookTable([FromBody] ReservationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }


            try
            {
                model.ReservationDateTime = model.ReservationDateTime.ToLocalTime();
                int reservationId = _reservationService.BookTable(model);

                if (reservationId > 0)
                {
                    TempData["SuccessMessage"] = "Reservation submitted! Waiting for admin confirmation.";
                    return RedirectToAction("SuccessPage");
                }
                else
                {
                    TempData["ErrorMessage"] = "Reservation failed. Try again!";
                    return View("Index", model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return View("BookingPage", model);
            }
        }
        [HttpGet]
        public IActionResult GetReservationById(int id)
        {
            var reservation = _reservationService.GetReservationById(id);
            if (reservation != null)
            {
                return Json(reservation);
            }
            return NotFound("Reservation not found");
        }
    }
}
