using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.BindingTargets;

namespace SportsStore.Controllers
{
  [Produces("application/json")]
  [Route("api/suppliers")]
  public class SupplierValuesController : Controller
  {
    private readonly DataContext _context;

    public SupplierValuesController(DataContext ctx)
    {
      _context = ctx;
    }

    [HttpGet]
    public IEnumerable<Supplier> GetSuppliers () {
      return _context.Suppliers;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody] SupplierData sdata) {
      if (ModelState.IsValid) {
        Supplier s = sdata.Supplier;
        await _context.AddAsync(s);
        await _context.SaveChangesAsync();
        return Ok(s.SupplierId);
      }

      return BadRequest(ModelState);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceSupplier(long id, [FromBody] SupplierData sdata) {
      if (ModelState.IsValid) {
        var s = sdata.Supplier;
        s.SupplierId = id;
        _context.Update(s);
        await _context.SaveChangesAsync();
        return Ok();
      }

      return BadRequest(ModelState);

    }

    [HttpDelete("{id}")]
    public async Task DeleteSupplier(long id) {
      _context.Remove(new Supplier { SupplierId = id });
      await _context.SaveChangesAsync();
    }

  }
}
