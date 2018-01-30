using LAB4.MODEL.Entities;
using System;
using Xunit;

namespace LAB_4.Tests
{
    public class Customer_Should
    {
        private Customer customer;
        public Customer_Should()
        {
            customer = new Customer()
            {
                AccountNumber = "AW00011300",
                PersonId = 1,
                StoreId = 292,
                TerritoryId = 1
            };
        }
        [Fact]
        public void PersonTestHasValue()
        {
            Assert.NotNull(customer);
            Assert.Equal("AW00011300", customer.AccountNumber);
        }
    }
}
