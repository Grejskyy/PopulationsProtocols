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
        public static String GaussNormalToSparse_Compare = "Agents;Normal Time;Sparse Time;Time Difference";
        public static String JacobiAndSeidel_Compare = "N;Error;Jacobi Steps; Jacobi Time;Gauss-Seidel Steps;Gauss Seidel Time";
        public static String AllMethods_Compare = "N;Gauss time;Sparse gauss time;Jacobi time;";
        public static String MonteCarlo_Equations = "P(Y,N);Monte Carlo;Gauss;Sparse Gauss;Jacobi;Seidel";

        static void Main(string[] args)
        {
        }

        public void compareGaussEliminationMethod(List<int> numberOfAgents)
        {
            AgentMatrix test;// = new AgentMatrix(numberOfAgents);
            StringBuilder csvRows = new StringBuilder();
            csvRows.Append(GaussNormalToSparse_Compare + "\n");
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
                csvRows.Append(n + ";" + normalGaussTime + ";" + sparseGaussTime + ";" + sum + "\n");
                Console.WriteLine("Done for " + n);
            }
            var filename = "NormalToSparseGaussComparation.csv";
            FileOutput.saveResult(filename, csvRows.ToString());
        }

        public void jacobiSeidelAccuracyTest(List<int> agentList, List<Double> errorList)
        {
            AgentMatrix test;
            StringBuilder csvRows = new StringBuilder();
            long t1, jacobiTime, seidelTime;
            foreach (int x in agentList)
            {
                csvRows = new StringBuilder();
                csvRows.Append(JacobiAndSeidel_Compare + "\n");
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
                var filename = "JacobiSeidelComparing " + x + " Agents.csv";
                FileOutput.saveResult(filename, csvRows.ToString());
            }
        }

        public void compareAllMethods(List<int> numberOfAgents)
        {
            AgentMatrix test;// = new AgentMatrix(numberOfAgents);
            StringBuilder csvRows = new StringBuilder();
            long t1, sparseGaussTime, normalGaussTime, jacobiTime, seidelTime;
            csvRows.Append(AllMethods_Compare + "\n");
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
                csvRows.Append(n + ";" + normalGaussTime + ";" + sparseGaussTime + ";" + jacobiTime + ";" + seidelTime);
                Console.WriteLine("Done for " + n);
            }
            var filename = "AllMethodsCompare.csv";
            FileOutput.saveResult(filename, csvRows.ToString());
        }
        public void compareToMonteCarlo(int numberOfAgents, int numberOfSimulations)
        {
            AgentMatrix test = new AgentMatrix(numberOfAgents);
            StringBuilder csvRow = new StringBuilder();
            csvRow.Append(MonteCarlo_Equations + "\n");
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
            var filename = "MonteCarlo " + numberOfAgents + " Agents.csv";
            FileOutput.saveResult(filename, csvRow.ToString());
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

