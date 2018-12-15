namespace PopulationsProtocols
{
    #region Usings
    using System;
    #endregion
    class Node
    {
        #region Consts
        public static string YU_PAIR = "YU";
        public const string NU_PAIR = "NU";
        public const string YN_PAIR = "YN";
        #endregion
        #region Private Fields
        private int Y;
        private int N;
        private int U; // Number of yes/no/unknown agents
        #endregion
        #region Public Methods and Operators
        public int GetY()
        {
            return Y;
        }
        public int GetN()
        {
            return N;
        }
        public int GetU()
        {
            return U;
        }
        public Node(int Y, int N, int U)
        {
            this.Y = Y;
            this.N = N;
            this.U = U;
        }
        public int GetNumberOfAgents()
        {
            return Y + N + U;
        }
        public String GetRandomPair()
        {
            String pair = "";
            Random random = new Random();
            int range = GetNumberOfAgents();
            int firstRandomElement = random.Next(range);
            int secondRandomElement = random.Next(range);
            while (secondRandomElement == firstRandomElement)
            {
                secondRandomElement = random.Next(range);
            }
            if (secondRandomElement < firstRandomElement)
            {
                int temp = firstRandomElement;
                firstRandomElement = secondRandomElement;
                secondRandomElement = temp;
            }
            int YRange = Y;
            int NRange = Y + N;
            if (firstRandomElement < YRange)
            {
                pair += "Y";
            }
            else if (firstRandomElement < NRange)
            {
                pair += "N";
            }
            else
            {
                pair += "U";
            }
            if (secondRandomElement < YRange)
            {
                pair += "Y";
            }
            else if (secondRandomElement < NRange)
            {
                pair += "N";
            }
            else
            {
                pair += "U";
            }
            return pair;
        }
        public void ChangeAgentsState(String pair)
        {
            if (pair.Equals(YU_PAIR))
            {
                U -= 1;
                Y += 1;
            }
            else if (pair.Equals(NU_PAIR))
            {
                U -= 1;
                N += 1;
            }
            else if (pair.Equals(YN_PAIR))
            {
                U += 2;
                N -= 1;
                Y -= 1;
            }
        }
        #endregion
    }
}