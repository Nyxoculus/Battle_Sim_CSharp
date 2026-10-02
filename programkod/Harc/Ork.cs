using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harc
{
    public abstract class Ork : Harcos
    {
        public Ork(string nev, int kincs) : base(nev)
        {
            Kincs = kincs;
        }
    }

    public class Verengzo : Ork
    {
        public Verengzo(string nev, int kincs) : base(nev, kincs)
        {
            MaxEletero = 100;
            Eletero = MaxEletero;
            Tamadas = 30;
            Pajzs = 5;
        }

        public override void Accept(IVisitor visitor) => visitor.VisitOrk(this);
    }

    public class Ravasz : Ork
    {
        public Ravasz(string nev, int kincs) : base(nev, kincs)
        {
            MaxEletero = 90;
            Eletero = MaxEletero;
            Tamadas = 15;
            Pajzs = 20;
        }

        public override void Accept(IVisitor visitor) => visitor.VisitOrk(this);
    }

    public class Ovatos : Ork
    {
        public Ovatos(string nev, int kincs) : base(nev, kincs)
        {
            MaxEletero = 80;
            Eletero = MaxEletero;
            Tamadas = 10;
            Pajzs = 15;
        }

        public override void Accept(IVisitor visitor) => visitor.VisitOrk(this);
    }
}
