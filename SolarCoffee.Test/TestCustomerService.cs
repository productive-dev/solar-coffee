using System.Collections.Generic;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Customer;
using System.Linq;
using Moq;
using Xunit;

namespace SolarCoffee.Test {
    public class TestCustomerService {

        [Fact]
        public void CustomerService_GetsAllCustomers_GivenTheyExist() {
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase("gets_all").Options;

            using var context = new SolarDbContext(options);

            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer {Id = 123123});
            sut.CreateCustomer(new Customer {Id = -213});

            var allCustomers = sut.GetAllCustomers();

            allCustomers.Count.Should().Be(2);
        }
        
        [Fact]
        public void CustomerService_CreatesCustomer_GivenNewCustomerObject() {
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase("Add_writes_to_database").Options;

            using var context = new SolarDbContext(options);
            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer { Id = 18645 });
            context.Customers.Single().Id.Should().Be(18645);
        }
        
        [Fact]
        public void CustomerService_DeletesCustomer_GivenId() {
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase("deletes_one")
                .Options;

            using var context = new SolarDbContext(options);
            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer { Id = 18645 });

            sut.DeleteCustomer(18645);
            var allCustomers = sut.GetAllCustomers();
            allCustomers.Count.Should().Be(0);
        }
        
        [Fact]
        public void CustomerService_OrdersByLastName_WhenGetAllCustomersInvoked() {
            // Arrange
            var data = new List<Customer> {
                new Customer { Id = 123, LastName = "Zulu"},
                new Customer { Id = 323, LastName = "Alpha"},
                new Customer { Id = -89, LastName = "Xu"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();
            
            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Provider)
                .Returns(data.Provider);
            
            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);
            
            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);
            
            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());
            
            var mockContext = new Mock<SolarDbContext>();
            
            mockContext.Setup(c => c.Customers)
                .Returns(mockSet.Object);
           
            // Act
            var sut = new CustomerService(mockContext.Object);
            var customers = sut.GetAllCustomers();

            // Assert
            customers.Count.Should().Be(3);
            customers[0].Id.Should().Be(323);
            customers[1].Id.Should().Be(-89);
            customers[2].Id.Should().Be(123);
        }
    }
}