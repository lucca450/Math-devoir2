using System;
using System.Collections;
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

        static void test(double [,] data)
        {
            data = new double[1,1];
        }
        static void Main(string[] args)
        {
            bool done = false;
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

                    List<string> options = new List<string>();
                    options.Add("Oui");
                    options.Add("Non");

                    done = OptionSelection("Voulez-vous ajouter une autre matrice ?", options) == 1;
                }
                catch
                {
                    Console.WriteLine("Taille invalide");
                }
            } while (!done);

            done = false;
            do
            {
                List<string> options = new List<string>();
                options.Add("Additionner 2 matrices");
                options.Add("Faire produit scalaire");
                options.Add("Faire produit matriciel");
                options.Add("Vérifier si une matrice est triangulaire");
                options.Add("Calculer les propiriétés d'une matrice");
                options.Add("Résoudre un système d'équation");
                options.Add("Visualiser les matrices");
                options.Add("Quitter");

                int selectedOption = OptionSelection("Que voulez-vous faire?", options);
               
                Matrix resultMatrix;
                List<Matrix> operationMatrixes;
                switch (selectedOption)
                {
                    case 0:                                                                     // Additions
                        operationMatrixes = AskForMatrixes(matrixes, 2);
                        resultMatrix = operationMatrixes[0].addition(operationMatrixes[1]);
                        Console.WriteLine("Matrice résultant de l'addition: ");
                        resultMatrix.display();
                        matrixes.Add(resultMatrix);
                        break;
                    case 1:                                                                     // Multiplication par scalar
                        Matrix operationMatrix = AskForMatrixes(matrixes, 1)[0];
                        double scalar = AskForDouble("Entrez un scalaire");
                        resultMatrix = operationMatrix.scallarProduct(scalar);
                        Console.WriteLine("Matrice résultant de la multiplication avec un scalaire: ");
                        resultMatrix.display();
                        matrixes.Add(resultMatrix);
                        break;
                    case 2:                                                                     // Multiplication de matrices
                        operationMatrixes = AskForMatrixes(matrixes);;
                        operationMatrix = new Matrix(operationMatrixes[0]);
                        operationMatrixes.RemoveAt(0);
                        int nbProducts = 0;
                        resultMatrix = operationMatrix.MultiplyMultipleMatrixes(operationMatrixes, ref nbProducts);             // à Faire quand vincent est là
                        resultMatrix.display();
                        Console.WriteLine(string.Format("Opération effectuée avec {0} produits.", nbProducts));
                        matrixes.Add(resultMatrix);
                        break;
                    case 3:                                                                     // Triangularité
                        Matrix m = AskForMatrixes(matrixes, 1)[0];
                        if(m.IsSquare)
                        {
                            string initialMessage = "Que voulez-vous faire ?";

                            options = new List<string>();
                            options.Add("Vérifier la triangularité supérieure");
                            options.Add("Vérifier la triangularité inférieure");
                            options.Add("Peu importe");

                            selectedOption = OptionSelection(initialMessage, options);

                            List<string> options2 = new List<string>();
                            options2.Add("Oui");
                            options2.Add("Non");

                            int selectedOption2 = OptionSelection("Vérifier strictement ?", options2);

                            bool verificationResult = m.VerifyTriangular(selectedOption, selectedOption2);

                            switch (selectedOption)
                            {
                                case 0:                         //Supérieur
                                    if (selectedOption2 == 0)        //Vérif Strict?
                                        if (verificationResult)
                                            Console.WriteLine("La matrice est triangulaire supérieure stricte.");
                                        else
                                            Console.WriteLine("La matrice n'est pas triangulaire supérieure stricte.");
                                    else
                                        if (verificationResult)
                                        Console.WriteLine("La matrice est triangulaire supérieure.");
                                    else
                                        Console.WriteLine("La matrice n'est pas triangulaire supérieure.");
                                    break;
                                case 1:                         //Inférieur
                                    if (selectedOption2 == 0)        //Vérif Strict?
                                        if (verificationResult)
                                            Console.WriteLine("La matrice est triangulaire inférieure stricte.");
                                        else
                                            Console.WriteLine("La matrice n'est pas triangulaire inférieure stricte.");
                                    else
                                       if (verificationResult)
                                        Console.WriteLine("La matrice est triangulaire inférieure.");
                                    else
                                        Console.WriteLine("La matrice n'est pas triangulaire inférieure.");
                                    break;
                                case 2:                         //Peu importe
                                    if (selectedOption2 == 0)        //Vérif Strict?
                                        if (verificationResult)
                                            Console.WriteLine("La matrice est triangulaire stricte.");
                                        else
                                            Console.WriteLine("La matrice n'est pas triangulaire stricte.");
                                    else
                                       if (verificationResult)
                                        Console.WriteLine("La matrice est triangulaire.");
                                    else
                                        Console.WriteLine("La matrice n'est pas triangulaire.");
                                    break;
                            }
                        }
                        else
                            Console.WriteLine("La matrice doit etre carrée pour vérifier sa triangularité.");

                        break;
                    case 4:
                        m = AskForMatrixes(matrixes, 1)[0];
                        Console.WriteLine("Voici les propriétés de la matrice : \n");

                        Console.WriteLine("Trace : " + m.Trace);
                        Console.WriteLine("Déterminant : " + m.Determinant);

                        Console.WriteLine("Transposée : \n");
                        Matrix temp = m.Transpose;
                        if (temp != null)
                            temp.display();
                        else
                            Console.Write("La matrice n'est pas carrée");
                        Console.WriteLine();

                        Console.WriteLine("Comatrice : \n");
                        temp = m.CoMatrice;
                        if (temp != null)
                            temp.display();
                        else
                            Console.Write("La matrice n'est pas carrée");
                        Console.WriteLine();

                        Console.WriteLine("Matrice inversée : \n ");
                        temp = m.Reversed;
                        if (temp != null)
                            temp.display();
                        else
                            Console.Write("La matrice n'est pas carrée ou a un déterminant de 0");

                        string isSquare = m.IsSquare ? "Oui" : "Non";
                        Console.WriteLine("Matrice carrée ? " + isSquare);
                        string isRegular = m.IsRegular ? "Oui" : "Non";
                        Console.WriteLine("Matrice réguliaire ? " + isRegular);
                        break;
                    case 5:
                        operationMatrixes = AskForMatrixes(matrixes, 2);
                        EquationSystem es = new EquationSystem(operationMatrixes[0], operationMatrixes[1]);
                        bool validated = es.VerifySystem();
                        if (validated)
                        {
                            string initialMessage = "Comment voulez-vous le faire ?";

                            options = new List<string>();
                            options.Add("Méthode Cramer");
                            options.Add("Méthode Inversion Matricielle");
                            options.Add("Méthode Jacobi");

                            selectedOption = OptionSelection(initialMessage, options);

                            Matrix xValues = null;
                            switch (selectedOption)
                            {
                                case 0:
                                    xValues = es.FindXByCramer();
                                    break;
                                case 1:
                                    xValues = es.FindXByInversion();
                                    break;
                                case 2:
                                    double epsilon = AskForDouble("Entrez la valeur de l'epsilon");
                                    xValues = es.FindXByJacobi(epsilon);
                                    break;
                            }
                            if (xValues != null)
                            {
                                Console.WriteLine("Valeurs de X : ");
                                xValues.display();
                            }
                        }
                        break;
                    case 6:
                        DisplayMatrixes(matrixes);
                        break;
                    case 7:
                        done = true;
                        break;
                    default:
                        Console.WriteLine("Something Wrong");
                        break;
                }
            } while (!done);
            Console.ReadLine();
        }

        private static int OptionSelection(string initialMessage, List<string> options)
        {
            int max = options.Count;
            bool ok = false;
            do
            {
                Console.WriteLine(initialMessage);
                for(int i = 1; i <= max ; i++)
                    Console.WriteLine(string.Format("{0}- {1}", i, options[i-1]));
                try
                {
                    int choosenID = int.Parse(Console.ReadLine());
                    if (choosenID >= 1 && choosenID <= max)
                    {
                        ok = true;
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

            bool again = false;
            do
            {
                Console.WriteLine(string.Format("Choissisez la matrice #{0} :",choosen.Count+1));
                int choosenID = ChooseIntegerOption(matrixes.Count);
                choosen.Add(matrixes[choosenID]);

                if (max != -1)
                {
                    again = choosen.Count != max;
                }
                else
                {
                    List<string> options = new List<string>();
                    options.Add("Oui");
                    options.Add("Non");

                    again = OptionSelection("Voulez-vous choisir une autre matrice ?", options) == 0;
                }
            } while (again);

            return choosen;
        }

        private static double AskForDouble(string message)
        {
            bool ok = false;
            do
            {
                Console.Write(message + " : ");
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
