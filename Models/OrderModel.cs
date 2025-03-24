namespace ReserveBiteee.Models
{
    public class OrderModel
    {
        public decimal GrandTotal { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }
}
