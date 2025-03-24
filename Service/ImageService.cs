using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using ReserveBiteee.DataBaseHelper;
using ReserveBiteee.Models;
using System.Data;

namespace ReserveBiteee.Service
{
    public class ImageService : IImageService
    {
        private readonly DatabaseHelper _dbHelper;

        public ImageService(DatabaseHelper dataBaseHelper)
        {
            _dbHelper = dataBaseHelper;
        }

        public List<CustomerPageImages> GetImages()
        {
            List<CustomerPageImages> images = new List<CustomerPageImages>();

            try
            {
                DataTable result = _dbHelper.GetData("sp_GetImages", null, CommandType.StoredProcedure);
                foreach (DataRow row in result.Rows)
                {
                    images.Add(new CustomerPageImages
                    {
                        Name = row["Name"].ToString(),
                        Description = row["Description"].ToString(),
                        ImageBase64 = row["ImageBase64"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }

            return images;
        }

        public int UploadImage(CustomerPageImages image)
        {
            try
            {
                SqlParameter[] parameters =
                {
                new SqlParameter("@Name", image.Name),
                new SqlParameter("@Description", image.Description),
                new SqlParameter("@ImageBase64", image.ImageBase64)
            };

                return _dbHelper.InsertUpdateDelete("sp_UploadImage", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }
        }

    }
}

