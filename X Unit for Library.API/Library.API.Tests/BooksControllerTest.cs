using Library.API.Controllers;
using Library.API.Data.Models;
using Library.API.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Library.API.Tests
{
    public class BooksControllerTest
    {
        private BooksController controller;
        private IBookService bookService;

        public BooksControllerTest()
        {
            bookService = new BookService();
            controller = new BooksController(bookService);
        }

        [Fact]          // no params passed to test
        public void Get_WhenCalled_ReturnsOkResult()
        {

            ActionResult<IEnumerable<Book>> result = controller.Get();

            // is it a ok result?
            Assert.IsType<OkObjectResult>(result.Result);


            // is it a list of Books
            var obj = result.Result as OkObjectResult;
            Assert.IsType<List<Book>>(obj.Value);

            // book list count
            var listOfBooks = obj.Value as List<Book>;
            Assert.NotNull(listOfBooks);
            Assert.Equal(5, listOfBooks.Count);
        }

        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c201")]
        public void GetById_WhenCalledWithInvalidId_ReturnsNotFoundResult(string id)
        {
            Guid guidId = new Guid(id);
            ActionResult<Book> result = controller.Get(guidId);

            // is it a ok Result
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200", "Managing Oneself", "We live in an age of unprecedented opportunity: with ambition, drive, and talent, you can rise to the top of your chosen profession, regardless of where you started out...", "Peter Ducker")]
        public void GetById_WhenCalledWithValidId_ReturnOkResultWithExpected(string id, string title, string description, string author)
        {
            // arrannge
            Guid guidId = new Guid(id);

            // act
            ActionResult<Book> result = controller.Get(guidId);

            // is it ok result?
            Assert.IsType<OkObjectResult>(result.Result);

            // is it a book object
            var obj = result.Result as OkObjectResult;
            Assert.IsType<Book>(obj.Value);

            // book object Guid should be same
            var bookObj = obj.Value as Book;
            Assert.NotNull(bookObj);
            Assert.Equal(guidId, bookObj.Id);
            Assert.Equal(title, bookObj.Title);
            Assert.Equal(description, bookObj.Description);
            Assert.Equal(author, bookObj.Author);
        }

        [Fact]
        public void Post_WhenCalledWithInvalidTitle_ReturnsBadRequest()
        {
            var book = new Book { Title = null, Author = null, Description = null };
            controller.ModelState.AddModelError("Title", "Title Required");

            var result = controller.Post(book);

            // is it BadRequest result?
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("Test title", "Test author", "Test description")]
        public void Post_WhenCalledWithValid_ReturnsCreatedResult(string title, string author, string description)
        {
            var book = new Book { Title = title, Author = author, Description = description };

            var result = controller.Post(book);

            // is created result
            Assert.IsType<CreatedAtActionResult>(result);

            var obj = result as CreatedAtActionResult;
            Assert.IsType<Book>(obj.Value);

            var bookReturned = obj.Value as Book;
            Assert.Equal(title, bookReturned.Title);
            Assert.Equal(author, bookReturned.Author);
            Assert.Equal(description, bookReturned.Description);
        }

        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200")]
        public void Remove_WhenCalledWithValidId_RemovedBook(string id)
        {
            Guid guid = new Guid(id);
            int countBeforeRemoval = bookService.GetAll().Count();

            ActionResult okReponse = controller.Remove(guid);

            // is it ok?
            Assert.IsType<OkResult>(okReponse);

            // test counts reduced by 1
            int countAfterRemoval = bookService.GetAll().Count();
            Assert.Equal(countBeforeRemoval - 1, countAfterRemoval);
        }


        [Fact]
        public void Remove_WhenCalledWithInvalidId_ReturnsNotFound()
        {
            Guid guid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c201");
            int countBeforeRemoval = bookService.GetAll().Count();


            ActionResult notFoundResponse = controller.Remove(guid);

            // is it not found?
            Assert.IsType<NotFoundResult>(notFoundResponse);

            // test counts unchanged
            int countAfterRemoval = bookService.GetAll().Count();
            Assert.Equal(countBeforeRemoval, countAfterRemoval);
        }
    }
}
