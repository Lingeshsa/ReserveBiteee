using System;
using System.Data;
using ReserveBiteee.DataBaseHelper;
using ReserveBiteee.Models;
using Microsoft.Data.SqlClient;


namespace ReserveBiteee.Service
{
    public class LoginService : ILoginService
    {
        private readonly DatabaseHelper _dbHelper;
        public LoginService(DatabaseHelper dataBaseHelper)
        {
            _dbHelper = dataBaseHelper;
        }
        public bool FindUser(string username, string password)
        {
            try
            {
                SqlParameter[] parameters =
                {
            new SqlParameter("@UserName", username),
            new SqlParameter("@PasswordHash", password)
        };

                DataTable result = _dbHelper.GetData("sp_FindUser", parameters, CommandType.StoredProcedure);

                return result != null && result.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }
        }

    }
}
