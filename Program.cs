
using devMathOpt.Implementations;
using devMathOpt.TestModel;
using MathTec.Lib.Metadata;
using MathTourTestDataContext.Models;
using Microsoft.EntityFrameworkCore;
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
           // var genericRepository = new Implementations.GenericRepository(dbContext);
            //var scenarioRepository = new Implementations.ScenarioRepository(dbContext, genericRepository);

            dbContext.Database.EnsureCreated();

            //create book with one page and one image ( image has navigation props to book and bookpage)
            var book = CreateBasicBook();
            var page = CreateBasicBookPage(book);
            var image = CreateBasicImage(book, page);

            dbContext.Add(book);
            dbContext.Add(page);
            dbContext.Add(image);
            dbContext.SaveChanges();

            var scenarioOriginal =dbContext.Books.Include(b => b.BookPages).Include(b => b.Images).First() as Book;
            var imageOriginal = scenarioOriginal.Images.First() as Image;
            var pageOriginal = scenarioOriginal.BookPages.First() as BookPage;

            //reset Connection to Database
            dbContext.Dispose();
            dbContext = new TestModelContext(options);
            //genericRepository = new Implementations.GenericRepository(dbContext);

            Book newbook = scenarioOriginal;

            // set keyproperties to zero
            newbook.Id = 0;
            //set keyproperties of children to zero
            foreach(var i in newbook.Images)
            {
                i.Id = 0;
            }
            foreach(var p in newbook.BookPages)
            {
                p.Id = 0;
            }

            var originalBook = dbContext.Books.First();

            dbContext.Add(newbook);

            dbContext.SaveChanges();

            //var scenarioRepository = new Implementations.ScenarioRepository
            //var test = new ScenarioRepositoryTest();
            //test.Copy();

            //Console.WriteLine("Hello World!");
            //String connectionString = "Server=localhost;Database=MathTour;Trusted_Connection=True;ConnectRetryCount=0";
            //var optionBuilder = new DbContextOptionsBuilder<MathTourContext>().UseSqlServer(connectionString);
            //var Context = new MathTourContext(optionBuilder.Options);
            //var Repository = new GenericRepository(Context);


            //var iv = Context.InputVersionen.Include(version => version.Auftraege)       
            //    .Include(version => version.Fahrzeuge)
            //            .Include(version => version.Filialen)
            //            .Include(version => version.TechnikLevel)
            //            .ToList();
            //var ivFirst = iv.First();

            ////Context.Dispose();
            ////Context = new MathTourContext(optionBuilder.Options);
            ////Repository = new GenericRepository(Context);
            ////DbContextOptions<MathTourContext> options = Context.ContextOptions as DbContextOptions<MathTourContext>;
            ////Context = new MathTourContext(options);
            ////Repository = new GenericRepository(Context);

            //Context = Context.Reset() as MathTourContext;
            //Repository = new GenericRepository(Context);

            //Context.SetKeyPropertiesToZero(ivFirst);
            //Context.SetKeyPropertiesOfChildrenToZero(ivFirst);
            //foreach(var a in ivFirst.Auftraege)
            //{
            //    //a.VersionsId = 0;
            //    //a.Versions = null;
            //}
            //foreach(var f in ivFirst.Filialen)
            //{
            //    //f.VersionsId = 0;
            //    //f.Versions = null;
            //    //f.BenoetigteTechnikLevelId = 0;

            //}
            //foreach(var f in ivFirst.Fahrzeuge)
            //{
            //    //f.VersionsId = 0;
            //    //f.Versions = null;
            //    //f.TechnikLevelId = 0;
            //}
            //foreach(var t in ivFirst.TechnikLevel)
            //{
            //    //t.Versions = null;
            //    //t.VersionsId = 0;
            //    t.Fahrzeuge = null;
            //    t.Filialen = null;
            //}
            //foreach(var r in ivFirst.Retouren)
            //{
            //    //r.FilialId = 0;
            //    //r.VersionsId = 0;
            //    //r.Versions = null;
            //}

            //Context.InputVersionen.Add(ivFirst);
            //Context.SaveChanges();

            //var copy = Repository.InsertEntityWithDeletedIds(ivFirst);

            ////Include(version => version.Auftraege).AsNoTracking()
            //            //.Include(version => version.Fahrzeuge).AsNoTracking()
            //            //.Include(version => version.Filialen).AsNoTracking()
            //            //.Include(version => version.TechnikLevel).AsNoTracking()
            ////            //.ToList();
            ////var input2 = Context.InputVersionen.Include(version => version.Auftraege)
            ////            .Include(version => version.Fahrzeuge)
            ////            .Include(version => version.Filialen)
            ////            .Include(version => version.TechnikLevel)
            ////            ;//.ToList();

            ////var input3 = Context.InputVersionen.AsNoTracking().Include(version => version.Auftraege)
            ////            .Include(version => version.Fahrzeuge)
            ////            .Include(version => version.Filialen)
            ////            .Include(version => version.TechnikLevel)
            //           ;// .ToList();

            //var copy = Repository.DeleteIdsAndCreate(input);
            //var copy2 = Repository.DeleteIdsAndCreate(input2);
            //var copy3 = Repository.DeleteIdsAndCreate(input3);
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
