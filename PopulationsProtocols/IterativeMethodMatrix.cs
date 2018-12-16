namespace PopulationsProtocols
{
    #region Usings
    using System;
    #endregion
    class IterativeMethodMatrix
    {
        public static double[] SolveJacobi(double[,] matrix, double[] vector, double precision) {
            int iterations = 0;
            int size = matrix.Length;
            double[,] M = new double[size,size];
            double[] reversedMatrix = new double[size];
            double[] result = new double[size];
            double[] persistedX = new double[size];

            for (int i=0; i<size; i++) {
                reversedMatrix[i] = 1 / matrix[i,i];
            }
            for (int i=0; i<size; i++) {
                for (int j = 0; j < size; j++) {
                    if (i == j) {
                        M[i,j] = 0.0;
                    } else {
                        M[i,j] = -(matrix[i,j] * reversedMatrix[i]);
                    }
                }
            }
            for (int i=0; i<size; i++) {
                result[i] = 0.0;
            }
            while(true) {
                for (int i = 0; i < size; i++) {
                    persistedX[i] = reversedMatrix[i] * vector[i];
                    for (int j = 0; j < size; j++) {
                        persistedX[i] += M[i,j] * result[j];
                    }
                }
                if(precisionReached(result, persistedX, precision)) break;
                if (size >= 0) Array.Copy(persistedX, 0, result, 0, size);
                iterations++;
            }
            return result;
        }

        public static double[] SolveGaussSeidel(double[,] matrix, double[] vector, double precision) {
            int iterations = 0;
            int size = matrix.Length;
            double[,] reversedMatrix = new double[size,size];
            double[,] upper = new double[size,size];
            double[,] lower = new double[size,size];
            double[] result = new double[size];
            double[] persistedX = new double[size];

            for (int i=0; i<size; i++) {
                result[i] = 0.0;
            }
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    if (i < j) {
                        upper[i,j] = matrix[i,j];
                    }
                    else if(i > j) {
                        lower[i,j] = matrix[i,j];
                    }
                    else {
                        reversedMatrix[i,j] = 1 / matrix[i,j];
                    }
                }
            }
            for (int i = 0; i < size; i++) {
                vector[i] *= reversedMatrix[i,i];
            }
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < i; j++) {
                    lower[i,j] *= reversedMatrix[i,i];
                }
            }
            for (int i = 0; i < size; i++) {
                for (int j = i + 1; j < size; j++) {
                    upper[i,j] *= reversedMatrix[i,i];
                }
            }        

            while(true) {
                Array.Copy(result, 0, persistedX, 0, size);
                for (int i = 0; i < size; i++) {
                    result[i] = vector[i];
                    for (int j = 0; j < i; j++) {
                        result[i] -= lower[i,j] * result[j];
                    }
                    for (int j = i + 1; j < size; j++) {
                        result[i] -= upper[i,j] * result[j];
                    }
                }
                if(iterations!=0 && PrecisionReached(persistedX, result, precision)) break;
                iterations++;
            }
            return result;
        }

        private static bool PrecisionReached(double[] comparedValues, double[] values, double precision) {
            double sum = 0.0;
            for(int i=0;i<values.Length;i++){
                double value = values[i];
                double value2 = comparedValues[i];
                if(value != 0) sum += Math.Abs((value-value2)/value);
            }
            sum /= values.Length;
            return !(sum > precision);
        }
    }
}
