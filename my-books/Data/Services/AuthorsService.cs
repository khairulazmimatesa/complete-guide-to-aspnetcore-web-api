using my_books.Data;
using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.Services {
    public class AuthorsService  {

        private AppDbContext _context;
        public AuthorsService(AppDbContext context) {
            _context = context;

        }

        public void AddAuthor(AuthorVM author) {
            var _author = new Author() {
                FullName = author.FullName
            };
            _context.Authors.Add(_author);
            _context.SaveChanges();
        }

        public AuthorWithBooksVM GetAuthorWithBooks(int authorId) {
            var _author = _context.Authors.Where(t => t.Id == authorId).Select(t => new AuthorWithBooksVM {
                FullName = t.FullName,
                BookTitles = t.Book_Authors.Select(x => x.Book.Title).ToList()
            }).FirstOrDefault();

            return _author;

        }

    }
}
