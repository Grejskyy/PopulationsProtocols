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
    protected abstract void CreateUpperTriangularMatrix ();

    protected GaussElimination(MyMatrix matrix, MyMatrix vector){
        this.matrix = matrix;
        matrixSize = matrix.GetSize();
        this.vector = vector;
    }
    protected void NullifyDownOfColumn(int column, bool sparse) {
        for (int row = column + 1; row < matrixSize; row++) {
            if(sparse && matrix.IsZero(row, column)) continue;
            Number factor = matrix.Divide(row, column, column, column);
            UpdateRow(column, row, factor);
            vector.SetElement(row, 0, vector.Subtract(row, 0, vector.MultiplyCell(column, 0, factor)));
        }
    }
    protected void NullifyDownOfColumn(int column) {
        NullifyDownOfColumn(column, false);
    }
    private void UpdateRow(int column, int row, Number factor) {
        for (int rowElement = column; rowElement < matrixSize; rowElement++) {
            Number newMatrixValue = matrix.Subtract(row, rowElement, matrix.MultiplyCell(column, rowElement, factor));
            matrix.SetElement(row, rowElement, newMatrixValue);
        }
    }

    protected void BackwardSubstitution(){
        for(int columnElement=matrixSize-1; columnElement>=0; columnElement--){
            double matrixValue = matrix.GetElement(columnElement, columnElement);
            double basicResult = vector.GetElement(columnElement, 0);
            vector.DivideByValue(columnElement, 0, matrixValue);
            matrix.SetOne(columnElement, columnElement);
            for(int rowElement=columnElement-1; rowElement>=0; rowElement--){
                double fraction = Divide(matrix.GetElement(rowElement, columnElement), matrixValue);
                doubleresult = Subtract(vector.GetElement(rowElement, 0), Multiply(basicResult, fraction));
                vector.SetElement(rowElement, 0, result);
                matrix.SetNull(rowElement, columnElement);
            }
        }
    }
    private double divide(double first, double second)
        {
            if (first instanceof Double)      return (Double)(((Double)first) / ((Double)second));
        }
        private double multiply(double first, double second)
        {
            if (first instanceof Double)      return (Double)(((Double)first) * ((Double)second));
        }
        private double subtract(double first, double second)
        {
            if (first instanceof Double)      return (Double)(((Double)first) - ((Double)second));
        }
    }
}
