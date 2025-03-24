using Microsoft.AspNetCore.Mvc;
using ReserveBiteee.Models;
using ReserveBiteee.DataBaseHelper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ReserveBiteee.Service
{
    public class RegisterService: IRegisterService
    {
        private readonly DatabaseHelper _dbHelper;
        public RegisterService(DatabaseHelper dataBaseHelper)
        {
            _dbHelper = dataBaseHelper;
        }
        public int Register(Register user)
        {

            try
            {;
                SqlParameter[] parameters =
                {
                    new SqlParameter("@UserName", user.UserName),
                    new SqlParameter("PasswordHash", user.Password),
                    new SqlParameter("Email",user.Email)
                };

              
                int result = _dbHelper.InsertUpdateDelete("sp_RegisterUser", parameters, CommandType.StoredProcedure);

                return result;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }   
        }
    }
}
