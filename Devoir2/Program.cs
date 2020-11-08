using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Devoir2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Matrix m0 = new Matrix(new double[,]
            {
                    //pour tester parametre 0 ( triangulaire supérieure)
                    /*
                    {5, 0, 2},
                    {0, -4, 4},
                    {0, 0, 1}
                    */
                    /*
                    {5, 0, 2},
                    {1, -4, 4},
                    {0, 0, 1}
                    */
                    /*
                    {0, 0, 2},
                    {0, 0, 4},
                    {0, 0, 0}
                    */

                    //pour tester parametre 0 ( triangulaire inférieure)
                    /*
                    {5, 0, 0},
                    {1, 4, 0},
                    {4, 5, 1}
                    */
                    
                   /*
                    {5, 0, 0},
                    {1, -4, 4},
                    {4, 5, 1}
                    */

                   
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0}
                    
                    
            });



            /*Console.WriteLine("Est-elle triangulaire? : " + m0.VerifyTriangular(0, true));
            Console.WriteLine("Est-elle triangulaire? : " + m0.VerifyTriangular(1, false));

            //Trace de la matrice
            /*Matrix m12 = new Matrix(new double[,]
            {
                /*
                    {1, 2, 3},
                    {4, 5, 6}
                */
            /*{1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
    });
    Console.WriteLine("Quel est sa trace? " + m12.getTrace());

    //Trace de la matrice
    Matrix m11 = new Matrix(new double[,]
    {

            /* {1, 2, 3},
             {4, 5, 6}*/


            /*{2, 6, 8},
            {10, 12, 14},
            {16, 18, 20}


});
Console.WriteLine("Quel est sa matrice transposée? ");
m11.transpose().display();

//Addition de matrices
Matrix m1 = new Matrix(new double[,]
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
//m3.multiply(m4).display();

//Test produit scallaire
Matrix m5 = new Matrix(new double[,]
{
        {5, 0, 2},
        {6, -4, 4},
        {1, 4, 1}
});
m5.scallarProduct(2).display();*/

            bool done = false;
            int i = 0;
            List<Matrix> matrixes = new List<Matrix>();
            do
            {
                Console.WriteLine("Matrice " + (matrixes.Count + 1) + " :");
                Console.Write("Nombre de colonnes : ");
                int nbCol, nbRow;
                try
                {
                    nbCol = int.Parse(Console.ReadLine());
                    Console.Write("Nombre de lignes : ");
                    nbRow = int.Parse(Console.ReadLine());

                    Matrix newMatrix = new Matrix(nbRow, nbCol);

                    newMatrix.fillMatrixWithData(matrixes.Count + 1);

                    matrixes.Add(newMatrix);

                    bool ok = false;

                    do
                    {
                        Console.WriteLine("Voulez-vous ajouter une autre matrice (o,n)?");
                        string ans = Console.ReadLine();
                        switch (ans)
                        {
                            case "o":
                            case "O":
                                done = false;
                                ok = true;
                                break;
                            case "N":
                            case "n":
                                done = true;
                                ok = true;
                                break;
                            default:
                                ok = false;
                                Console.WriteLine("Réponse incorrecte");
                                break;
                        }
                    } while (!ok);
                }
                catch
                {
                    Console.WriteLine("Taille invalide");
                }
            } while (!done);

            done = false;
            do
            {

                bool menuOk = true;
                do
                {
                    Console.WriteLine("Que voulez-vous faire ?");
                    Console.WriteLine(
                        "1- Additionner 2 matrices" + '\n' +
                        "2- Faire produit scalaire" + '\n' +
                        "3- Faire produit matriciel" + '\n' +
                        "4- Vérifier si une matrice est triangulaire" + '\n' +
                        "5- Calculer les propiriétés d'une matrice" + '\n' +
                        "6- Visualiser les matrices" + '\n' +
                        "7- Quitter");

                    string ans = Console.ReadLine();
                    Matrix resultMatrix;
                    List<Matrix> operationMatrixes;
                    switch (ans)
                    {
                        case "1":
                            operationMatrixes = AskForMatrixes(matrixes, 2);
                            resultMatrix = operationMatrixes[0].addition(operationMatrixes[1]);
                            Console.WriteLine("Matrice résultant de l'addition: ");
                            resultMatrix.display();
                            matrixes.Add(resultMatrix);
                            break;
                        case "2":
                            Matrix operationMatrix = AskForMatrixes(matrixes, 1)[0];
                            double scalar = AskForScalar();
                            resultMatrix = operationMatrix.scallarProduct(scalar);
                            Console.WriteLine("Matrice résultant de la multiplication avec un scalaire: ");
                            resultMatrix.display();
                            matrixes.Add(resultMatrix);
                            break;
                        case "3":
                            operationMatrixes = AskForMatrixes(matrixes);;
                            operationMatrix = new Matrix(operationMatrixes[0]);
                            operationMatrixes.RemoveAt(0);
                            int nbProducts = 0;
                            resultMatrix = operationMatrix.MultiplyMultipleMatrixes(operationMatrixes, ref nbProducts);             // à Faire quand vincent est là
                            resultMatrix.display();
                            Console.WriteLine(string.Format("Opération effectuée avec {0} produits.", nbProducts));
                            matrixes.Add(resultMatrix);
                            break;
                        case "4":
                            Matrix m = AskForMatrixes(matrixes, 1)[0];
                            if(m.Rows == m.Cols)
                            {
                                bool[] bools = m.VerifyTriangular();

                                if (bools[0])
                                    Console.WriteLine("La matrice est triangulaire supérieure.");
                                else
                                    Console.WriteLine("La matrice n'est pas triangulaire supérieure.");

                                if (bools[1])
                                    Console.WriteLine("La matrice est triangulaire supérieure stricte.");
                                else
                                    Console.WriteLine("La matrice n'est pas triangulaire supérieure stricte.");

                                if (bools[2])
                                    Console.WriteLine("La matrice est triangulaire inférieure.");
                                else
                                    Console.WriteLine("La matrice n'est pas triangulaire inférieure.");

                                if (bools[3])
                                    Console.WriteLine("La matrice est triangulaire inférieure stricte.");
                                else
                                    Console.WriteLine("La matrice n'est pas triangulaire inférieure stricte.");

                            }
                            else
                                Console.WriteLine("La matrice doit etre carrée pour vérifier sa triangularité.");

                            break;
                        case "5":
                            break;
                        case "6":
                            break;
                        case "7":
                            done = true;
                            break;
                        default:
                            menuOk = false;
                            Console.WriteLine("Mauvaise entrée");
                            break;
                    }
                    Console.WriteLine();
                } while (!menuOk);
                Console.WriteLine();
            } while (!done);
            Console.ReadLine();
        }

        private static void DisplayMatrixes(List<Matrix> matrixes)
        {
            int i = 1;
            foreach (Matrix m in matrixes)
            {
                Console.WriteLine("Matrice " + i++);
                m.display();
            }
        }

        private static int ChooseIntegerOption(int max)
        {
            bool ok = false;
            do
            {
                try
                {
                    int choosenID = int.Parse(Console.ReadLine());
                    if (choosenID >= 1 && choosenID <= max)
                    {
                        return choosenID - 1;
                    }
                    else
                        Console.WriteLine("Mauvaise entrée");
                }
                catch
                {
                    Console.WriteLine("Mauvaise entrée");
                }

            } while (!ok);
            return -1;
        }

        private static List<Matrix> AskForMatrixes(List<Matrix> matrixes, int max = -1)
        {
            List<Matrix> choosen = new List<Matrix>();
            DisplayMatrixes(matrixes);
            //Console.WriteLine("Choissisez une matrice :");

            bool again = false;
            do
            {
                Console.WriteLine("Choissisez une matrice :");
                int choosenID = ChooseIntegerOption(matrixes.Count);
                choosen.Add(matrixes[choosenID]);

                if (max == -1 || choosen.Count != max)
                {
                    again = AskForAnother();
                }
            } while (again);

            return choosen;
        }
        private static bool AskForAnother()
        {
            bool ok = true;
            do
            {
                Console.WriteLine("Voulez-vous choisir une autre matrice (o,n)?");
                string ans = Console.ReadLine();
                switch (ans)
                {
                    case "o":
                    case "O":
                        return true;
                    case "N":
                    case "n":
                        return false;
                    default:
                        ok = false;
                        Console.WriteLine("Réponse incorrecte");
                        break;
                }
            } while (!ok);
            return false;
        }

        private static double AskForScalar()
        {
            bool ok = false;
            do
            {
                Console.Write("Entrez un scalaire :");
                try
                {
                    return double.Parse(Console.ReadLine());
                }catch
                {
                    Console.WriteLine("Ceci n'est pas un scalaire");
                }
            } while (!ok);
            return -1;
        }
    }

}
