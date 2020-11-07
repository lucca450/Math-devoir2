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

        public double[,] getData()
        {
            return data; 
        }

        public void setData(double[,] data)
        {
            this.data = data;
        }

        public void display()
        {
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

        public Matrix multiply(Matrix p_matrix)
        {
            if (cols != p_matrix.rows)
            {
                throw new InvalidOperationException ("Le nombre de colonnes de la première matrice doit etre égal au nombre de lignes de la deuxième matrice."); 
            }
            Matrix rm = new Matrix(rows,cols);
            rm.data = new double[rows, p_matrix.cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < p_matrix.cols; j++)
                {
                    for (int z = 0; z < cols; z++)
                    {
                        rm.data[i, j] += data[i, z] * p_matrix.data[z, j];
                    }
                }
            }
            return rm;
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

        public void fillMatrix()
        {
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    data[i, j] = 0;
                }
            }
        }


    }
}
