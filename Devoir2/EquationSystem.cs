using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devoir2
{
    class EquationSystem
    {
        private Matrix a;
        private Matrix b;

        private int nbEquation;

        public Matrix A
        {
            get
            {
                return b;
            }
        }
        public Matrix B
        {
            get
            {
                return b;
            }
        }

        public EquationSystem(Matrix a, Matrix b)
        {
            this.a = a;
            nbEquation = a.Rows;
            this.b = b;
        }

        public bool VerifySystem()
        {
            bool matrixBOK = (b.Cols == 1 && b.Rows == nbEquation) || (b.Cols == nbEquation && b.Rows == 1);

            return a.IsSquare && matrixBOK;
        }

        public Matrix FindXByCramer()
        {
            double det = a.Determinant;
            if(det != 0)
            {
                b = Standarize(b);

                Matrix exxes = b.Clone();
                Matrix cofactors = a.Clone();

                for(int time = 0; time < exxes.Rows; time++)
                {
                    for(int row = 0; row < a.Rows; row++)
                    {
                        for(int col = 0;col < a.Cols; col++)
                        {
                            if(time != col)
                            {
                                cofactors.Data[row, col] = a.Data[row, col];
                            }
                            else
                            {
                                cofactors.Data[row, col] = b.Data[row, 0];
                            }
                        }
                    }
                    double tempDet = cofactors.Determinant;
                    exxes.setElement(time, 0, tempDet / det);
                }
                return exxes;
            }
            else
            {
                Console.WriteLine("Erreur ! Le déterminant de la matrice A est de 0");
                return null;
            }
        }

        private Matrix Standarize(Matrix b)
        {
            if(b.Cols == 1)
            {
                return b;
            }
            else
            {
                Matrix newB = new Matrix(b.Cols, 1);
                for(int i = 0; i< b.Cols; i++)
                {
                    newB.Data[i, 0] = b.Data[0, i];
                }
                return newB;
            }
        }

        public Matrix FindXByInversion()
        {
            double det = a.Determinant;
            if (det != 0)
            {
                b = Standarize(b);
                return a.Reversed.multiply(b);
            }
            else
            {
                Console.WriteLine("Erreur ! Le déterminant de la matrice A est de 0");
                return null;
            }
        }

        public Matrix FindXByJacobi(double epsilon)
        {
            throw new NotImplementedException();
        }
    }
}
