using System;
using System.Collections.Generic;
using System.Linq;
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
                if(rows == cols)
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
        public bool[] VerifyTriangular(int triangular_type= 0, bool check_is_strict_triangular= false)
        {
            bool upper = true, lower = true, s_upper = true, s_lower = true;

            for (int i = 1; i < cols; i++)      // Upper
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

            for (int i = 0; i < cols; i++)         // lower
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

            return new bool[] { upper, s_upper, lower, s_lower };
        }
        public void fillMatrixWithData(int id)
        {
            Console.WriteLine("Insertion des données de la matrice "+id);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(string.Format("Donnée [{0},{1}]", i, j));
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
                        }
                    } while (!ok);
                }
            }
        }
    }
}
