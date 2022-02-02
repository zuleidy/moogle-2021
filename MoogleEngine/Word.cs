using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class Word
    {
        public Word(string name, int tf)
        {
            this.Name = name;
            this.Tf = tf;
        }

        public string Name { get; set; }

        public int Tf { get; set; }

    }
}
