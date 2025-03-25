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
            new SqlParameter("@Requests", (object)model.Requests ?? DBNull.Value),
            new SqlParameter("@Categorie", model.Categorie),
            new SqlParameter("@TableId", model.TableId)
        };

                return _dbHelper.InsertUpdateDelete("sp_AddReservation", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }
        }

        public List<ReservationModel> GetAllReservations()
        {
            List<ReservationModel> reservations = new List<ReservationModel>();

            try
            {
                DataTable result = _dbHelper.GetData("sp_GetAllReservations", null, CommandType.StoredProcedure);

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        reservations.Add(new ReservationModel
                        {
                            Id = row["Id"] != DBNull.Value ? Convert.ToInt32(row["Id"]) : 0,
                            FullName = row["FullName"]?.ToString() ?? "N/A",
                            Email = row["Email"]?.ToString() ?? "N/A",
                            Phone = row["Phone"]?.ToString() ?? "N/A",
                            ReservationDateTime = row["ReservationDateTime"] != DBNull.Value
                                ? DateTime.SpecifyKind(Convert.ToDateTime(row["ReservationDateTime"]), DateTimeKind.Utc).ToLocalTime()
                                : DateTime.MinValue,
                            Guests = row["Guests"] != DBNull.Value ? Convert.ToInt32(row["Guests"]) : 1,
                            Requests = row.IsNull("Requests") ? null : row["Requests"].ToString(),
                            Status = row["Status"]?.ToString() ?? "Unknown",
                            Categorie = row["Categorie"]?.ToString() ?? "Unspecified",  // Fetching Category
                            TableNumber = row["TableNumber"]?.ToString() ?? "Unknown"  // Fetching Table Number
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to fetch reservations: {ex.Message}");
                throw new Exception("An error occurred while fetching reservations. Please try again later.");
            }

            return reservations;
        }


        public List<Tables> GetAvailableTables(string categorie, DateTime reservationDateTime)
        {
            List<Tables> tables = new List<Tables>();

            try
            {
                SqlParameter[] parameters =
                {
            new SqlParameter("@Categorie", categorie),
            new SqlParameter("@RequestedDateTime", reservationDateTime)
        };

                DataTable dt = _dbHelper.GetData("sp_GetAvailableTables", parameters, CommandType.StoredProcedure);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        tables.Add(new Tables
                        {
                            TableId = Convert.ToInt32(row["TableId"]),
                            TableNumber = row["TableNumber"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }

            return tables;
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
