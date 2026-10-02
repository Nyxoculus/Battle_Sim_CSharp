using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Harc
{
    public interface IVisitor
    {
        public void VisitElf(Vakmero elf);
        public void VisitElf(Ugyes elf);
        public void VisitElf(Bolcs elf);

        public void VisitOrk(Verengzo ork);
        public void VisitOrk(Ravasz ork);
        public void VisitOrk(Ovatos ork);
    }

    public class KiirVisitor : IVisitor
    {
        public void VisitElf(Vakmero elf) => KiirElf(elf, "Vakmero");
        public void VisitElf(Ugyes elf) => KiirElf(elf, "Ugyes");
        public void VisitElf(Bolcs elf) => KiirElf(elf, "Bolcs");

        public void VisitOrk(Verengzo ork) => KiirOrk(ork, "Verengzo");
        public void VisitOrk(Ravasz ork) => KiirOrk(ork, "Ravasz");
        public void VisitOrk(Ovatos ork) => KiirOrk(ork, "Ovatos");

        private void KiirElf(Elf elf, string tipusNev)
        {
            Console.WriteLine($"[Elf] Név: {elf.Nev}, Típus: {tipusNev}, Élet: {elf.Eletero}/{elf.MaxEletero}, Elixír: {elf.Elixir}, Kincs: {elf.Kincs}");
        }

        private void KiirOrk(Ork ork, string tipusNev)
        {
            Console.WriteLine($"[Ork] Név: {ork.Nev}, Típus: {tipusNev}, Élet: {ork.Eletero}/{ork.MaxEletero}, Kincs: {ork.Kincs}");
        }
    }
}