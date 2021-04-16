using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using my_books.Controllers;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_book_tests {
    public class PublishersControllerTest {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbControllerTest")
            .Options;

        AppDbContext context;
        PublishersService publisherService;
        PublishersController publishersController;
        //Method to put all the code before run unit test
        [OneTimeSetUp]//decorator
        public void Setup() {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();

            publisherService = new PublishersService(context);
            publishersController = new PublishersController(publisherService, new NullLogger<PublishersController>());
        }

        [Test]
        public void HTTPGET_GetAllPublishers_WithSortBySearchStringPageNumber_ReturnOk_Test() {
            IActionResult actionResult = publishersController.GetAllPublishers("name_desc","publisher",1);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var actionResultData = (actionResult as OkObjectResult).Value as List<Publisher>;

            Assert.That(actionResultData.First().Name, Is.EqualTo("Publisher 6"));
            Assert.That(actionResultData.First().Id, Is.EqualTo(6));
            Assert.That(actionResultData.Count, Is.EqualTo(5));
            
            IActionResult actionResult = publishersController.GetAllPublishers("name_desc","publisher",1);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var actionResultData = (actionResult as OkObjectResult).Value as List<Publisher>;

            Assert.That(actionResultData.First().Name, Is.EqualTo("Publisher 6"));
            Assert.That(actionResultData.First().Id, Is.EqualTo(6));
            Assert.That(actionResultData.Count, Is.EqualTo(5));
        }



        [OneTimeTearDown]
        public void CleanUp() {
            context.Database.EnsureDeleted();
        }

        private void SeedDatabase() {
            var publishers = new List<Publisher>() {
                new Publisher() {
                    Id=1,
                    Name="Publisher 1"
                },new Publisher() {
                    Id=2,
                    Name="Publisher 2"
                },new Publisher() {
                    Id=3,
                    Name="Publisher 3"
                },new Publisher() {
                    Id=4,
                    Name="Publisher 4"
                },new Publisher() {
                    Id=5,
                    Name="Publisher 5"
                },new Publisher() {
                    Id=6,
                    Name="Publisher 6"
                },
            };
            context.Publishers.AddRange(publishers);

            context.SaveChanges();
        }

    }
}
