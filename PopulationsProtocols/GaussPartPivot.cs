using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopulationsProtocols
{
    class GaussPartPivot
    {
        public static MyMatrix GaussElimination(MyMatrix matrix, MyMatrix vector)
        {
            GaussElimination SolveMatrix = new GaussPartPivot(matrix, vector);
            SolveMatrix.createUpperTriangularMatrix();
            SolveMatrix.backwardSubstitution();
            return SolveMatrix.matrix;
        }
        public static MyMatrix GaussElimination(MyMatrix matrix, MyMatrix vector, bool sparse)
        {
            GaussPartPivot SolveMatrix = new GaussPartPivot(matrix, vector);
            SolveMatrix.CreateUpperTriangularMatrix(sparse);
            SolveMatrix.BackwardSubstitution();
            return SolveMatrix.matrix;
        }
        protected GaussPartPivot(MyMatrix matrix, MyMatrix vector)
        {
            base.GaussElimination(matrix, vector);
        }

        protected void CreateUpperTriangularMatrix()
        {
            for (int column = 0; column < matrixSize; column++)
            {
                MaxColumnValueToTop(column);
                NullifyDownOfColumn(column); //sciagnac z gausselimination
            }
        }

        protected void CreateUpperTriangularMatrix(bool sparse)
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
            double maxValue = matrix.GetAbsElement(column, column);
            if (column == matrixSize - 1) return;
            for (int i = column; i < matrixSize; i++)
            {
                if (maxValue.compareTo((T)matrix.getAbsElement(i, column)) < 0)
                {
                    maxIndex = i;
                    maxValue = matrix.getElement(i, column);
                }
            }
            matrix.swapRows(maxIndex, column);
            vector.swapRows(maxIndex, column);
        }
    }


}
}
