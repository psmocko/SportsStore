using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportsStore.Models;
using SportsStore.Models.BindingTargets;

namespace SportsStore.Controllers
{
  [Produces("application/json")]
  [Route("api/session")]
  //[ValidateAntiForgeryToken]
  //[AutoValidateAntiforgeryToken]
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

    [HttpGet("checkout")]
    public IActionResult GetCheckout() {
      return Ok(HttpContext.Session.GetString("checkout"));
    }

    [HttpPost("checkout")]
    public void StoreCheckout([FromBody] CheckoutState data) {
      HttpContext.Session.SetString("checkout", JsonConvert.SerializeObject(data));
    }

  }

}
