using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sol_Demo.Models;
using Sol_Demo.Repository;

namespace Sol_Demo.Controllers
{
    [Produces("application/json")]
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository = null;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpPost("getbyid")]
        public async Task<IActionResult> GetProductByIdAsync([FromBody] ProductModel productModel)
        {
            try
            {
                var data = await productRepository?.GetProductByIdAsync(productModel.ProductId);

                if (data == null) return base.NotFound();

                return Ok(data);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] ProductModel productModel)
        {
            try
            {
                var data = await productRepository?.UpdateProductAsync(productModel);

                if (data is string response)
                {
                    if (response == "Not Found") return base.NotFound();

                    if (response == "Conflict") return base.Conflict();
                }
                else
                {
                    return base.Ok((object)data);
                }
            }
            catch
            {
                throw;
            }

            return null;
        }
    }
}