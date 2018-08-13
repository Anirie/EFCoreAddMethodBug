
using devMathOpt.TestModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using TestSupport.EfHelpers;

namespace devMathOpt
{

    class Program
    {

        static void Main(string[] args)
        {


            var options = SqliteInMemory.CreateOptions<TestModelContext>();
            var dbContext = new TestModelContext(options);


            dbContext.Database.EnsureCreated();

            //create book with one page and one image ( image has navigation props to book and bookpage)
            var book = CreateBasicBook();
            var page = CreateBasicBookPage(book);
            var image = CreateBasicImage(book, page);

            dbContext.Add(book);
            dbContext.Add(page);
            dbContext.Add(image);
            dbContext.SaveChanges();

            //take existing book
            var bookToCopy =dbContext.Books.Include(b => b.BookPages).Include(b => b.Images).First() as Book;

            //reset Connection to Database
            dbContext.Dispose();
            dbContext = new TestModelContext(options);

            // set keyproperties to zero
            bookToCopy.Id = 0;
            //set keyproperties of children to zero
            foreach(var i in bookToCopy.Images)
            {
                i.Id = 0;
            }
            foreach(var p in bookToCopy.BookPages)
            {
                p.Id = 0;
            }

            //add to context
            dbContext.Add(bookToCopy);
            //save context
            dbContext.SaveChanges();

            //reset Connection to Database changes everything
            //dbContext.Dispose();
            //dbContext = new TestModelContext(options);

            //output
            var allBooks = dbContext.Books.Include(b => b.BookPages).Include(b => b.Images).ToList();
  
            Console.WriteLine("Initial Book: "+ JsonConvert.SerializeObject(allBooks[0], new JsonSerializerSettings
            {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
            Console.WriteLine("Copied Book: " + JsonConvert.SerializeObject(allBooks[1], new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));


        }

        private static Book CreateBasicBook()
        {
            return new Book()
            {
                Title = "Ein Titel - " + Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString()
            };
        }

        private static int seitenZahlCounter = 0;

        private static BookPage CreateBasicBookPage(Book book)
        {
            return new BookPage()
            {
                Book = book,
                Seitenzahl = ++seitenZahlCounter,
                Text = Guid.NewGuid().ToString()
            };
        }

        private static Image CreateBasicImage(Book book, BookPage page)
        {
            return new Image()
            {
                Book = book,
                BookPage = page,
                ImageDescription = Guid.NewGuid().ToString()
            };
        }

        
    }

}
