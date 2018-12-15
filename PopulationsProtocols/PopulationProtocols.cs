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
            var monteCarloTestNumbers = new MyMatrix(3, 4);
            monteCarloTestNumbers.addElement(3);
            monteCarloTestNumbers.addElement(5);
            monteCarloTestNumbers.addElement(7);
            monteCarloTestNumbers.addElement(10);
            var numberTest = new MyMatrix(3, 4);
            numberTest.addElement(3);
            numberTest.addElement(5);
            numberTest.addElement(7);
            numberTest.addElement(10);
            numberTest.addElement(15);
            numberTest.addElement(20);
            numberTest.addElement(30);
            numberTest.addElement(40);
            numberTest.addElement(50);

            List<int> allMethodTest = new LinkedList<>(numberTest);
            List<double> accuracyRange = new LinkedList<>();
            accuracyRange.addElement(0.000001);
            accuracyRange.addElement(0.0000000001);
            accuracyRange.addElement(0.00000000000001);

            compareAllMethods(allMethodTest);
            jacobiSeidelAccuracyTest(allMethodTest, accuracyRange);
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
                double[] s1 = test.solveMatrix(true);
                long sparseGaussTime = nanoTime() - t1;
                t1 = nanoTime();
                double[] s2 = test.solveMatrix(false);
                long normalGaussTime = nanoTime() - t1;
                t1 = nanoTime();
                double sum = 0.0;
                for (int i = 0; i < s1.Length; i++)
                {
                    sum += s1[i] - s2[i];
                }
                sum /= s1.Length;
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
            long t1, sparseGaussTime, normalGaussTime, jacobiTime, seidelTime;
            foreach (int x in agentList)
            {
                csvRows = new StringBuilder();
                csvRows.Append(CSV_JACOBI_SEIDEL_ACCURACY_TEST + "\n");
                test = new AgentMatrix(x);
                foreach (double n in errorList)
                {
                    t1 = nanoTime();
                    test.solveJacobi(n);
                    jacobiTime = nanoTime() - t1;
                    int jacobiIteration = test.getRecentIterations().i;
                    t1 = nanoTime();
                    test.solveSeidel(n);
                    seidelTime = nanoTime() - t1;
                    int seidelIterations = test.getRecentIterations().i;
                    csvRows.Append(x + ";" + n + ";" + jacobiIteration + ";" + jacobiTime + ";" + seidelIterations + ";" + seidelTime + "\n");
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
                test.solveMatrix(true);
                sparseGaussTime = nanoTime() - t1;
                t1 = nanoTime();
                test.solveMatrix(false);
                normalGaussTime = nanoTime() - t1;
                t1 = nanoTime();
                test.solveJacobi(0.00000000000001);
                jacobiTime = nanoTime() - t1;
                int jacobiIteration = test.getRecentIterations().i;
                t1 = nanoTime();
                test.solveSeidel(0.00000000000001);
                seidelTime = nanoTime() - t1;
                int seidelIterations = test.getRecentIterations().i;
                csvRows.Append(n + ";" + normalGaussTime + ";" + (sparseGaussTime) + ";" + jacobiTime + ";" + seidelTime + ";" + jacobiIteration + ";" + seidelIterations + "\n");
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
            double[] gaussSeidel = test.solveSeidel(0.00001);
            double[] jacobi = test.solveJacobi(0.00001);
            double[] gauss = test.solveMatrix(false);
            double[] sparseGauss = test.solveMatrix(true);
            object[] monteCarlo = test.monteCarloSimulation(numberOfSimulations).toArray();
            for (int i = 0; i < gauss.Length; i++)
            {
                Node n = test.getNodeList().get(i);
                csvRow.Append("P(" + n.getY() + "," + n.getN() + ");" + monteCarlo[i] + ";" + gauss[i] + ";" + sparseGauss[i] + ";" + jacobi[i] + ";" + gaussSeidel[i] + "\n");
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
