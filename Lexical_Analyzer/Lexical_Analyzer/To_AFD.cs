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

        /// <summary>
        /// Create the Finite Deterministic Automaton base in a tree
        /// nodes, a list of followpos and mathematical set states
        /// </summary>
        /// <param name="root"></param>
        /// <param name="nodos"></param>
        /// <param name="followpos"></param>
        /// <returns></returns>
        public Dictionary<string, string> CreateAutomata (ExpressionNode root, 
            Dictionary<int, string> nodos, Dictionary<int, List<int>> followpos)
        {
            State state = new State();
            List<string> node_values = new List<string>();
            bool end_followpos = false;
            int state_num = 0;
            int count = 0;
            int current_state = 0;
            state.StateSet.Add("q" + state_num.ToString(), root.firstPos);
            state_num++;

            Dictionary<string, string> transicion_valor = new Dictionary<string, string>();

            //se inicia obteniendo los nodos y se valuan todos los datos

            node_values = ObtainNodeValues(nodos);
            List<int> temp_followpos = state.StateSet.ElementAt(0).Value;
            List<int> followPos_insert = new List<int>();

            while (!end_followpos)
            {
                //no se ha terminado de analizar todos los conjuntos
                //se inicia analizado el conjunto perteneciente a q0
                count++;

                for (int i = 0; i < node_values.Count; i++)
                {
                    //se toman los datos y se analizar para ver si se pueden hacer transiciones de estados
                    //se obtiene cada nodo del followpos y se evalua si el nodo contiene el dato a evaluar del listado de nodos
                    //si si, se crea un nuevo conjunto con el followpos o concatenacion del followpos

                    
                    for (int j = 0; j < nodos.Count; j++)
                    {
                        //obtenemos el dato del nodo
                        string dato = nodos.ElementAt(j).Value;

                        //verificamos si es igual al dato buscado, si es asi, se verifica tambien que el dato este en el conjunto de
                        //followpos

                        if (node_values.ElementAt(i) == dato)
                        {
                            //existe un dato similar
                            if (temp_followpos.Contains(j + 1))
                            {   //existe dentro del conjunto

                                if (followPos_insert.Count == 0)
                                {
                                    followPos_insert.AddRange(followpos.ElementAt(j).Value);
                                }
                                else
                                {
                                    for (int l = 0; l < temp_followpos.Count; l++)
                                    {
                                        if (followPos_insert.Contains(temp_followpos.ElementAt(l)))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            followPos_insert.Add(temp_followpos.ElementAt(l));
                                        }
                                    }
                                }

                                
                                //verificamos que el conjunto no exista dentro de los estados validos
                                
                                if (!state.StateSet.Values.Contains(followPos_insert))
                                {
                                    state.StateSet.Add("q" + state_num, followPos_insert);
                                }

                                
                            }

                            string name = "q" + current_state.ToString() + " / " + node_values.ElementAt(i).ToString();

                            //buscamos la transicion que corresponde al estado (o sea, obtener la llave asociada a tal followpos)
                            for (int k = 0; k < state.StateSet.Count; k++)
                            {
                                if (state.StateSet.ElementAt(k).Value == followPos_insert)
                                {
                                    transicion_valor.Add(name, state.StateSet.ElementAt(k).Key);
                                }
                            }

                            if (transicion_valor.Count != 0 && transicion_valor.ContainsKey(name) == false)
                            {   //significa que no se encontro ninguna transicion y se pone un default

                                transicion_valor.Add(name, "");
                            }
                        
                        }
                    }
                }

                if (count == state.StateSet.Count)
                {
                    end_followpos = true;
                }
            }


            return transicion_valor;
        }

        private List<string> ObtainNodeValues(Dictionary<int, string> nodos)
        {
            List<string> node_values = new List<string>();

            for (int i = 0; i < nodos.Count; i++)
            {
                if (node_values.Count == 0)
                {
                    node_values.Add(nodos.Values.ElementAt(i));
                }
                if (!node_values.Contains(nodos.ElementAt(i).Value))
                {
                    node_values.Add((nodos.Values.ElementAt(i)));
                }
            }

            return node_values;
        }
    }
}
