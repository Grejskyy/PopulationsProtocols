namespace PopulationsProtocols
{

    #region Usings

    using System;

    using System.Collections.Generic;

    #endregion

    class MyMatrix
    {
        #region Private Fields

        private List<double> _data = new List<double>();
        private int _rows;
        private int _cols;

        #endregion

        #region Constructors and Destructors

        public MyMatrix(int rows, int cols)
        {
            _cols = cols;
            _rows = rows;
            for (int i = 0; i < _rows * _cols; i++) _data.Add(0);
        }

        public MyMatrix(MyMatrix m)
        {
            _cols = m.Cols;
            _rows = m.Rows;
            for (int i = 0; i < _rows * _cols; i++) _data.Add(m[0, i]);
        }

        #endregion

        #region Public Methods and Operators

        public List<double> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public int Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        public int Cols
        {
            get { return _cols; }
            set { _cols = value; }
        }
        public bool IsZero(int x, int y)
        {
            return this[x, y] == 0;
        }

        public int GetSize() {
            return _rows;
        }

        public void SwapRows(int first, int second)
        {
            var temp = new MyMatrix(1, _rows);
            temp = this.GetRow(first);
            for (int i = 0; i < this.Cols; i++)
            {
                this[first, i] = this[second, i];
                this[second, i] = temp[0, i];
            }
        }
        public void Copy(MyMatrix m)
        {
            _rows = m.Rows;
            _cols = m.Cols;
            for (int i = 0; i < _rows * _cols; i++) this[0,i] = m[0, i];
        }
        public MyMatrix GetColumn(int column)
        {
            var result = new MyMatrix(_rows,1);
            for(int i = 0; i < _rows; i++)
            {
                result[i,0] = this[i, column];
            }
            return result;
        }

        public MyMatrix GetRow(int row)
        {
            var result = new MyMatrix(1, _cols);
            for (int i = 0; i < _cols; i++)
            {
                result[0, i] = this[row, i];
            }
            return result;
        }

        static public List<double> GetColumnAsList(MyMatrix m, int column)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < m._rows; i++)
            {
                result.Add(m[i, column]);
            }
            return result;
        }

        public void addElement(double element)
        {
            _data.Add(element);
        }

        public double this[int row, int col]
        {
            get { return _data[_cols * row + col]; }
            set { _data[_cols * row + col] = value; }
        }

        public List<double> Gauss(bool sparse)
        {
            var temp = new MyMatrix(this);
            double div = 0;
            int hlp = 0;
            for(int n = 0; n < _rows; n++)
            {
                hlp = 0;
                for (int i = n; i < _rows; i++)
                    if (Math.Abs(this[i, n]) > Math.Abs(this[hlp, n])) hlp = i;
                if (hlp != 0)
                {
                    for(int i = 0; i < _cols; i++)
                    {
                        this[n, i] = temp[hlp, i];
                        this[hlp, i] = temp[n, i];
                    }

                    temp.Copy(this);
                }
                for(int i = n+1; i < _rows; i++)
                {
                    if (this.IsZero(i, n) && sparse) continue;
                    div = temp[i, n] / temp[n, n];
                    for(int j = n; j < _cols; j++)
                    {
                        this[i, j] -= div * temp[n, j];
                    }
                }

                temp.Copy(this);
            }
            var x = new List<double>();
            double value = 0;
            for(int i = 1; i <= _rows; i++)
            {
                value = this[_rows - i, _cols - 1];
                for (int j = 2; j <= i; j++) value -= this[_rows - i, _cols - j] * x[j - 2];
                value /= this[_rows - i, _cols - (i + 1)];
                x.Add(value);
            }
            x.Reverse();
            return x;
        }

        public List<double> Jacobi()
        {
            List<double> xvalues = new List<double>();
            List<double> temp_xvalues = new List<double>();
            for (int i = _rows; i > 0; i--)
            {
                xvalues.Add(0);
                temp_xvalues.Add(0);
            }
            for (int iter = 0; iter < 2; iter++)
            {
                for (int i = 0; i < _rows; i++)
                {
                    xvalues[i] = this[i, _rows];
                    for (int j = 0; j < _rows; j++)
                    {
                        if (i == j) continue;
                        else
                        {
                            xvalues[i] -= this[i, j] * temp_xvalues[j];
                        }
                    }
                    xvalues[i] /= this[i, i];
                }
                for (int i = 0; i < _rows; i++) temp_xvalues[i] = xvalues[i];
            }
            return xvalues;
        }
        public List<double>Seidel()
        {
            List<double> xvalues = new List<double>();
            for (int i = _rows; i > 0; i--)
            {
                xvalues.Add(0);
            }
            for (int iter = 0; iter < 2; iter++)
            {
                for (int i = 0; i < _rows; i++)
                {
                    xvalues[i] = this[i, _rows];
                    for (int j = 0; j < _rows; j++)
                    {
                        if (i == j) continue;
                        else
                        {
                            xvalues[i] -= this[i, j] * xvalues[j];
                        }
                    }
                    xvalues[i] /= this[i, i];
                }
            }
            return xvalues;
        }

        #endregion
    }
}
