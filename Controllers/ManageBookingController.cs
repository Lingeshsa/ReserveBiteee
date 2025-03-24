using Microsoft.AspNetCore.Mvc;
using ReserveBiteee.Models;
using ReserveBiteee.Service;

namespace ReserveBiteee.Controllers
{
    public class ManageBookingController : Controller
    {

        private readonly IReservationService _reservationService;
        private readonly IEmailService _emailService;
        public ManageBookingController(IReservationService reservationService, IEmailService emailService)
        {
            _reservationService = reservationService;
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }
        public IActionResult Index()
        {
            var reservations = _reservationService.GetAllReservations();
            return View(reservations);
        }
        [HttpPost]
        public IActionResult UpdateStatus([FromBody] ReservationModel model)
        {
            try
            {
                bool isUpdated = _reservationService.UpdateReservationStatus(model.Id, model.Status);

                if (isUpdated)
                {
                    var reservation = _reservationService.GetReservationById(model.Id);

                    if (reservation != null)
                    {
                        string menuLink = "<a href='http://localhost:5156/Customer/CustomerIndex' target='_blank'>View Our Menu</a>";

                        string subject = $"ReserveBite: {model.Status}";
                        string message = $"Hello {reservation.FullName},<br><br>" +
                  $"Your table reservation on {reservation.ReservationDateTime:yyyy-MM-dd HH:mm} for {reservation.Guests} guests is now <b>{model.Status}</b>.<br><br>" +
                  "Check out our menu before your visit: " + menuLink + "<br><br>" +
                  "Thank you for choosing our service!";

                        _emailService.SendEmail(reservation.Email, subject, message);
                    }

                    return Json(new { success = true, message = "Status updated and email sent!" });
                }
                else
                {
                    return BadRequest("Failed to update status.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

    }
}
