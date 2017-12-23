using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SportsStore.Controllers
{
  [Produces("application/json")]
  [Route("api/orders")]
  [Authorize(Roles = "Administrator")]
  //[ValidateAntiForgeryToken]
  //[AutoValidateAntiforgeryToken]
  public class OrderValuesController : Controller
  {
    private DataContext _context;

    public OrderValuesController(DataContext ctx)
    {
      _context = ctx;
    }

    [HttpGet]
    public IEnumerable<Order> GetOrders() {
      return _context.Orders
        .Include(o => o.Products)
        .Include(o => o.Payment);
    }

    [HttpPost("{id}")]
    public async Task MarkShipped(long id) {
      var order = await _context.Orders.FindAsync(id);
      if (order != null) {
        order.Shipped = true;
        await _context.SaveChangesAsync();
      }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateOrder([FromBody] Order order) {
      if (ModelState.IsValid) {
        order.OrderId = 0;
        order.Shipped = false;
        order.Payment.Total = GetPrice(order.Products);

        ProcessPayment(order.Payment);
        if (order.Payment.AuthCode != null)
        {
          await _context.AddAsync(order);
          await _context.SaveChangesAsync();
          return Ok(new { orderId = order.OrderId, authCode = order.Payment.AuthCode, amount = order.Payment.Total });
        }
        else {
          return BadRequest("Payment rejected.");
        }
      }

      return BadRequest(ModelState);
    }

    private void ProcessPayment(Payment payment)
    {
      // TODO : integrate payment system here
      payment.AuthCode = "12345";
    }

    private decimal GetPrice(IEnumerable<CartLine> lines)
    {
      var ids = lines.Select(l => l.ProductId);
      return _context.Products.Where(p => ids.Contains(p.ProductId))
        .Select(p => lines.First(l => l.ProductId == p.ProductId).Quantity * p.Price)
        .Sum();
    }
  }
}
