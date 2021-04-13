﻿using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.Services {
    public class BooksService {

        private AppDbContext _context;
        public BooksService(AppDbContext context) {
            _context = context;

        }

        public void AddBookWithAuthors(BookVM book) {
            var _book = new Book() {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverURL = book.CoverURL,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId
            };
            _context.Books.Add(_book);
            _context.SaveChanges();

            foreach (var id in book.AuthorsId) {
                var _book_author = new Book_Author() {
                    BookId = _book.Id,
                    AuthorId = id
                };

                _context.Books_Authors.Add(_book_author);
                _context.SaveChanges();

            }
        }


        public List<Book> GetAllBooks() => _context.Books.ToList();

        public BookwithAuthorsVM GetBookById(int BookId) {
            var _bookWithAuthors = _context.Books.Where(t => t.Id == BookId).Select(t => new BookwithAuthorsVM() {
                Title = t.Title,
                Description = t.Description,
                IsRead = t.IsRead,
                DateRead = t.IsRead ? t.DateRead.Value : null,
                Rate = t.IsRead ? t.Rate.Value : null,
                Genre = t.Genre,
                CoverURL = t.CoverURL,
                PublisherName = t.Publisher.Name,
                AuthorsName = t.Book_Authors.Select(x => x.Author.FullName).ToList()
            }).FirstOrDefault();

            return _bookWithAuthors;
        }

        public Book UpdateBookById(int bookId, BookVM book) {
            var _book = _context.Books.FirstOrDefault(t => t.Id == bookId);
            if (_book != null) {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead.Value : null;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.Genre = book.Genre;
                _book.CoverURL = book.CoverURL;

                _context.SaveChanges();
            }

            return _book;
        }

        public void DeleteBookById(int bookId) {
            var _book = _context.Books.FirstOrDefault(t => t.Id == bookId);
            if (_book != null) {
                _context.Books.Remove(_book);
                _context.SaveChanges();
            }
        }
    }
}