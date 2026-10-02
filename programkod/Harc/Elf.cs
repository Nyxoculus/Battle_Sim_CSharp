using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harc
{
    public abstract class Elf : Harcos
    {
        
        public int Elixir { get; set; }

        public Elf(string nev) :base(nev)
        {
            Kincs = 0;
        }

        public void GyogyitasiFazis()
        {
            Gyogyit();
            if (Eletero > MaxEletero)
            {
                Eletero = MaxEletero;
            }
        }

        protected abstract void Gyogyit();
    }
    
    public class Vakmero : Elf
    {
        public Vakmero(string nev) : base(nev)
        {
            MaxEletero = 100;
            Eletero = MaxEletero;
            Tamadas = 30;
            Pajzs = 10;
            Elixir = 0;
        }

        protected override void Gyogyit()
        {
            while (Eletero < 30 && Elixir > 0)
            {
                Eletero++;
                Elixir--;
            }
        }

        public override void Accept(IVisitor visitor) => visitor.VisitElf(this);
    }

    public class Ugyes : Elf
    {
        public Ugyes(string nev) : base(nev)
        {
            MaxEletero = 80;
            Eletero = MaxEletero;
            Tamadas = 20;
            Pajzs = 20;
            Elixir = 0;
        }

        protected override void Gyogyit()
        {
            if (Kincs > 0)
            {
                int valthato = Kincs / 2;
                Kincs -= valthato;
                Elixir += valthato;
            }

            while (Eletero < 50 && Elixir > 0)
            {
                Eletero++;
                Elixir--;
            }
        }

        public override void Accept(IVisitor visitor) => visitor.VisitElf(this);
    }

    public class Bolcs : Elf
    {
        public Bolcs(string nev) : base(nev)
        {
            MaxEletero = 60;
            Eletero = MaxEletero;
            Tamadas = 10;
            Pajzs = 10;
            Elixir = 0;
        }

        protected override void Gyogyit()
        {
            if (Kincs > 0)
            {
                Elixir += Kincs;
                Kincs = 0;
            }

            while (Eletero < MaxEletero && Elixir > 0)
            {
                Eletero++;
                Elixir--;
            }
        }

        public override void Accept(IVisitor visitor) => visitor.VisitElf(this);
    }
}
