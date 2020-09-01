using Dapper;
using Microsoft.Extensions.Configuration;
using Sol_Demo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sol_Demo.Repository
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly IConfiguration configuration = null;

        public ProductRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private async Task<ProductModel> GetProductByIdAsync(decimal? productId, SqlConnection connection)
        {
            // Dynamic Prameter
            var dynamicParameterObj = new DynamicParameters();
            dynamicParameterObj.Add("@ProductId", productId, direction: ParameterDirection.Input);

            // Query
            var query = "SELECT * FROM tblProducts WHERE ProductId=@ProductId";

            // get Single Product based on product Id
            var productData = await connection.QueryFirstOrDefaultAsync<ProductModel>(query, param: dynamicParameterObj, commandType: CommandType.Text);

            return productData;
        }

        async Task<ProductModel> IProductRepository.GetProductByIdAsync(decimal? productId)
        {
            try
            {
                using (var connection = new SqlConnection(configuration?.GetConnectionString("DefaultConnection")))
                {
                    // Open Connection
                    await connection?.OpenAsync();

                    return await GetProductByIdAsync(productId, connection);
                };
            }
            catch
            {
                throw;
            }
        }

        async Task<dynamic> IProductRepository.UpdateProductAsync(ProductModel productModel)
        {
            try
            {
                using (var connection = new SqlConnection(configuration?.GetConnectionString("DefaultConnection")))
                {
                    // Open Connection
                    await connection?.OpenAsync();

                    var productData = await GetProductByIdAsync(productModel?.ProductId, connection);

                    if (productData == null) return "Not Found";

                    if (Convert.ToBase64String(productData.Version) != Convert.ToBase64String(productModel.Version)) return "Conflict";

                    // dynamic Parameter
                    var dynamicParameter = new DynamicParameters();
                    dynamicParameter.Add("@ProductId", productModel?.ProductId, direction: ParameterDirection.Input);
                    dynamicParameter.Add("@Name", productModel?.Name, direction: ParameterDirection.Input);
                    dynamicParameter.Add("@UnitPrice", productModel?.UnitPrice, direction: ParameterDirection.Input);
                    dynamicParameter.Add("@Version", productModel?.Version, direction: ParameterDirection.Input);

                    // Query
                    var query = new StringBuilder()
                                    .Append("UPDATE tblProducts ")
                                    .Append("SET ")
                                    .Append("Name=@Name,")
                                    .Append("UnitPrice=@UnitPrice ")
                                    .Append("WHERE ")
                                    .Append("ProductId=@ProductId ")
                                    .Append("AND ")
                                    .Append("Version=@Version")
                                    .ToString();

                    var rowUpdate = await connection.ExecuteAsync(query, param: dynamicParameter, commandType: CommandType.Text);

                    return (rowUpdate != 1) ? (dynamic)"Conflict" : (dynamic)true;
                };
            }
            catch
            {
                throw;
            }
        }
    }
}