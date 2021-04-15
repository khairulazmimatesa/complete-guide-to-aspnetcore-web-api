using my_books.Data.Models;
using my_books.Data.Paging;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace my_books.Data.Services {
    public class PublishersService {

        private AppDbContext _context;
        public PublishersService(AppDbContext context) {
            _context = context;

        }

        public Publisher AddPublisher(PublisherVM publisher) {
            if (StringStartWithNumber(publisher.Name)) throw new PublisherNameException("Name starts with number", publisher.Name);
            var _publisher = new Publisher() {
                Name = publisher.Name
            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();

            return _publisher;
        }

        public List<Publisher> GetAllPublishers(string sortBy,string searchString, int? pageNumber) {

           var allPublishers =  _context.Publishers.OrderBy(t => t.Name).ToList();

            if (!string.IsNullOrEmpty(sortBy)) {
                switch (sortBy) {
                case "name_desc":
                    allPublishers = allPublishers.OrderByDescending(t => t.Name).ToList();
                    break;
                default:
                    break;
                }

            }
            if (!string.IsNullOrEmpty(searchString)) {
                allPublishers = allPublishers.Where(t => t.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

            }

            //paging
            int pagesize = 5;
            allPublishers = PaginatedList<Publisher>.Create(allPublishers.AsQueryable(), pageNumber ?? 1, pagesize);

            return allPublishers;
        }

        public Publisher GetPublisherById(int id) => _context.Publishers.FirstOrDefault(t => t.Id == id);
        public PublisherWithBookAndAuthorsVM GetPublisherData(int publisherId) {
            var _publisherData = _context.Publishers.Where(t => t.Id == publisherId)
                .Select(t => new PublisherWithBookAndAuthorsVM {
                    Name = t.Name,
                    BookAuthors = t.Books.Select(x => new BookAuthorVM {
                        BookName = x.Title,
                        BookAuthors = x.Book_Authors.Select(y => y.Author.FullName).ToList()
                    }).ToList()
                }).FirstOrDefault();

            return _publisherData;
        }

        public void DeletePublisherById(int id) {
            var _publisher = _context.Publishers.FirstOrDefault(t => t.Id == id);

            if (_publisher != null) {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            } else {
                throw new Exception($"The Publisher with id: {id} does not exist!");

            }

        }

        private bool StringStartWithNumber(string name) => (Regex.IsMatch(name, @"^\d"));
       
    }
}
