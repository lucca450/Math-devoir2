using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devoir2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Addition de matrices
            Matrix m1 = new Matrix (new double[,]
            {
                    {5, 0, 2},
                    {6, -4, 4},
                    {1, 4, 1}
            });

            Matrix m2 = new Matrix(new double[,]
            {
                    {4, 1, 0},
                    {4, -4, 3},
                    {2, -8, 1}
            });

            m1.addition(m2).display();

            //Test multiplication de matrices
            Matrix m3 = new Matrix(new double[,]
            {
                    {2, 4},
                    {-5, 1},
                    {-1, 0}
            });

            Matrix m4 = new Matrix(new double[,]
            {
                    {8, 6},
                    {-6, 7}
            });

            m3.multiply(m4).display();


            //Test produit scallaire
            Matrix m5 = new Matrix(new double[,]
            {
                    {5, 0, 2},
                    {6, -4, 4},
                    {1, 4, 1}
            });

            m5.scallarProduct(2).display();

            Console.ReadLine();
        }
    }
}
