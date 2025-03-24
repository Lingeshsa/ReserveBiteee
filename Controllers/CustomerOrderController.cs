using Microsoft.AspNetCore.Mvc;
using ReserveBiteee.Models;
using ReserveBiteee.Service;

namespace ReserveBiteee.Controllers
{
    public class CustomerOrderController: Controller
    {
        private readonly ICustomerOrderService _customerOrderService;

        public CustomerOrderController(ICustomerOrderService customerOrderService)
        {
            _customerOrderService = customerOrderService;
        }
        [HttpPost]
        public IActionResult AddOrder([FromBody] OrderModel order)
        {

            if (order == null || order.Items == null || !order.Items.Any())
                return BadRequest("Invalid order data");

            var result = _customerOrderService.AddOrder(order);
            if (result)
                return Ok(new { Message = "Order placed successfully!" });
            else
                return StatusCode(500, "Error processing the order");

        }
    }
}
