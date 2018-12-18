using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopulationsProtocols
{
    class GaussPartPivot : GaussElimination
    {
        public static MyMatrix GaussElimination(MyMatrix matrix, MyMatrix vector)
        {
            GaussElimination Solvematrix = new GaussElimination(matrix, vector);
            Solvematrix.BackwardSubstitution();
            return Solvematrix.matrix;
        }
        public static MyMatrix GaussElimination(MyMatrix matrix, MyMatrix vector, bool sparse)
        {
            GaussPartPivot Solvematrix = new GaussPartPivot(matrix, vector);
            Solvematrix.CreateUpperTriangularmatrix(sparse);
            Solvematrix.BackwardSubstitution();
            return Solvematrix.matrix;
        }
        protected GaussPartPivot(MyMatrix matrix, MyMatrix vector) : base(matrix, vector)
        {
        }

        protected void CreateUpperTriangularmatrix()
        {
            for (int column = 0; column < matrixSize; column++)
            {
                MaxColumnValueToTop(column);
                NullifyDownOfColumn(column);
            }
        }

        protected void CreateUpperTriangularmatrix(bool sparse)
        {
            for (int column = 0; column < matrixSize; column++)
            {
                MaxColumnValueToTop(column);
                NullifyDownOfColumn(column, sparse);
            }
        }
        private void MaxColumnValueToTop(int column)
        {
            int maxIndex = column;
            double maxValue = Math.Abs(matrix[column, column]);
            if (column == matrixSize - 1) return;
            for (int i = column; i < matrixSize; i++)
            {
                if (maxValue.CompareTo(Math.Abs(matrix[i, column])) < 0)
                {
                    maxIndex = i;
                    maxValue = matrix[i, column];
                }
            }
            if (column != maxIndex)
            {
                matrix.SwapRows(maxIndex, column);
                vector.SwapRows(maxIndex, column);
            }
        }
    }
}