/// ==================================================================================================================
/// Zrobione:
///     Klasa MyMatrix przeniesiona na tyle na ile byla potrzeba dx
///     Zaimplementowana metoda Jacobiego (watch?v=bR2SEe8W3Ig)
///     Zaimplementowana metoda Gaussa-Seidla (watch?v=F6J3ZmXkMj0)
///     Zaimplementowana metoda eliminacji Gaussa (poprzednie zadanie,)
/// Co trzeba zrobic:
///     Najlepiej byloby zrozumiec o co chodzi
///     Jak uzyc tych metod do obliczenia tych smiesznych prawdopodobienstw
///     Implementacja metody Monte Carlo
///     Na pewno jeszcze tysiac zadan
/// ==================================================================================================================

namespace PopulationsProtocols
{
    #region Usings
    using System;
    #endregion
    class PopulationProtocols
    {
        static void Main(string[] args)
        {
            var m = new MyMatrix(3,4);
            m.addElement(5);
            m.addElement(-1);
            m.addElement(2);
            m.addElement(12);
            m.addElement(3);
            m.addElement(8);
            m.addElement(-2);
            m.addElement(-25);
            m.addElement(1);
            m.addElement(1);
            m.addElement(4);
            m.addElement(6);
            var x = m.Jacobi();
            var y = m.Seidel();
            for(int i = 0; i < m.Rows; i++) Console.WriteLine(x[i]);
            Console.WriteLine("\n");
            for (int i = 0; i < m.Rows; i++) Console.WriteLine(y[i]);
            var m_gauss = new MyMatrix(4, 5);
            m_gauss.addElement(4);
            m_gauss.addElement(-2);
            m_gauss.addElement(4);
            m_gauss.addElement(-2);
            m_gauss.addElement(8);
            m_gauss.addElement(3);
            m_gauss.addElement(1);
            m_gauss.addElement(4);
            m_gauss.addElement(2);
            m_gauss.addElement(7);
            m_gauss.addElement(2);
            m_gauss.addElement(4);
            m_gauss.addElement(2);
            m_gauss.addElement(1);
            m_gauss.addElement(10);
            m_gauss.addElement(2);
            m_gauss.addElement(-2);
            m_gauss.addElement(4);
            m_gauss.addElement(2);
            m_gauss.addElement(2);
            var z = m_gauss.Gauss();

            for (int i = 0; i < m_gauss.Rows; i++) Console.WriteLine(z[i]);
            Console.ReadKey();
        }
    }
}
