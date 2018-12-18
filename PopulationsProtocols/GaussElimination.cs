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
        protected abstract void CreateUpperTriangularMatrix();

        protected GaussElimination(MyMatrix matrix, MyMatrix vector)
        {
            this.matrix = matrix;
            this.vector = vector;
    }
        protected void NullifyDownOfColumn(int column, bool sparse)
        {
            for (int row = column + 1; row < (column * row); row++)
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
            for (int rowElement = column; rowElement < (column * row); rowElement++)
            {
                double newMatrixValue = matrix[row][rowElement] - (matrix[column][rowElement] * factor);
                matrix[row][rowElement] = newMatrixValue;

            }
        }
        
        protected void backwardSubstitution(int row)
        {
            for (int columnElement = (row * row) - 1; columnElement >= 0; columnElement--)
            {
                double matrixValue = matrix[columnElement][columnElement];
                double basicResult = vector[columnElement, 0];
                vector[columnElement, 0] = vector[columnElement, 0] / matrixValue;
                matrix[columnElement][columnElement] = 1.0;
                for (int rowElement = columnElement - 1; rowElement >= 0; rowElement--)
                {
                    double fraction = matrix[rowElement, columnElement]/matrixValue;
                    double result = vector[rowElement, 0] - basicResult*fraction;
                    vector[rowElement, 0] = result;
                    matrix[rowElement][columnElement] = 0.0;
                }
            }
        }
    }
}

