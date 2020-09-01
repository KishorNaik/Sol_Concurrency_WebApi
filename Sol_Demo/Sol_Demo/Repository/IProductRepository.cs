using Sol_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sol_Demo.Repository
{
    public interface IProductRepository
    {
        Task<ProductModel> GetProductByIdAsync(decimal? productId);

        Task<dynamic> UpdateProductAsync(ProductModel productModel);
    }
}