using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportsStore.Models;

namespace SportsStore.Controllers
{
  [Produces("application/json")]
  [Route("api/session")]
  public class SessionValuesController : Controller
  {
    [HttpGet("cart")]
    public IActionResult GetCart()
    {
      return Ok(HttpContext.Session.GetString("cart"));
    }

    [HttpPost("cart")]
    public void PostCart([FromBody] ProductSelection[] products) {
      var json = JsonConvert.SerializeObject(products);
      HttpContext.Session.SetString("cart", json);
    }
  }
}
