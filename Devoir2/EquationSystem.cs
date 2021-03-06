using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devoir2
{
    class EquationSystem
    {
        private Matrix a;
        private Matrix b;
        private List<string> equations = new List<string>();
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
            if (VerifyDiagonallyDominant(a))
            {
                Matrix d = new Matrix(a.Cols, a.Cols), l = new Matrix(a.Cols, a.Cols), u = new Matrix(a.Cols, a.Cols);

                for(int i =0; i < a.Cols; i++)
                {
                    for(int j = 0; j< a.Cols; j++)
                    {
                        if (i == j)
                            d.Data[i, j] = a.Data[i, j];
                        else if (j < i)
                            l.Data[i, j] = a.Data[i, j];
                        else
                            u.Data[i, j] = a.Data[i, j];
                    }
                }

                Matrix lNu = l.addition(u);

                BuildLinearEquations(d, lNu.scallarProduct(-1), b);
                Matrix exxes = FindXValuesFromEquations(b.Rows, epsilon);
                return exxes;
            }
            Console.WriteLine("La matrice A n'est pas dominante diagolale stricte.");
            return null;
        }

        private Matrix FindXValuesFromEquations(int nbRows, double epsilon)
        {
            Matrix exxes = new Matrix(nbRows, 2);

            return FindXValuesFromEquations(exxes, epsilon, true);
        }
        private Matrix FindXValuesFromEquations(Matrix exxes, double epsilon, bool firstTime = false)
        {
            for(int i = 0; i< exxes.Rows;i++)
            {
                char var = 'a';
                string equation = equations[i];
                for (int j = 0; j < exxes.Rows; j++)
                {
                    equation = equation.Replace(var.ToString(), "*" + exxes.Data[j,0].ToString());
                    var++;
                }

                DataTable dt = new DataTable();
                double result = (double)dt.Compute(equation, "");

                exxes.Data[i, 1] = result;
            }

            if (firstTime)
            {
                return FindXValuesFromEquations(MoveExxes(exxes), epsilon);
            }
            else
            {
                bool differenceIsOk = true;

                for(int i = 0; i < exxes.Rows; i++)
                {
                    double diff = Math.Abs(exxes.Data[i, 1] - exxes.Data[i, 0]);
                    if (diff > epsilon)
                    {
                        differenceIsOk = false;
                        break;
                    }
                }

                if (!differenceIsOk)
                {
                    return FindXValuesFromEquations(MoveExxes(exxes), epsilon);
                }
                else
                {
                    Matrix m = new Matrix(exxes.Rows, 1);
                    for(int i = 0;i < exxes.Rows; i++)
                    {
                        m.Data[i, 0] = exxes.Data[i, 1];    
                    }
                    return m;
                }
            }
        }

        private Matrix MoveExxes(Matrix exxes)
        {
            for(int i = 0; i < exxes.Rows; i++)
            {
                exxes.Data[i, 0] = exxes.Data[i, 1];
            }
            return exxes;
        }

        private void BuildLinearEquations(Matrix d, Matrix m, Matrix b)
        {
            char var;
            char var2 = 'a';
            equations = new List<string>();
            for(int i = 0;i < m.Cols; i++)
            {
                var = 'a';
                string equation = "1/" + d.Data[i, i] + '*' + '(' + b.Data[i,0].ToString() + "+";
                for(int j = 0; j < m.Cols; j++)
                {
                    if(m.Data[i,j] != 0)
                    {
                        equation += m.Data[i, j] + var.ToString() + "+";
                    }
                    var++;
                }

                equation = equation.Remove(equation.Length - 1) + ')';

                equations.Add(equation);
                var2++;
            }
        }

        public override string ToString()
        {
            string str = "";
            char var;
            for (int i = 0; i < a.Rows; i++)
            {
                var = 'a';
                for(int j = 0;j<a.Cols; j++)
                {
                    if(a.Data[i,j] != 0)
                    {
                        str += a.Data[i, j] + var.ToString() + " + ";
                    }

                    var++;
                }
                if(str != "")
                    str = str.Remove(str.Length - 3).Replace("+-", "-") + " = " + b.Data[i,0] + '\n';
            }

            return str;
        }

        private bool VerifyDiagonallyDominant(Matrix m)
        {
            for(int i = 0; i< m.Cols; i++)
            {
                double diagonalValue = 0;
                double otherValues = 0;
                for(int j = 0; j< m.Cols; j++)
                {
                    if (i == j)
                        diagonalValue = Math.Abs(m.Data[i, j]);
                    else
                        otherValues += Math.Abs(m.Data[i, j]);
                }

                if (diagonalValue < otherValues)
                    return false;
            }
            return true;
        }
    }
}
