/// ==================================================================================================================
/// Zrobione:
///     Klasa MyMatrix przeniesiona na tyle na ile byla potrzeba dx
///     Zaimplementowana metoda Jacobiego (watch?v=bR2SEe8W3Ig)
///     Zaimplementowana metoda Gaussa-Seidla (watch?v=F6J3ZmXkMj0)
///     Zaimplementowana metoda eliminacji Gaussa (poprzednie zadanie,)
/// Co trzeba zrobic:
///     Najlepiej byloby zrozumiec o co chodzi
///     Jak uzyc tych metod do obliczenia tych smiesznych prawdopodobienstw
///     Implementacja metody Monte Carlo
///     Na pewno jeszcze tysiac zadan
/// ==================================================================================================================
namespace PopulationsProtocols
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    #endregion
    class PopulationProtocols
    {
        public static String CSV_MONTE_CARLO_COPARE_HEADER = "P(Y,N);Monte Carlo;Gauss;Sparse Gauss;Jacobi;Gauss-Seidel";
        public static String CSV_GAUSS_ELIMINATION_HEADER = "N;Normal Time; Sparse Time; Difference in result";
        public static String CAV_ALL_METHODS_COMPARE_HEADER = "N;Gauss time;Sparse gauss time;Jacobi time; Gauss Seidel Time; Itrations for 10-14 Jacobi; Iterations for 10-14 Seidel";
        public static String CSV_JACOBI_SEIDEL_ACCURACY_TEST = "N;Error;Jacobi Steps; Jacobi Time;Gauss-Seidel Steps;Gauss Seidel Time";
        static void Main(string[] args)
        {
        }

        public void compareGaussEliminationMethod(List<int> numberOfAgents)
        {
            AgentMatrix test;// = new AgentMatrix(numberOfAgents);
            StringBuilder csvRows = new StringBuilder();
            csvRows.Append(CSV_GAUSS_ELIMINATION_HEADER + "\n");
            foreach (int n in numberOfAgents)
            {
                test = new AgentMatrix(n);
                long t1 = nanoTime();
                var s1 = test.SolveGauss(true);
                long sparseGaussTime = nanoTime() - t1;
                t1 = nanoTime();
                var s2 = test.SolveGauss(false);
                long normalGaussTime = nanoTime() - t1;
                t1 = nanoTime();
                double sum = 0.0;
                for (int i = 0; i < s1.Count; i++)
                {
                    sum += s1[i] - s2[i];
                }
                sum /= s1.Count;
                csvRows.Append(n + ";" + (normalGaussTime) + ";" + (sparseGaussTime) + ";" + sum + "\n");
                Console.WriteLine("Done for " + n);
            }
            var filename = "1GaussComparation.csv";
            FileOutput.saveResult(filename, csvRows.ToString());
        }

        public void jacobiSeidelAccuracyTest(List<int> agentList, List<Double> errorList)
        {
            AgentMatrix test;// = new AgentMatrix(numberOfAgents);
            StringBuilder csvRows = new StringBuilder();
            long t1, jacobiTime, seidelTime;
            foreach (int x in agentList)
            {
                csvRows = new StringBuilder();
                csvRows.Append(CSV_JACOBI_SEIDEL_ACCURACY_TEST + "\n");
                test = new AgentMatrix(x);
                foreach (double n in errorList)
                {
                    t1 = nanoTime();
                    test.SolveJacobi();
                    jacobiTime = nanoTime() - t1;
                    t1 = nanoTime();
                    test.SolveSeidel();
                    seidelTime = nanoTime() - t1;
                    csvRows.Append(x + ";" + n + ";" + jacobiTime + ";" + seidelTime + "\n");
                    Console.WriteLine("Done for " + x);
                }
                var filename = "JacobiSeidelCompare" + x + "Agents.csv";
                FileOutput.saveResult(filename, csvRows.ToString());
            }
        }

        public void compareAllMethods(List<int> numberOfAgents)
        {
            AgentMatrix test;// = new AgentMatrix(numberOfAgents);
            StringBuilder csvRows = new StringBuilder();
            long t1, sparseGaussTime, normalGaussTime, jacobiTime, seidelTime;
            csvRows.Append(CAV_ALL_METHODS_COMPARE_HEADER + "\n");
            foreach (int n in numberOfAgents)
            {
                test = new AgentMatrix(n);
                t1 = nanoTime();
                test.SolveGauss(true);
                sparseGaussTime = nanoTime() - t1;
                t1 = nanoTime();
                test.SolveGauss(false);
                normalGaussTime = nanoTime() - t1;
                t1 = nanoTime();
                test.SolveJacobi();
                jacobiTime = nanoTime() - t1;
                t1 = nanoTime();
                test.SolveSeidel();
                seidelTime = nanoTime() - t1;
                csvRows.Append(n + ";" + normalGaussTime + ";" + (sparseGaussTime) + ";" + jacobiTime + ";" + seidelTime);
                Console.WriteLine("Done for " + n);
            }
            var filename = "1AllMethodsCompare.csv";
            FileOutput.saveResult(filename, csvRows.ToString());
        }
        public void compareToMonteCarlo(int numberOfAgents, int numberOfSimulations)
        {
            AgentMatrix test = new AgentMatrix(numberOfAgents);
            StringBuilder csvRow = new StringBuilder();
            csvRow.Append(CSV_MONTE_CARLO_COPARE_HEADER + "\n");
            var gaussSeidel = test.SolveSeidel();
            var jacobi = test.SolveJacobi();
            var gauss = test.SolveGauss(false);
            var sparseGauss = test.SolveGauss(true);
            var monteCarlo = test.MonteCarloSimulation(numberOfSimulations).ToArray();
            for (int i = 0; i < gauss.Count; i++)
            {
                Node n = test.GetNodeList()[i];
                csvRow.Append("P(" + n.GetY() + "," + n.GetN() + ");" + monteCarlo[i] + ";" + gauss[i] + ";" + sparseGauss[i] + ";" + jacobi[i] + ";" + gaussSeidel[i] + "\n");
            }
            var filename = "MonteCarlo" + numberOfAgents + "Agents.csv";
            FileOutput.saveResult(filename, csvRow.ToString());// "MonteCarlo"+numberOfAgents+"Agents.csv";
            Console.WriteLine(csvRow.ToString());
        }

        public void monteCarloTest(List<int> n)
        {
            foreach (int x in n) compareToMonteCarlo(x, 1000000);
        }

        private static long nanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }
    }
}
