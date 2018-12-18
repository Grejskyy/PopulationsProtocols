using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopulationsProtocols
{
    class GaussElimination
    {
        public MyMatrix matrix;
        public MyMatrix vector;
        public int matrixSize;

        public GaussElimination(MyMatrix mmatrix, MyMatrix vvector)
        {
            this.matrix.Copy(mmatrix);
            matrixSize = matrix.GetSize();
            this.vector.Copy(vvector);
        }

        public void NullifyDownOfColumn(int column, bool sparse)
        {
            for (int row = column + 1; row < matrixSize; row++)
            {
                if (sparse && matrix.IsZero(row, column)) continue;
                double factor = matrix[row,column] / matrix[column,column];
                UpdateRow(column, row, factor);
                matrix[row,0] = matrix[row,0] - (matrix[column,0] * factor);
            }
        }
        public void NullifyDownOfColumn(int column)
        {
            NullifyDownOfColumn(column, false);
        }
        private void UpdateRow(int column, int row, double factor)
        {
            for (int rowElement = column; rowElement < matrixSize; rowElement++)
            {
                double newmatrixValue = matrix[row,rowElement] - (matrix[column,rowElement] * factor);
                matrix[row,rowElement] = newmatrixValue;

            }
        }
        
        public void BackwardSubstitution()
        {
            for (int columnElement = matrixSize - 1; columnElement >= 0; columnElement--)
            {
                double matrixValue = matrix[columnElement,columnElement];
                double basicResult = vector[columnElement, 0];
                vector[columnElement, 0] = vector[columnElement, 0] / matrixValue;
                matrix[columnElement,columnElement] = 1.0;
                for (int rowElement = columnElement - 1; rowElement >= 0; rowElement--)
                {
                    double fraction = matrix[rowElement, columnElement]/matrixValue;
                    double result = vector[rowElement, 0] - basicResult*fraction;
                    vector[rowElement, 0] = result;
                    matrix[rowElement,columnElement] = 0.0;
                }
            }
        }
    }
}
