using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harc
{
    public abstract class Harcos
    {
        public string Nev { get; protected set; }
        public int Eletero { get; set; }
        public int MaxEletero { get; protected set; }
        public int Tamadas { get; protected set; }
        public int Pajzs { get; protected set; }
        public int Kincs { get; set; }
        public bool El => Eletero > 0;

        public Harcos(string nev)
        {
            Nev = nev;
        }

        public abstract void Accept(IVisitor visitor);
    }
}
