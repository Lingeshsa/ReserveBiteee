using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            //if (!ModelState.IsValid)
            //{
            //    return View("Index", model);
            //}

            try
            {
                
                model.ReservationDateTime = model.ReservationDateTime.ToLocalTime();

               
                int reservationId = _reservationService.BookTable(model);

                if (reservationId > 0 || reservationId == 0)
                {
                    TempData["SuccessMessage"] = "Reservation submitted successfully! Waiting for admin confirmation.";
                    return RedirectToAction("SuccessPage");
                }
                else
                {
                    TempData["ErrorMessage"] = "Reservation failed. The selected table might be unavailable. Try again!";
                    return View("Index", model);
                }
            }
            catch (SqlException ex)
            {
                TempData["ErrorMessage"] = "Database error: " + ex.Message;
                return View("BookingPage", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
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


        [HttpGet]
        public IActionResult GetAvailableTables(string categorie, DateTime reservationDateTime)
        {
            var tables = _reservationService.GetAvailableTables(categorie, reservationDateTime);

            if (tables == null || !tables.Any())
            {
                return Json(new { error = "No tables available." });
            }

            return Json(tables.Select(t => new
            {
                TableId = t.TableId,
                TableNumber = t.TableNumber
            }));
        }



    }
}

