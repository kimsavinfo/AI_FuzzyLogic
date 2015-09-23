using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AI_FuzzyLogic.FuzzySystemLib;
using AI_FuzzyLogic.FuzzySetLib;

namespace AI_FuzzyLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("================ IA - Fuzzy Logic: Manage GPS zoom ================\n\n");

            FuzzySystem system = new FuzzySystem("Manage GPS zoom");

            WriteLine("1) Setting values", true);

            // Add the linguistc variable "Distance" : 0 to 500 000 m
            WriteLine("Ajout de la variable Distance");
            LinguisticVariable distance = new LinguisticVariable("Distance", 0, 500000);
            distance.AddValue(new LinguisticValue("Faible", new LeftFuzzySet(0, 500000, 30, 50)));
            distance.AddValue(new LinguisticValue("Moyenne", new TrapezoidalFuzzySet(0, 500000, 40, 50, 100, 150)));
            distance.AddValue(new LinguisticValue("Grande", new RightFuzzySet(0, 500000, 100, 150)));
            system.addInputVariable(distance);

            // Add the linguistc variable "Vitesse" (=speed) : 0 to 200 km/h
            WriteLine("Ajout de la variable Vitesse");
            LinguisticVariable vitesse = new LinguisticVariable("Vitesse", 0, 200);
            vitesse.AddValue(new LinguisticValue("Lente", new LeftFuzzySet(0, 200, 20, 30)));
            vitesse.AddValue(new LinguisticValue("PeuRapide", new TrapezoidalFuzzySet(0, 200, 20, 30, 70, 80)));
            vitesse.AddValue(new LinguisticValue("Rapide", new TrapezoidalFuzzySet(0, 200, 70, 80, 90, 110)));
            vitesse.AddValue(new LinguisticValue("TresRapide", new RightFuzzySet(0, 200, 90, 110)));
            system.addInputVariable(vitesse);

            // Add the linguistc variable "Zoom" : scale 1 to 5
            WriteLine("Ajout de la variable Zoom");
            LinguisticVariable zoom = new LinguisticVariable("Zoom", 0, 5);
            zoom.AddValue(new LinguisticValue("Petit", new LeftFuzzySet(0, 5, 1, 2)));
            zoom.AddValue(new LinguisticValue("Normal", new TrapezoidalFuzzySet(0, 5, 1, 2, 3, 4)));
            zoom.AddValue(new LinguisticValue("Gros", new RightFuzzySet(0, 5, 3, 4)));
            system.addOutputVariable(zoom);

            WriteLine("\n\n2) Ajout des règles", true);

            // Create the rules to generate the following matrix
            // The smaller the zoom is, the farther and less precise we are
            // V \ D  || F | M | G |
            // Lent   || N | P | P |
            // Peu Ra || N | N | P |
            // Rapide || G | N | P |
            // Très R || G | G | P |
            system.addFuzzyRule("IF Distance IS Grande THEN Zoom IS Petit");
            system.addFuzzyRule("IF Distance IS Faible AND Vitesse IS Lente THEN Zoom IS Normal");
            system.addFuzzyRule("IF Distance IS Faible AND Vitesse IS PeuRapide THEN Zoom IS Normal");
            system.addFuzzyRule("IF Distance IS Faible AND Vitesse IS Rapide THEN Zoom IS Gros");
            system.addFuzzyRule("IF Distance IS Faible AND Vitesse IS TresRapide THEN Zoom IS Gros");
            system.addFuzzyRule("IF Distance IS Moyenne AND Vitesse IS Lente THEN Zoom IS Petit");
            system.addFuzzyRule("IF Distance IS Moyenne AND Vitesse IS PeuRapide THEN Zoom IS Normal");
            system.addFuzzyRule("IF Distance IS Moyenne AND Vitesse IS Rapide THEN Zoom IS Normal");
            system.addFuzzyRule("IF Distance IS Moyenne AND Vitesse IS TresRapide THEN Zoom IS Gros");
            WriteLine("9 règles ajoutées \n");

            WriteLine("\n\n3) Résolution de cas pratiques\n", true);
            // Case 1 : 
            //          - speed = 35 km/h 
            //          - next direction shift = 70 m
            WriteLine("Cas 1 :", true);
            WriteLine("V = 35 (peu rapide)");
            WriteLine("D = 70 (moyenne)");
            system.SetInputVariable(vitesse, 35);
            system.SetInputVariable(distance, 70);
            WriteLine("Attendu : zoom normal, centroïde à 2.5");
            WriteLine("Résultat : " + system.Solve() + "\n");

            // Case 2 : 
            //          - speed = 25 km/h 
            //          - next direction shift = 70 m
            system.ResetCase();
            WriteLine("Cas 2 :", true);
            WriteLine("V = 25 (50% lente, 50% peu rapide)");
            WriteLine("D = 70 (moyenne)");
            system.SetInputVariable(vitesse, 25);
            system.SetInputVariable(distance, 70);
            WriteLine("Attendu : zoom normal à 50% + zoom petit à 50%");
            WriteLine("Résultat : " + system.Solve() + "\n");

            // Case 3 : 
            //          - speed = 72.5 km/h 
            //          - next direction shift = 40 m
            system.ResetCase();
            WriteLine("Cas 3 :", true);
            WriteLine("V = 72.5 (75% peu rapide + 25% rapide)");
            WriteLine("D = 40 (50% faible)");
            system.SetInputVariable(vitesse, 72.5);
            system.SetInputVariable(distance, 40);
            WriteLine("Attendu : zoom normal à 50% + zoom gros à 25%");
            WriteLine("Résultat : " + system.Solve() + "\n");

            // Case 4 : 
            //          - speed = 100 km/h 
            //          - next direction shift = 110 m
            system.ResetCase();
            WriteLine("Cas 4 :", true);
            WriteLine("V = 100 (50% rapide + 50% très rapide)");
            WriteLine("D = 110 (80% moyenne, 20% grande)");
            system.SetInputVariable(vitesse, 100);
            system.SetInputVariable(distance, 110);
            WriteLine("Attendu : zoom petit à 20% + zoom normal à 50% + zoom gros à 50%");
            WriteLine("Résultat : " + system.Solve() + "\n");

            // Case 5 : 
            //          - speed = 45 km/h 
            //          - next direction shift = 160 m
            system.ResetCase();
            WriteLine("Cas 5 :", true);
            WriteLine("V = 45 (100% peu rapide)");
            WriteLine("D = 160 (100% grande)");
            system.SetInputVariable(vitesse, 45);
            system.SetInputVariable(distance, 160);
            WriteLine("Attendu : zoom petit à 100%");
            WriteLine("Résultat : " + system.Solve() + "\n");

            System.Console.WriteLine("\n\n================ Have a nice day :) ================");
            Console.ReadLine();
        }

        private static void WriteLine(string msg, bool stars = false)
        {
            if (stars)
            {
                msg = "*** " + msg + " ";
                while (msg.Length < 45)
                {
                    msg += "*";
                }
            }
            Console.WriteLine(msg);
        }
    }
}
