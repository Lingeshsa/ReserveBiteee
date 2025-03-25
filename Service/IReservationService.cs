using ReserveBiteee.Models;

namespace ReserveBiteee.Service
{
    public interface IReservationService
    {

        int BookTable(ReservationModel model);
        bool UpdateReservationStatus(int reservationId, string status);
        List<ReservationModel> GetAllReservations();

        ReservationModel GetReservationById(int reservationId);

        List<Tables> GetAvailableTables(string categorie, DateTime reservationDateTime);

    }
}
