using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace devMathOpt.TestModel
{
    public class BookPage
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Seitenzahl { get; set; }
        public string Text { get; set; }
        
        public Book Book { get; set; }
        public ICollection<Image> Images { get; set; }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;
            else
                return this.Equals((BookPage)obj);
        }

        public bool Equals(BookPage otherPage)
        {
            return this.Id == otherPage.Id 
                && this.BookId == otherPage.BookId
                && this.Seitenzahl == otherPage.Seitenzahl
                && this.Text == otherPage.Text;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
