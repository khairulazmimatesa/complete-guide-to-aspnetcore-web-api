using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace my_book_tests {
    public class PublishersServiceTest {
        //Setup Unit testing In-Memory DB
        private static DbContextOptions<AppDbContext>  dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName:"BookDbTest")
            .Options;

        AppDbContext context;
        PublishersService publisherService;
        //Method to put all the code before run unit test
        [OneTimeSetUp]//decorator
        public void Setup() {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();

            publisherService = new PublishersService(context);
        }

        [Test, Order(1)]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithNoPageNumber_Test() {
            var result = publisherService.GetAllPublishers("","",null);

            Assert.That(result.Count, Is.EqualTo(5));
        }
        [Test, Order(2)]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithPageNumber_Test() {
            var result = publisherService.GetAllPublishers("","",2);

            Assert.That(result.Count, Is.EqualTo(1));
        }
         
        [Test, Order(3)]
        public void GetAllPublishers_WithNoSortBy_WithSearchString_WithNoPageNumber_Test() {
            var result = publisherService.GetAllPublishers("","3",null);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher 3"));
        }
        [Test, Order(4)]
        public void GetAllPublishers_WithSortBy_WithNoSearchString_WithNoPageNumber_Test() {
            var result = publisherService.GetAllPublishers("name_desc", "",null);

            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher 6"));

        }
        [Test, Order(5)]
        public void GetPublisherById_WithResponse_Test() {
            var result = publisherService.GetPublisherById(1);

            Assert.That(result.Name, Is.EqualTo("Publisher 1"));
            Assert.That(result.Id, Is.EqualTo(1));

        }
        [Test, Order(6)]
        public void GetPublisherById_WithoutResponse_Test() {
            var result = publisherService.GetPublisherById(99);

            Assert.That(result, Is.Null);

        }
        [Test, Order(7)]
        public void AddPublisher_WithException_Test() {
            var model = new PublisherVM() {
                Name = "123 With Exception"
            };

            Assert.That(() => publisherService.AddPublisher(model), 
                Throws.Exception.TypeOf<PublisherNameException>().With.Message.EqualTo("Name starts with number"));
        }

        [Test, Order(8)]
        public void AddPublisher_WithoutException_Test() {
            var model = new PublisherVM() {
                Name = "Without Exception"
            };

            var result = publisherService.AddPublisher(model);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Does.StartWith("Without"));
            Assert.That(result.Id, Is.Not.Null);

        }
        [Test, Order(9)]
        public void GetPublisherData_Test() {
            var result = publisherService.GetPublisherData(1);

            Assert.That(result.Name, Is.EqualTo("Publisher 1"));
            Assert.That(result.BookAuthors, Is.Not.Empty);
            Assert.That(result.BookAuthors.Count, Is.GreaterThan(0));

            var firstBookName = result.BookAuthors.OrderBy(t => t.BookName).FirstOrDefault().BookName;
            Assert.That(firstBookName, Is.EqualTo("Book 1 Title"));


        }
        [Test,Order(10)]
        public void DeletePublisherById_WithException() {
           
            Assert.That(() => publisherService.DeletePublisherById(99),
                        Throws.Exception.TypeOf<PublisherNameException>().With.Message.EqualTo("The Publisher with id"));

        }
        [Test,Order(11)]
        public void DeletePublisherById_WithoutException() {

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

            var authors = new List<Author>() {
                new Author(){
                    Id=1,
                    FullName="Author 1"
                },new Author(){
                    Id=2,
                    FullName="Author 2"
                },new Author(){
                    Id=3,
                    FullName="Author 3"
                },
            };
            context.Authors.AddRange(authors);

            var books = new List<Book>() {
                new Book() {
                    Id=1,
                    Title="Book 1 Title",
                    Description ="book 1 Description",
                    IsRead=false,
                    Genre="genre",
                    CoverURL="https://...",
                    DateAdded=DateTime.Now.AddDays(-10),
                    PublisherId=1
                },new Book() {
                    Id=2,
                    Title="Book 2 Title",
                    Description ="book 2 Description",
                    IsRead=false,
                    Genre="genre",
                    CoverURL="https://...",
                    DateAdded=DateTime.Now.AddDays(-10),
                    PublisherId=1
                },
            };
            context.Books.AddRange(books);

            var book_authors = new List<Book_Author>() {
                new Book_Author() {
                    Id=1,
                    BookId=1,
                    AuthorId=1
                },new Book_Author() {
                    Id=2,
                    BookId=1,
                    AuthorId=2
                },new Book_Author() {
                    Id=3,
                    BookId=2,
                    AuthorId=2
                },new Book_Author() {
                    Id=4,
                    BookId=2,
                    AuthorId=3
                },

            };
            context.Books_Authors.AddRange(book_authors);

            context.SaveChanges();
        }
    }
}