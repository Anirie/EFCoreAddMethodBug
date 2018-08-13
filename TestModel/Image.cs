using System;
using System.Collections.Generic;
using System.Text;

namespace devMathOpt.TestModel
{
    public class Image
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int BookPageId { get; set; }
        public String ImageDescription { get; set; }

        public Book Book { get; set; }
        public BookPage BookPage { get; set; }
    }
}
