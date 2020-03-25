using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voice_Recognition
{
    public class GrammarWord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Counter { get; set; }

        public GrammarWord()
        {

        }
        public GrammarWord(int id, string name, int counter)
        {
            Id = id;
            Name = name;
            Counter = counter;
        }
    }
}
