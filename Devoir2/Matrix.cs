using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Devoir2
{
    class Matrix
    {
        private double[,] data;
        private int rows;
        private int cols;

        public Matrix(Matrix m)
        {
            this.data = m.data;
            this.rows = m.rows;
            this.cols = m.cols;
        }

        public Matrix(double[,] data)
        {
            this.data = data;
            this.rows = data.GetLength(0);
            this.cols = data.GetLength(1);
        }

        internal Matrix Clone()
        {
            Matrix other = (Matrix)this.MemberwiseClone();
            other.cols = cols;
            other.rows = rows;
            other.data = (double[,])data.Clone();
            return other;
        }

        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            data = new double[rows, cols];
        }

        public int Rows
        {
            get
            {
                return rows;
            }
        }
        public int Cols
        {
            get
            {
                return cols;
            }
        }
        public double[,] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
        public double Trace
        {
            get
            {
                if (IsSquare)
                {
                    double trace = 0;

                    for (int i = 0; i < rows; i++)
                    {
                        trace += data[i, i];
                    }
                    return trace;
                }
                return double.NaN;
            }
        }
        public Matrix Transpose
        {
            get
            {
                Matrix rm = new Matrix(cols, rows);

                for (int i = 0; i < cols; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        rm.data[i, j] = data[j, i];
                    }
                }
                return rm;
            }
        }
        public double Determinant
        {
            get
            {
                if (IsSquare)
                {
                    return Determinant_fct(Data, Cols);
                }
                return double.NaN;
            }
        }
        private double[,] getCofactor(double[,] data, int p, int q, int n)
        {
            int i = 0, j = 0;

            double[,] cofactors = new double[data.Length - 1, data.Length - 1];

            // Looping for each element of
            // the matrix
            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {

                    // Copying into temporary matrix
                    // only those element which are
                    // not in given row and column
                    if (row != p && col != q)
                    {
                        cofactors[i, j++] = data[row, col];

                        // Row is filled, so increase
                        // row index and reset col
                        // index
                        if (j == n - 1)
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }
            return cofactors;
        }
        private double Determinant_fct(double[,] data, int n)
        {
            int nbRows = n;
            if (nbRows == 1)
                return data[0, 0];
            else if (nbRows == 2)
            {
                return data[0, 0] * data[1, 1] - data[1, 0] * data[0, 1];
            }
            else
            {
                double D = 0;
                double[,] temp;
                int sign = 1;

                for (int f = 0; f < n; f++)
                {
                    temp = getCofactor(data, 0, f, n);
                    D += sign * data[0, f] * Determinant_fct(temp, n - 1);

                    sign = -sign;
                }
                return D;
            }
        }

        public Matrix CoMatrice 
        {
            get
            {
                if (IsSquare)
                {
                    Matrix m;
                    if (cols == 1)
                    {
                        m = new Matrix(1, 1);
                        m.data[0, 0] = 1;
                        return m;
                    }
                    else if(cols == 2)
                    {
                        m = Clone();

                        double temp = m.data[0, 1];
                        m.data[0, 1] = -m.data[1, 0];
                        m.data[1, 0] = -temp;

                        temp = m.data[0, 0];
                        m.data[0, 0] = m.data[1, 1];
                        m.data[1, 1] = temp;

                        return m;
                    }
                    else
                    {
                        int sign;
                        m = Clone();
                        double[,] cofactors;

                        for (int i = 0; i < m.cols; i++)
                        {
                            for (int j = 0; j < m.cols; j++)
                            {
                                cofactors = getCofactor(data, i, j, m.cols);

                                sign = ((i + j) % 2 == 0) ? 1 : -1;

                                double det = Determinant_fct(cofactors, m.cols - 1);

                                m.data[i,j] = sign * det;
                            }
                        }
                        return m;
                    }  
                }
                return null;
            }
        }

        public Matrix Reversed 
        {
            get
            {                
                if (IsSquare)
                {
                    double det = Determinant;
                    if(det != 0)
                    {
                        Matrix m = Clone().CoMatrice.Transpose.scallarProduct(1 / det);
                        return m;
                    }
                }
                return null;
            }
        }

        public bool IsSquare
        {
            get
            {
                return rows == cols;            
            }
        }

        public bool IsRegular { get; internal set; }

        public double getElement(int row, int col)
        {
            return Data[row,col];
        }
        public void setElement(int row, int col, double value)
        {
            Data[row, col] = value;
        }


        public void display(bool displayExxes = false)
        {
            if (rows == 0 && cols == 0 || rows == 0 && cols == 1)
            {
                Console.WriteLine("La matrice est vide");
                return;
            }
            char var = 'a';
            for (int i = 0; i < rows; i++)
            {
                string ex = "";
                if (displayExxes)
                {
                    ex = var.ToString() + " : ";
                }
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(ex + data[i, j] + "\t");
                }
                Console.WriteLine();
                var++;
            }
            Console.WriteLine('\n');
        }
        public Matrix addition(Matrix p_matrix)
        {
            if (rows == p_matrix.rows && cols == p_matrix.cols)
            {
                Matrix rm = new Matrix(rows,cols);

                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        rm.data[i, j] = data[i, j] + p_matrix.data[i, j];

                return rm;
            }
            return null;
        }

        public Matrix multiply(Matrix p_matrix)
        {
            int i = 0;
            return multiply(p_matrix, ref i);
        }

        public Matrix multiply(Matrix p_matrix, ref int nbProducts)
        {
            if (cols != p_matrix.rows)
            {
                Console.WriteLine("Le nombre de colonnes de la première matrice doit etre égal au nombre de lignes de la deuxième matrice."); 
            }
            Matrix rm = new Matrix(rows, p_matrix.cols);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < p_matrix.cols; j++)
                {
                    for (int z = 0; z < cols; z++)
                    {
                        rm.data[i, j] += data[i, z] * p_matrix.data[z, j];
                        nbProducts++;
                    }
                }
            }
            return rm;
        }
        public Matrix MultiplyMultipleMatrixes(List<Matrix> matrixes, ref int nbProducts)
        {
            Matrix result = Clone();
            bool workedOnce = false;

            int i = 2;
            foreach(Matrix m in matrixes)
            {
                if(result.cols == m.rows)
                {
                    workedOnce = true;
                    result = result.multiply(m, ref nbProducts);
                }
                else
                {
                    Console.WriteLine(string.Format("La matrice #{0} a été exclue du calcul, car son nombre de colonne doit etre égal au nombre de ligne de la premiere matrice ou du dernier résultat.",i));
                }
                i++;
            }

            if (workedOnce)
                return result;
            else
                return null;
        }
        public Matrix scallarProduct(double p_nbr)
        {
            Matrix rm = new Matrix(rows, cols);
            rm.data = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rm.data[i, j] = data[i, j] * p_nbr;
                }
            }
            return rm;
        }
        public bool VerifyTriangular(int option, int option2)
        {
            bool upper = true, s_upper = true, lower = true, s_lower = true;

            if (option == 0 || option == 2)                                     // Supérieur
            {
                for (int i = 0; i < cols; i++)
                {
                    if (data[i, i] != 0)
                        s_upper = false;
                    for (int j = 0; j < i; j++)
                        if (data[i, j] != 0)
                        {
                            upper = false;
                            s_upper = false;
                            break;
                        }
                    if (!upper) break;
                }
                if(option != 2)
                {
                    if (option2 == 0)                                   // Stricte
                        return s_upper;
                    else
                        return upper;
                }
            }

            if(option == 1 || option == 2)                                     // Inférieur
            {
                for (int i = 0; i < cols; i++)
                {
                    if (data[i, i] != 0)
                        s_lower = false;
                    for (int j = i + 1; j < cols; j++)
                        if (data[i, j] != 0)
                        {
                            lower = false;
                            s_lower = false;
                            break;
                        }
                    if (!lower) break;
                }
                if (option != 2)
                {
                    if (option2 == 0)                                   // Stricte                         
                        return s_lower;
                    else
                        return lower;
                }
            }

            if(option == 2)                                     // Peu importe
            {
                if (option2 == 0)
                    return s_upper || s_lower;
                else
                    return upper || lower;
            }
            return new bool();
        }
        public void fillMatrixWithData(int id)
        {
            Console.WriteLine("Insertion des données de la matrice "+id);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    bool ok;

                    do
                    {
                        Console.Write(string.Format("Donnée [{0},{1}] : ", i, j));
                        try
                        {
                            double value = double.Parse(Console.ReadLine());
                            data[i, j] = value;
                            ok = true;
                        }
                        catch
                        {
                            Console.WriteLine("La donnée doit être un chiffre");
                            ok = false;
                        }
                    } while (!ok);
                }
            }
        }
    }
}
