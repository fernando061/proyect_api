using Api.Helpers;
using Api.Services;
using API.Controllers;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.UnitOfWorck;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Text;

namespace Api.Controllers;
public class ProductController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("import")]
    public async Task<ActionResult> Import(IFormFile fileProduct)
    {
        var extencion = Path.GetExtension(fileProduct.FileName);
        if (extencion.ToLower() != ".csv") return BadRequest(new {message="Solo se admiten archivos csv"});
        try
        {
            IO iO = new IO();
            List<Product> products = new List<Product>();
            DataTable dataTable = iO.convertDataTable(fileProduct);
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {

                Product product = new Product();
                product.Name = dataTable.Rows[i]["Name"].ToString();
                decimal price;
                if ((Decimal.TryParse(dataTable.Rows[i]["Price"].ToString(), out price)))
                    product.Price = price;
                else
                    product.Price = price;
                int amount;
                if ((int.TryParse(dataTable.Rows[i]["Amount"].ToString(), out amount)))
                    product.Amount = amount;
                else
                    product.Amount = amount;
                products.Add(product);
            }
            _unitOfWork.Product.AddRangeAsync(products);
            await _unitOfWork.SaveAsync();
        }
        catch(Exception ex) 
        {
            return BadRequest(new {mewssage= ex.Message });
        }

       
        return StatusCode(StatusCodes.Status201Created, new { message = "Productos registrados" });
    }

   
    // POST: ProductController/Create
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Create(IFormCollection collection)
    //{
    //    try
    //    {
    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

  
}
