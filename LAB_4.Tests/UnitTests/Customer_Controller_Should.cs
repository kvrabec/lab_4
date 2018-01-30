using LAB_4.Controllers;
using LAB4.MODEL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LAB_4.Tests.UnitTests
{
    public class Customer_Controller_Should
    {
        DbContextOptions<StoreDbContext> _dbContextOptions;

        public Customer_Controller_Should()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StoreDbContext>()
                            .UseInMemoryDatabase(databaseName: "Test_database")
                            .Options;
        }

        // Testiranje dodavanja novih dobavljaca u bazu podataka
        [Fact]
        public async void PostCustomer()
        {
            using (var context = new StoreDbContext(_dbContextOptions))
            {
                var customerAPI = new CustomersController(context);
                for (int i = 0; i < 10; ++i)
                {
                    Customer customer = new Customer()
                    {
                        AccountNumber = "AW0001130" + i,
                        PersonId = 1,
                        StoreId = 292,
                        TerritoryId = 1
                    };
                    var result = await customerAPI.PostCustomer(customer);
                    var badRequest = result as BadRequestObjectResult;

                    Assert.Null(badRequest);   
                }
            }
        }

        [Fact]
        public async void GetCustomer()
        {
            using (var context = new StoreDbContext(_dbContextOptions))
            {
                var customerAPI = new CustomersController(context);
                for (int i = 0; i < 10; ++i)
                {
                    Customer customer = new Customer()
                    {
                        AccountNumber = "AW0001130" + i,
                        PersonId = 1,
                        StoreId = 292,
                        TerritoryId = 1
                    };
                    customerAPI.PostCustomer(customer).Wait();
                }
            }
            using (var context = new StoreDbContext(_dbContextOptions))
            {
                var customerAPI = new CustomersController(context);
                var result = await customerAPI.GetCustomer(7);
                var okResult = result as OkObjectResult;

                Assert.NotNull(okResult);
                Assert.Equal(200, okResult.StatusCode);

                Customer customer = okResult.Value as Customer;
                Assert.NotNull(customer);
                Assert.Equal("AW00011306", customer.AccountNumber);
            }
        }

        [Fact]
        public async void DeleteCustomer()
        {
            using (var context = new StoreDbContext(_dbContextOptions))
            {
                var customerAPI = new CustomersController(context);
                Customer customer = new Customer()
                {
                    AccountNumber = "AW00011300",
                    PersonId = 1,
                    StoreId = 292,
                    TerritoryId = 1
                };
                    customerAPI.PostCustomer(customer).Wait();
            }
            using (var context = new StoreDbContext(_dbContextOptions))
            {
                var customerAPI = new CustomersController(context);
                var result = await customerAPI.DeleteCustomer(1);
                var okResult = result as OkObjectResult;

                Assert.NotNull(okResult);
                Assert.Equal(200, okResult.StatusCode);

                Customer customer = okResult.Value as Customer;
                Assert.NotNull(customer);
                Assert.Equal("AW00011300", customer.AccountNumber);
            }

            using (var context = new StoreDbContext(_dbContextOptions))
            {
                var customerAPI = new CustomersController(context);
                var result = await customerAPI.DeleteCustomer(1);
                var notFoundResult = result as NotFoundResult;

                Assert.NotNull(notFoundResult);
                Assert.Equal(404, notFoundResult.StatusCode);
            }

        }
    }
}
