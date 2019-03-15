using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lexical_Analyzer
{
    class To_AFD
    {
        //Introducir el RegEx al arbol
        //recorrerlo para introducir las reglas

        /// <summary>
        /// se introduce la regex y se crea el arbol de operaciones para 
        /// transformar a AFD
        /// </summary>
        /// <param name="regex"></param>
        public ExpressionNode CreateTree(string regex)
        {
            ExpressionTree tree = new ExpressionTree();
            ExpressionNode root = tree.PrefixToBinaryTree(regex);

            return root;
            
        }

        public Dictionary<string, List<string>> CreateAutomata (ExpressionNode root, 
            Dictionary<int, string> nodos, Dictionary<int, List<int>> followpos)
        {
            //empieza con el estado A
            List<int> A = root.firstPos;
            List<int> temp = new List<int>();
            List<List<int>> transiciones = new List<List<int>>();
            bool salida = true;
            //sirve para recorrer las nuevas transiciones creadas, hasta que llega a un punto de fin y hace que termine el algoritmo
            int integer_transiciones = 0;
            Queue<Dictionary<string, List<int>>> colaTransiciones = new Queue<Dictionary<string, List<int>>>();

            //de mi conjunto A, a que dato puedo ir con ese conjunto
            while (salida == true)
            {
                salida = false;
                transiciones.Add(A);

                for (int i = 1; i <= nodos.Count; i++)
                {//se obtiene el dato actual
                    string datoActual = nodos[i];
                    int valueActual = i;

                    //tiene el followpos del estado actual
                    if (A.Contains(valueActual))
                    {
                        temp = followpos[valueActual];

                        if (transiciones.Contains(temp))
                        {
                            //no se agre como transicion extra
                        }
                        else
                        {
                            transiciones.Add(temp);
                        }
                    }

                    
                }
            }

            return null;
        }
        
    }
}
