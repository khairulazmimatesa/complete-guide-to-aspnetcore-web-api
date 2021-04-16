using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using my_books.Controllers;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
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

        [Test,Order(1)]
        public void HTTPGET_GetAllPublishers_WithSortBySearchStringPageNumber_ReturnOk_Test() {

            //First Page
            IActionResult actionResult = publishersController.GetAllPublishers("name_desc","publisher",1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultData = (actionResult as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResultData.First().Name, Is.EqualTo("Publisher 6"));
            Assert.That(actionResultData.First().Id, Is.EqualTo(6));
            Assert.That(actionResultData.Count, Is.EqualTo(5));

            //Second Page
            IActionResult actionResultSecondPage = publishersController.GetAllPublishers("name_desc","publisher",2);
            Assert.That(actionResultSecondPage, Is.TypeOf<OkObjectResult>());
            var actionResultSecondPageData = (actionResultSecondPage as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResultSecondPageData.First().Name, Is.EqualTo("Publisher 1"));
            Assert.That(actionResultSecondPageData.First().Id, Is.EqualTo(1));
            Assert.That(actionResultSecondPageData.Count, Is.EqualTo(1));
        }

        [Test,Order(2)]
        public void HTTPGET_GetPublisherById_ReturnOk_Test() {

            IActionResult actionResult = publishersController.GetPublisherById(1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultData = (actionResult as OkObjectResult).Value as Publisher;
            Assert.That(actionResultData.Name, Is.EqualTo("publisher 1").IgnoreCase);
            Assert.That(actionResultData.Id, Is.EqualTo(1));

        }
        [Test,Order(3)]
        public void HTTPGET_GetPublisherById_ReturnNotFound_Test() {

            IActionResult actionResult = publishersController.GetPublisherById(99);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }
        [Test,Order(4)]//Decorator
        public void HTTPPOST_AddPublisher_ReturnCreated_Test() {
            var newPublisherVM = new PublisherVM() {
                Name = "New Publisher"
            };

            IActionResult actionResult = publishersController.AddPublisher(newPublisherVM);

            Assert.That(actionResult, Is.TypeOf<CreatedResult>());

        }
        [Test,Order(5)]//Decorator
        public void HTTPPOST_AddPublisher_ReturnBadRequest_Test() {
            var newPublisherVM = new PublisherVM() {
                Name = "123 New Publisher"
            };

            IActionResult actionResult = publishersController.AddPublisher(newPublisherVM);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());

        }

        [Test,Order(6)]//Decorator
        public void HTTPPOST_DeletePublisherById_ReturnOk_Test() {
            int publisherId = 6;

            IActionResult actionResult = publishersController.DeletePublisherById(publisherId);

            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }
        [Test,Order(7)]//Decorator
        public void HTTPPOST_DeletePublisherById_ReturnBadRequent_Test() {
            int publisherId = 6;

            IActionResult actionResult = publishersController.DeletePublisherById(publisherId);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
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
