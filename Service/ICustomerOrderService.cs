using Microsoft.AspNetCore.Mvc;
using ReserveBiteee.Models;

namespace ReserveBiteee.Service
{
    public interface ICustomerOrderService
    {
        bool AddOrder(OrderModel order);
    }
}
