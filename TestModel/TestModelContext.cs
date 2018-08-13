using devMathOpt.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace devMathOpt.TestModel
{
    public class TestModelContext : DbContext, IDbToolContext
    {
        public TestModelContext(DbContextOptions<TestModelContext> options) : base(options) {
            ContextOptions = options;
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookPage> BookPages { get; set; }

        public DbSet<Image> Images { get; set; }

        public Type ScenarioType => typeof(Book);

        public Type ScenarioIdType => typeof(int);

        public DbContextOptions ContextOptions {get;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Title");

            });

            modelBuilder.Entity<BookPage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Seitenzahl).HasColumnName("Seitenzahl");
                
                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.Text)
                                   .HasMaxLength(255)
                                   .IsUnicode(false)
                                   .HasColumnName("Title");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookPages)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookPage_Book");

            });

           

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ImageDescription)
                                    .HasMaxLength(255)
                                    .IsUnicode(false)
                                    .HasColumnName("ImageDescription");

                entity.Property(e => e.BookId).HasColumnName("BookID");
                entity.Property(e => e.BookPageId).HasColumnName("BookPageID");


                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_Book");

                entity.HasOne(d => d.BookPage)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.BookPageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_BookPage");

            });
        }
    }
}