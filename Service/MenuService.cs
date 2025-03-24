using Microsoft.Data.SqlClient;
using ReserveBiteee.DataBaseHelper;
using ReserveBiteee.Models;
using System.Data;

namespace ReserveBiteee.Service
{
    public class MenuService : IMenuService
    {

        private readonly DatabaseHelper _dbHelper;

        public MenuService(DatabaseHelper dataBaseHelper)
        {
            _dbHelper = dataBaseHelper;
        }

        public int AddMenu(MenuModel menu)
        {
            try
            {
                SqlParameter[] parameters =
                {
            new SqlParameter("@MenuItem", menu.MenuItem),
            new SqlParameter("@Amount", menu.Amount)
                };

                return _dbHelper.InsertUpdateDelete("sp_AddMenu", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return -1; 
            }
        }
        public List<MenuModel> GetMenus()
        {
            List<MenuModel> menuList = new List<MenuModel>();

            try
            {
                SqlParameter[] parameters = { };
                DataTable dt = _dbHelper.GetData("sp_GetAllMenus", parameters, CommandType.StoredProcedure);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        menuList.Add(new MenuModel
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            MenuItem = row["MenuItem"].ToString(),
                            Amount = row["Amount"] != DBNull.Value ? Convert.ToInt32(row["Amount"]) : 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }

            return menuList;
        }
        public int UpdateMenu(MenuModel menu)
        {
            SqlParameter[] parameters =
            {
            new SqlParameter("@Id", menu.Id),
            new SqlParameter("@MenuItem", menu.MenuItem),
            new SqlParameter("Amount",menu.Amount)
        };
            return _dbHelper.InsertUpdateDelete("sp_UpdateMenu", parameters, CommandType.StoredProcedure);
        }
        public int DeleteMenu(int id)
        {
            SqlParameter[] parameters =
            {
            new SqlParameter("@Id", id)
        };
            return _dbHelper.InsertUpdateDelete("sp_DeleteMenu", parameters, CommandType.StoredProcedure);
        }
    }
}

