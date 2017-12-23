using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models.BindingTargets;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace SportsStore.Controllers
{
  [Produces("application/json")]
  [Route("api/products")]
  [Authorize(Roles = "Administrator")]
  //[ValidateAntiForgeryToken]
  //[AutoValidateAntiforgeryToken]
  public class ProductValuesController : Controller
  {
    private readonly DataContext _context;

    public ProductValuesController(DataContext context)
    {
      _context = context;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<Product> GetProduct(long id)
    {
      IQueryable<Product> query = _context.Products
        .Include(p => p.Ratings);

      if (HttpContext.User.IsInRole("Administrator")) {
        query = query.Include(p => p.Supplier)
          .ThenInclude(s => s.Products);
      }

      var result = query.First(p => p.ProductId == id); 

      if (result != null)
      {
        if (result.Supplier != null)
        {
          result.Supplier.Products = result.Supplier.Products.Select(p => new Product
          {
            ProductId = p.ProductId,
            Name = p.Name,
            Category = p.Category,
            Description = p.Description,
            Price = p.Price
          });
        }

        if (result.Ratings != null)
        {
          foreach (var r in result.Ratings)
          {
            r.Product = null;
          }
        }
      }

      return result;

    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetProducts(string category, string search, bool related = false, bool metadata = false)
    {
      IQueryable<Product> query = _context.Products;

      if (!string.IsNullOrWhiteSpace(category))
      {
        var catLower = category.ToLower();
        query = query.Where(p => p.Category.ToLower().Contains(catLower));
      }

      if (!string.IsNullOrWhiteSpace(search))
      {
        var searchLower = search.ToLower();
        query = query.Where(p => p.Name.ToLower().Contains(searchLower) ||
          p.Description.ToLower().Contains(searchLower));
      }

      if (related && HttpContext.User.IsInRole("Administrator"))
      {
        query = query.Include(p => p.Supplier).Include(p => p.Ratings);
        var data = query.ToList();
        data.ForEach(p =>
        {
          if (p.Supplier != null)
            p.Supplier.Products = null;

          if (p.Ratings != null)
            p.Ratings.ForEach(r => r.Product = null);
        });
        return metadata ? CreateMetadata(data) : Ok(data);
      }
      else
      {
        return metadata ? CreateMetadata(query) : Ok(query);
      }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductData pdata)
    {
      if (ModelState.IsValid)
      {
        var p = pdata.Product;
        if (p.Supplier != null && p.Supplier.SupplierId != 0)
        {
          _context.Attach(p.Supplier);
        }
        await _context.AddAsync(p);
        await _context.SaveChangesAsync();

        return Ok(p.ProductId);
      }

      return BadRequest(ModelState);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceProduct(long id, [FromBody] ProductData pdata)
    {
      if (ModelState.IsValid)
      {
        var p = pdata.Product;
        p.ProductId = id;
        if (p.Supplier != null && p.Supplier.SupplierId != 0)
        {
          _context.Attach(p.Supplier);
        }
        _context.Update(p);
        await _context.SaveChangesAsync();
        return Ok();
      }

      return BadRequest(ModelState);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateProduct(long id, [FromBody]JsonPatchDocument<ProductData> patch)
    {
      var product = _context.Products
        .Include(p => p.Supplier)
        .First(p => p.ProductId == id);
      var pdata = new ProductData { Product = product };
      patch.ApplyTo(pdata, ModelState);

      if (ModelState.IsValid && TryValidateModel(pdata))
      {
        if (product.Supplier != null && product.Supplier.SupplierId != 0)
        {
          _context.Attach(product.Supplier);
        }
        await _context.SaveChangesAsync();
        return Ok();
      }
      else
      {
        return BadRequest(ModelState);
      }
    }

    [HttpDelete("{id}")]
    public async Task DeleteProduct(long id)
    {
      _context.Remove(new Product { ProductId = id });
      await _context.SaveChangesAsync();
    }

    private IActionResult CreateMetadata(IEnumerable<Product> products)
    {
      return Ok(new
      {
        data = products,
        categories = _context.Products.Select(p => p.Category).Distinct().OrderBy(c => c)
      });
    }

  }

}
