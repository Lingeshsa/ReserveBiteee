namespace ReserveBiteee.Models
{
    public class OrderItemModel
    {
        public int MenuId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
