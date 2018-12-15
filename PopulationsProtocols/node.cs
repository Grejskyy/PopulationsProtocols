namespace PopulationsProtocols
{
    #region Usings
    using System;
    #endregion
    class Node {
        public static String YU_PAIR = "YU", NU_PAIR = "NU", YN_PAIR = "YN";
        private int Y,N,U; // Number of yes/no/unknown agents

        public int getY() {
            return Y;
        }
        public int getN() {
            return N;
        }
        public int getU() {
            return U;
        }
        public Node(int Y, int N, int U) {
            this.Y = Y;
            this.N = N;
            this.U = U;
        }
        public int getNumberOfAgents() {
        return Y + N + U;
        }
        public String getRandomPair() {
            String pair = "";
            Random random = new Random();
            int range = getNumberOfAgents();
            int firstRandomElement = random.Next(range);
            int secondRandomElement = random.Next(range);
            while(secondRandomElement == firstRandomElement) {
                secondRandomElement = random.Next(range);
            }
            if(secondRandomElement < firstRandomElement) {
                int temp = firstRandomElement;
                firstRandomElement = secondRandomElement;
                secondRandomElement = temp;
            }
            int YRange = Y;
            int NRange = Y + N;
            if(firstRandomElement < YRange) {
                pair += "Y";
            } else if(firstRandomElement < NRange) {
                pair += "N";
            } else{
                pair += "U";
            }
            if(secondRandomElement < YRange) {
                pair += "Y";
            } else if(secondRandomElement < NRange) {
                pair += "N";
            } else {
                pair += "U";
            }
            return pair;
        }
        public void changeAgentsState(String pair) {
            if(pair.equals(YU_PAIR)) {
                U -= 1;
                Y += 1;
            } else if(pair.equals(NU_PAIR)) {
                U -= 1;
                N += 1;
            } else if(pair.equals(YN_PAIR)) {
                U += 2;
                N -= 1;
                Y -= 1;
            }
        }
    }
}