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
        private void getCofactor(double[,] data, double[,] temp, int p, int q, int n)
        {
            int i = 0, j = 0;

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
                        temp[i, j++] = data[row, col];

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
                // To store cofactors
                double[,] temp = new double[n, n];

                // To store sign multiplier
                int sign = 1;

                // Iterate for each element
                // of first row
                for (int f = 0; f < n; f++)
                {
                    // Getting Cofactor of mat[0][f]
                    getCofactor(data, temp, 0, f, n);
                    D += sign * data[0, f] * Determinant_fct(temp, n - 1);

                    // terms are to be added with
                    // alternate sign
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
                        double[,] temp = new double[cols, cols];

                        for (int i = 0; i < m.cols; i++)
                        {
                            for (int j = 0; j < m.cols; j++)
                            {
                                // Get cofactor of A[i,j] 
                                getCofactor(m.data, temp, i, j, m.cols);

                                // sign of adj[j,i] positive if sum of row 
                                // and column indexes is even. 
                                sign = ((i + j) % 2 == 0) ? 1 : -1;

                                // Interchanging rows and columns to get the 
                                // transpose of the cofactor matrix 
                                m.data[j, i] *= sign /** Determinant_fct(temp, m.cols - 1)*/;
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
                    Matrix m = Clone();
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            m.data[i, j] = 1 / m.data[i, j];
                        }
                    }
                    return m;
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


        public void display()
        {
            if (rows == 0 && cols == 0 || rows == 0 && cols == 1)
            {
                Console.WriteLine("La matrice est vide");
                return;
            }
            Console.WriteLine("Matrice: \n");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(data[i, j] + "\t");
                }
                Console.WriteLine();
            }
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
            Matrix result = new Matrix(this);

            int i = 0;
            foreach(Matrix m in matrixes)
            {
                if(result.cols == m.rows)
                {
                    result = result.multiply(m, ref nbProducts);
                }
                else
                {
                    Console.WriteLine(string.Format("La matrice #{0} a été exclue du calcul, car son nombre de colonne doit etre égal au nombre de ligne de la premiere matrice.",i));
                }
                i++;
            }
            return result;
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
            bool upper = false, s_upper = false, lower = false, s_lower = false;

            if (option == 0 || option == 2)                                     // Supérieur
            {
                for (int i = 1; i < cols; i++)
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
                if (option2 == 0)                                   // Stricte
                    return s_upper;
                else
                    return upper;
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
                if (option2 == 0)                                   // Stricte                         
                    return s_lower;
                else
                    return lower;
            }

            if(option == 2)                                     // Peu importe
            {
                return s_upper || s_lower;
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
                    Console.Write(string.Format("Donnée [{0},{1}] : ", i, j));
                    bool ok = false;

                    do
                    {
                        try
                        {
                            double value = double.Parse(Console.ReadLine());
                            data[i, j] = value;
                            ok = true;
                        }
                        catch
                        {
                            throw new Exception("La donnée doit être un chiffre");
                            ok = false;
                        }
                    } while (!ok);
                }
            }
        }
    }
}
