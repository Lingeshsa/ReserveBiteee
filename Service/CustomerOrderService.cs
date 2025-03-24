using Microsoft.Data.SqlClient;
using ReserveBiteee.DataBaseHelper;
using ReserveBiteee.Models;
using System.Data;

namespace ReserveBiteee.Service
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private readonly DatabaseHelper _dbHelper;

        public CustomerOrderService(DatabaseHelper dataBaseHelper)
        {
            _dbHelper = dataBaseHelper;
        }
        public bool AddOrder(OrderModel order)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@GrandTotal", order.GrandTotal),
            new SqlParameter("@OrderItems", SqlDbType.Structured)
            {
                TypeName = "OrderItemTableType", 
                Value = ConvertOrderItemsToDataTable(order.Items)
            }
                };

                int result = _dbHelper.InsertUpdateDelete("sp_AddOrder", parameters, CommandType.StoredProcedure);

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddOrder: " + ex.Message);
                return false;
            }
        }
        private DataTable ConvertOrderItemsToDataTable(List<OrderItemModel> items)
        {
            DataTable table = new DataTable();
            table.Columns.Add("MenuId", typeof(int));
            table.Columns.Add("ItemName", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Total", typeof(decimal));

            foreach (var item in items)
            {
                table.Rows.Add(item.MenuId, item.ItemName, item.Quantity, item.Price, item.Total);
            }

            return table;
        }

    }
}
