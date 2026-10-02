using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Harc
{
    public class Program
    {
        static void Main(string[] args)
        {
            string fajlNev = "input.txt";

            if (!File.Exists(fajlNev))
            {
                File.WriteAllLines(fajlNev, new string[]
                {
                    "Falánk o v 7",
                    "Sudár e u",
                    "Köpcös o v 4",
                    "Nyúlánk e v"
                });
            }

            List<Elf> elfek = new List<Elf>();
            List<Ork> orkok = new List<Ork>();

            foreach (var sor in File.ReadAllLines(fajlNev))
            {
                if (string.IsNullOrWhiteSpace(sor)) continue;
                string[] darabok = sor.Split(' ');

                string nev = darabok[0];
                string faj = darabok[1];
                string tipus = darabok[2];

                if (faj == "e")
                {
                    if (tipus == "v") elfek.Add(new Vakmero(nev));
                    else if (tipus == "u") elfek.Add(new Ugyes(nev));
                    else if (tipus == "b") elfek.Add(new Bolcs(nev));
                }
                else if (faj == "o")
                {
                    int kincs = darabok.Length > 3 ? int.Parse(darabok[3]) : 0;
                    if (tipus == "v") orkok.Add(new Verengzo(nev, kincs));
                    else if (tipus == "r") orkok.Add(new Ravasz(nev, kincs));
                    else if (tipus == "o") orkok.Add(new Ovatos(nev, kincs));
                }
            }

            KiirVisitor kiirato = new KiirVisitor();
            int menetszamlalo = 1;

            string elozoKorAllapota = "";

            Console.WriteLine("CSATA KEZDETE!!!");

            while (elfek.Any(e => e.El) && orkok.Any(o => o.El))
            {
                Console.WriteLine($"\n--- {menetszamlalo}. MENET ---");

                var eloElfek = elfek.Where(e => e.El).ToList();
                var eloOrkok = orkok.Where(o => o.El).ToList();

                for (int i = 0; i < eloElfek.Count; i++)
                {
                    if (!orkok.Any(o => o.El)) break;

                    Elf aktualisElf = eloElfek[i];
                    Ork aktualisOrk = orkok.Where(o => o.El).ElementAt(i % orkok.Count(o => o.El));

                    TamadasLevezetes(aktualisElf, aktualisOrk);

                    if (aktualisOrk.El)
                    {
                        TamadasLevezetes(aktualisOrk, aktualisElf);
                    }
                }

                foreach (var elf in elfek.Where(e => e.El))
                {
                    elf.GyogyitasiFazis();
                }

                Console.WriteLine("\nKör után életben maradt harcosok:");
                foreach (var elf in elfek.Where(e => e.El)) elf.Accept(kiirato);
                foreach (var ork in orkok.Where(o => o.El)) ork.Accept(kiirato);

                string aktualisAllapot = AllapotLenyomatGeneralas(elfek, orkok);

                if (aktualisAllapot == elozoKorAllapota)
                {
                    Console.WriteLine("\nCSATA VÉGE!!!");
                    Console.WriteLine($"Patthelyzet alakult ki a(z) {menetszamlalo}. menetben! A csata DÖNTETLEN!");
                    return;
                }

                elozoKorAllapota = aktualisAllapot;
                menetszamlalo++;
            }
            Console.WriteLine("\nCSATA VÉGE!!!");

            if (elfek.Any(e => e.El) && !orkok.Any(o => o.El))
            {
                Console.WriteLine("Az Elfek csapata nyerte a csatát!");
            }
            else if (orkok.Any(o => o.El) && !elfek.Any(e => e.El))
            {
                Console.WriteLine("Az Orkok csapata nyerte a csatát!");
            }
            else
            {
                Console.WriteLine("Minden harcos elesett a csatatéren! Nincs túlélő.");
            }
        }

        public static string AllapotLenyomatGeneralas(List<Elf> elfek, List<Ork> orkok)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (var elf in elfek.Where(e => e.El))
            {
                sb.Append($"{elf.Nev}:{elf.Eletero}:{elf.Elixir}:{elf.Kincs}|");
            }
            foreach (var ork in orkok.Where(o => o.El))
            {
                sb.Append($"{ork.Nev}:{ork.Eletero}:{ork.Kincs}|");
            }

            return sb.ToString();
        }

        public static void TamadasLevezetes(Harcos tamado, Harcos vedo)
        {
            int sebzes = tamado.Tamadas - vedo.Pajzs;
            if (sebzes > 0)
            {
                vedo.Eletero -= sebzes;
                if (vedo.Eletero <= 0)
                {
                    vedo.Eletero = 0; 
                    tamado.Kincs += vedo.Kincs;
                    vedo.Kincs = 0;
                }
            }
        }
    }
}