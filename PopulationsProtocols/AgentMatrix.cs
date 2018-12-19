namespace PopulationsProtocols
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion
    class AgentMatrix
    {
        #region Private Fields
        private int numberOfAgents;
        private int possibilityDivider;
        private int matrixSize;
        private List<Node> nodes;
        private double[,] values;
        private int recentIterations;
        private MyMatrix matrixOfAgents;
        private MyMatrix vectorMatrix;
        #endregion

        #region Constructors and Destructors
        public AgentMatrix(int n)
        {
            this.numberOfAgents = n;
            this.nodes = new List<Node>();
            this.recentIterations = 0;
            this.CreateNodes();
            this.matrixSize = nodes.Count();
            this.values = new double[matrixSize, matrixSize];
            this.possibilityDivider = numberOfAgents * (numberOfAgents - 1) / 2;
            this.FillRows();
            this.matrixOfAgents = new MyMatrix(ValuesAsObject());
            this.FillVector();
        }
        #endregion

        #region Public Methods and Operators
        public MyMatrix JoinMatrix(MyMatrix left, MyMatrix right)
        {
            var result = new MyMatrix(left.Rows, left.Cols + 1);
            for(int i = 0; i < left.Rows; i++)
            {
                for(int j = 0; j < left.Cols; j++)
                {
                    result[i, j] = left[i, j];
                }
                result[i, left.Cols] = right[i, 0];
            }
            return result;
        public List<double> SolveGauss(bool sparse)
        {
            var tempMatrix = new MyMatrix(matrixOfAgents);
            var tempVector = new MyMatrix(vectorMatrix.GetColumn(0));
            var joinedMatrix = new MyMatrix(JoinMatrix(tempMatrix, tempVector));
            var values = joinedMatrix.Gauss(sparse);
            return values;
        }
        public List<double> SolveJacobi()
        {
            var tempMatrix = new MyMatrix(matrixOfAgents);
            var tempVector = new MyMatrix(vectorMatrix.GetColumn(0));
            var joinedMatrix = new MyMatrix(JoinMatrix(tempMatrix, tempVector));
            var values = joinedMatrix.Jacobi();
            return values;
        }

        public List<double> SolveSeidel()
        {
            var tempMatrix = new MyMatrix(matrixOfAgents);
            var tempVector = new MyMatrix(vectorMatrix.GetColumn(0));
            var joinedMatrix = new MyMatrix(JoinMatrix(tempMatrix, tempVector));
            var values = joinedMatrix.Seidel();
            return values;
        }

        public List<double> MonteCarloSimulation(int numberOfTries)
        {
            var succRatio = new List<double>();
            foreach (Node node in nodes)
            {
                succRatio.Add(SimulationForNode(node, numberOfTries));
            }
            return succRatio;
        }

        public void FillRows()
        {
            FillValuesWithZero();
            for (int i = 0; i < matrixSize; i++) FillRow(i);
        }

        public void FillRow(int rowIndex)
        {
            Node rowNode = nodes[rowIndex];
            int Y = rowNode.GetY();
            int N = rowNode.GetN();
            int U = rowNode.GetU();
            double noChangePossibility = ((double)(PossibleCombinations(Y, 2) + PossibleCombinations(N, 2) + PossibleCombinations(U, 2))) / possibilityDivider;
            double uChangePossibility = (double)Y * N / possibilityDivider;
            double nChangePossibility = (double)U * N / possibilityDivider;
            double yChangePossibility = (double)Y * U / possibilityDivider;
            int noChangeNodeIndex = GetNodeIndex(Y, N);
            int uChangeNodeIndex = GetNodeIndex(Y - 1, N - 1);
            int nChangeNodeIndex = GetNodeIndex(Y, N + 1);
            int yChangeNodeIndex = GetNodeIndex(Y + 1, N);
            values[rowIndex, noChangeNodeIndex] = noChangePossibility;
            if (nChangeNodeIndex != -1) values[rowIndex, nChangeNodeIndex] = nChangePossibility;
            if (yChangeNodeIndex != -1) values[rowIndex, yChangeNodeIndex] = yChangePossibility;
            if (uChangeNodeIndex != -1) values[rowIndex, uChangeNodeIndex] = uChangePossibility;

        }

        public Node GetNode(int Y, int N)
        {
            Node preset = nodes
                .FirstOrDefault(node => node.GetY() == Y && node.GetN() == N);
            return preset;
        }

        public List<Node> GetNodeList()
        {
            return nodes;
        }

        public double SimulationForNode(Node node, int numberOfTries)
        {
            int numberOfAgents = node.GetNumberOfAgents();
            double succRatio = 0;

            for (int i = 0; i < numberOfTries; i++)
            {
                Node testNode = new Node(node.GetY(), node.GetN(), node.GetU());

                while (testNode.GetY() != numberOfAgents && testNode.GetN() != numberOfAgents && testNode.GetU() != numberOfAgents)
                {
                    string pair = testNode.GetRandomPair();
                    testNode.ChangeAgentsState(pair);
                }

                if (testNode.GetY() == numberOfAgents) succRatio += 1;
            }
            succRatio /= numberOfTries;
            return succRatio;
        }
        public UInt64 PossibleCombinations(int n, int k)
        {
            if (n == k) return 1;
            UInt64 counter = Factorial(n);
            int x = n - k;
            UInt64 denominator = Factorial(x) * Factorial(k);
            return counter / denominator;
        }
        public UInt64 Factorial(int x)
        {
            if (x < 2) return 1;
            else return (UInt64)x * Factorial(x - 1);
        }
        #endregion

        #region Private Methods
        private void CreateNodes()
        {
            for(int n = numberOfAgents; n>=0; n--)
            {
                for(int j = numberOfAgents-n; j >= 0; j--)
                {
                    for(int i = numberOfAgents-n-j; i >= 0; i--)
                    {
                        if (i + j + n == numberOfAgents) nodes.Insert(0, new Node(i, j, n));
                    }
                }
            }
        }
        private MyMatrix ValuesAsObject()
        {
            var value = new MyMatrix(matrixSize, matrixSize);

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    value[i, j] = values[i, j];
                }
            }
            return value;
        }

        private int GetNodeIndex(int Y, int N)
        {
            Node n = GetNode(Y, N);
            return n != null ? nodes.IndexOf(n) : -1;
        }

        private void FillValuesWithZero()
        {
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    values[i, j] = 0;
                }
            }
        }

        private void FillVector()
        {
            var vector = new MyMatrix(matrixSize, 1);

            for (int i = 0; i<matrixSize; i++)
            {
                if (nodes[i].GetY() == numberOfAgents) vector[i, 0] = 1;
                else vector[i, 0] = 0;
            }
            vectorMatrix = new MyMatrix(vector);
            for (int i = 0; i < matrixSize; i++)
            {
                if (nodes[i].GetN() != numberOfAgents
                    && nodes[i].GetY() != numberOfAgents
                    && nodes[i].GetU() != numberOfAgents)
                    matrixOfAgents[i, i] -= 1;
            }
        }
        #endregion
    }

}