using Microsoft.Data.SqlClient;
using ReserveBiteee.DataBaseHelper;
using ReserveBiteee.Models;
using System.Data;

namespace ReserveBiteee.Service
{
    public class ReservationService : IReservationService
    {

        private readonly DatabaseHelper _dbHelper;
        public ReservationService(DatabaseHelper dataBaseHelper)
        {
            _dbHelper = dataBaseHelper;
        }
        public int BookTable(ReservationModel model)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@FullName", model.FullName),
                    new SqlParameter("@Email", model.Email),
                    new SqlParameter("@Phone", model.Phone),
                    new SqlParameter("@ReservationDateTime", model.ReservationDateTime),
                    new SqlParameter("@Guests", model.Guests),
                    new SqlParameter("@Requests", model.Requests ?? (object)DBNull.Value),
                    new SqlParameter("@Status", model.Status)
                };

                return _dbHelper.InsertUpdateDelete("sp_InsertReservation", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }
        }
        public List<ReservationModel> GetAllReservations()
        {
            try
            {
                DataTable result = _dbHelper.GetData("sp_GetAllReservations", null, CommandType.StoredProcedure);
                List<ReservationModel> reservations = new List<ReservationModel>();

                foreach (DataRow row in result.Rows)
                {
                    reservations.Add(new ReservationModel
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        FullName = row["FullName"].ToString(),
                        Email = row["Email"].ToString(),
                        Phone = row["Phone"].ToString(),
                        ReservationDateTime = DateTime.SpecifyKind(Convert.ToDateTime(row["ReservationDateTime"]), DateTimeKind.Utc).ToLocalTime(),
                        Guests = Convert.ToInt32(row["Guests"]),
                        Requests = row.IsNull("Requests") ? null : row["Requests"].ToString(),
                        Status = row["Status"].ToString()
                    });
                }

                return reservations;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }
        }
        public ReservationModel GetReservationById(int reservationId)
        {
            try
            {
                SqlParameter[] parameters =
                {
            new SqlParameter("@Id", reservationId)
        };

                DataTable result = _dbHelper.GetData("sp_GetReservationById", parameters, CommandType.StoredProcedure);

                if (result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[0];

                    return new ReservationModel
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        FullName = row["FullName"].ToString(),
                        Email = row["Email"].ToString(),
                        Phone = row["Phone"].ToString(),
                        ReservationDateTime = Convert.ToDateTime(row["ReservationDateTime"]),
                        Guests = Convert.ToInt32(row["Guests"]),
                        Requests = row["Requests"] as string,
                        Status = row["Status"].ToString()
                    };
                }

                return null; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }
        }
        public bool UpdateReservationStatus(int reservationId, string status)
        {
            try
            {
                SqlParameter[] parameters =
                {
            new SqlParameter("@Id", reservationId),
            new SqlParameter("@Status", status)
        };

                int rowsAffected = _dbHelper.InsertUpdateDelete("sp_UpdateReservationStatus", parameters, CommandType.StoredProcedure);
                return rowsAffected > 0 || rowsAffected == 0; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }
        }
       
    }
}
