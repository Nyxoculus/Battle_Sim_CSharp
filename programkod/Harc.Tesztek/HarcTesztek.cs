using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil.Rocks;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using Harc;

namespace Harc.Tesztek
{
    [TestClass]
    public class HarcTesztek
    {
        [TestMethod]
        public void TamadasLevezetes_SikeresSebzes_CsokkentiAzEleterot()
        {
            Elf vakmeroElf = new Vakmero("TesztElf");
            Ork verengoOrk = new Verengzo("TesztOrk", 0);

            Program.TamadasLevezetes(vakmeroElf, verengoOrk);

            Assert.AreEqual(75, verengoOrk.Eletero, "Az Ork életerejének 75-re kellett volna csökkennie.");
        }

        [TestMethod]
        public void TamadasLevezetes_VedoMeghal_TamadoMegkapjaAKincset()
        {
            Elf vakmeroElf = new Vakmero("ErősElf");
            Ork ovatosOrk = new Ovatos("GazdagOrk", 10);
            ovatosOrk.Eletero = 5;

            Program.TamadasLevezetes(vakmeroElf, ovatosOrk);

            Assert.AreEqual(0, ovatosOrk.Eletero, "Az Ork életerejének le kell nullázódnia.");
            Assert.AreEqual(10, vakmeroElf.Kincs, "Az Elfnek meg kellett kapnia a meghalt Ork 10 kincsét.");
            Assert.AreEqual(0, ovatosOrk.Kincs, "A meghalt Ork kincsének le kell nullázódnia.");
        }

        [TestMethod]
        public void VakmeroElf_Gyogyulas_CsakHarmincAlattFogyasztElixirt()
        {
            Vakmero vakmero = new Vakmero("VakmerőX");
            vakmero.Elixir = 20;

            vakmero.Eletero = 25;
            vakmero.GyogyitasiFazis();
            Assert.AreEqual(30, vakmero.Eletero, "30-ig kellett volna gyógyulnia.");
            Assert.AreEqual(15, vakmero.Elixir, "5 elixírt el kellett volna használnia.");

            vakmero.Eletero = 35;
            vakmero.GyogyitasiFazis();

            Assert.AreEqual(35, vakmero.Eletero, "Nem szabadott volna változnia az életerőnek.");
            Assert.AreEqual(15, vakmero.Elixir, "Az elixír mennyiségének nem szabadott volna csökkennie.");
        }

        [TestMethod]
        public void UgyesElf_Gyogyulas_KincsFeletValtjaEsOtvenigGyogyul()
        {
            Ugyes ugyes = new Ugyes("ÜgyesX");
            ugyes.Kincs = 10;
            ugyes.Elixir = 0;
            ugyes.Eletero = 45;

            ugyes.GyogyitasiFazis();

            Assert.AreEqual(5, ugyes.Kincs, "A kincs felének meg kellett maradnia.");
            Assert.AreEqual(0, ugyes.Elixir, "Minden elixírt el kellett használnia az 50-es limit eléréséhez.");
            Assert.AreEqual(50, ugyes.Eletero, "Az életerőnek pontosan 50-re kellett növekednie.");
        }

        [TestMethod]
        public void BolcsElf_Gyogyulas_MindenKincsetBevaltEsMaxigGyogyul()
        {
            Bolcs bolcs = new Bolcs("BölcsX");
            bolcs.Kincs = 100;
            bolcs.Eletero = 10;
            bolcs.Elixir = 0;

            bolcs.GyogyitasiFazis();

            Assert.AreEqual(0, bolcs.Kincs, "Minden kincset be kellett váltania.");
            Assert.AreEqual(60, bolcs.Eletero, "Az életerő nem lépheti túl a maximumot (60).");
            Assert.AreEqual(50, bolcs.Elixir, "Csak 50 elixírt szabadott elhasználnia a maximum eléréséhez.");
        }

        [TestMethod]
        public void VisitorMinta_HelyesMetodusMeghivas()
        {
            Elf bolcsElf = new Bolcs("TeszterBolcs");
            FakeVisitor fakeVisitor = new FakeVisitor();

            bolcsElf.Accept(fakeVisitor);

            Assert.IsTrue(fakeVisitor.VisitElfVoltHivva, "Az Accept metódusnak a VisitElf-et kellett meghívnia a Visitoron.");
        }

        [TestMethod]
        public void MenetLogika_TobbiElfMintOrk_ModuloParositasHelyesenMukodik()
        {
            List<Elf> elfek = new List<Elf> { new Vakmero("E1"), new Vakmero("E2"), new Vakmero("E3") };
            List<Ork> orkok = new List<Ork> { new Verengzo("O1", 0), new Verengzo("O2", 0) };

            var eloElfek = elfek.Where(e => e.El).ToList();

            List<string> parositasok = new List<string>();

            for (int i = 0; i < eloElfek.Count; i++)
            {
                if (!orkok.Any(o => o.El)) break;

                Elf aktualisElf = eloElfek[i];
                Ork aktualisOrk = orkok.Where(o => o.El).ElementAt(i % orkok.Count(o => o.El));

                parositasok.Add($"{aktualisElf.Nev}-{aktualisOrk.Nev}");
            }

            Assert.AreEqual(3, parositasok.Count);
            Assert.AreEqual("E1-O1", parositasok[0]);
            Assert.AreEqual("E2-O2", parositasok[1]);
        }

        [TestMethod]
        public void TamadasLevezetes_SebzesKisebbMintPajzs_NemValtozikAzEletero()
        {
            Elf gyengeElf = new Bolcs("Gyenge");

            Ork pancelosOrk = new Ravasz("Páncélos", 0);

            Program.TamadasLevezetes(gyengeElf, pancelosOrk);

            Assert.AreEqual(90, pancelosOrk.Eletero, "A pajzs teljesen felfogta a sebzést, az életerő nem csökkenhet.");
        }
    }

    public class FakeVisitor : IVisitor
    {
        public bool VisitElfVoltHivva { get; private set; } = false;

        public void VisitElf(Vakmero elf) { }
        public void VisitElf(Ugyes elf) { }
        public void VisitElf(Bolcs elf) => VisitElfVoltHivva = true;

        public void VisitOrk(Verengzo ork) { }
        public void VisitOrk(Ravasz ork) { }
        public void VisitOrk(Ovatos ork) { }
    }
}