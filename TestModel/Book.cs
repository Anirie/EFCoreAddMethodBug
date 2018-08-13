using devMathOpt.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace devMathOpt.TestModel
{
    public class Book : IScenario
    {
        public int Id { get; set; }
        public String Title { get; set; }

        public ICollection<BookPage> BookPages { get; set; }
        public ICollection<Image> Images { get; set; }

        public string ScenarioID => Id + "";

        public string ShortDescription
        {
            get
            {
                return Title;
            }
            set
            {
                this.Title = value;
            }
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;
            else
                return this.Equals((Book)obj);
        }

        public bool Equals(Book otherBook)
        {
            return this.Id == otherBook.Id && this.Title == otherBook.Title;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
