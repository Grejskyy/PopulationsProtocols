using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PopulationsProtocols
{
    abstract class GaussElimination
    {
        protected MyMatrix matrix;
        protected MyMatrix vector;
        protected int matrixSize;
        protected abstract void CreateUpperTriangularMatrix();

        protected GaussElimination(MyMatrix matrix, MyMatrix vector)
        {
            this.matrix = matrix;
            this.vector = vector;
        }
        protected void NullifyDownOfColumn(int column, bool sparse)
        {
            for (int row = column + 1; row < matrixSize; row++)
            {
                if (sparse && matrix.IsZero(row, column)) continue;
                double factor = matrix[row][column] / matrix[column][column];
                UpdateRow(column, row, factor);
                matrix[row][0] = matrix[row][0] - (matrix[column][0] * factor);
            }
        }
        protected void NullifyDownOfColumn(int column)
        {
            NullifyDownOfColumn(column, false);
        }
        private void UpdateRow(int column, int row, double factor)
        {
            for (int rowElement = column; rowElement < matrixSize; rowElement++)
            {
                double newMatrixValue = matrix[row][rowElement] - (matrix[column][rowElement] * factor);
                matrix[row][rowElement] = newMatrixValue;

            }
        }
    }
}

