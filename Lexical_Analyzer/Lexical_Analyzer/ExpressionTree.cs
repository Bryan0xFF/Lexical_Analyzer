using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class ExpressionTree
    {
        static int values = 1;
        Dictionary<int, List<int>> followPos = new Dictionary<int, List<int>>();



        /// <summary>
        /// retorna el arbol creado con expresion posfix
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public ExpressionNode PrefixToBinaryTree(string expression)
        {
            Stack<string> operadores = new Stack<string>();
            Stack<ExpressionNode> datos = new Stack<ExpressionNode>();
            Stack<ExpressionNode> regexAnterior = new Stack<ExpressionNode>();
            Stack<ExpressionNode> opAnteriores = new Stack<ExpressionNode>();
            string chunk = "";
            string current = "";

            for (int i = 0; i < expression.Length; i++)
            {
                current = expression.Substring(i, 1);

                if (isDigit(current))
                {
                    chunk += current;
                    continue;
                }

                if (isOperator(current) && current != ")" && current != "(")
                {
                    if (i + 1 < expression.Length && i + 2 < expression.Length)
                    { //la siguiente es una palabra

                        if (current == "." && (expression.Substring(i + 1, 1) == ".") && expression.Substring(i + 2, 1) != ".")
                        {
                            //dato actual en i + 1 es una palabra
                            chunk += current;
                            continue;
                        }
                    }
                    if (i + 1 < expression.Length)
                    {
                        if (expression.Substring(i + 1, 1) == "Æ")
                        {
                            chunk += current;
                            ExpressionNode node = new ExpressionNode();
                            node.dato = chunk;
                            chunk = "";
                            datos.Push(node);
                            i++;
                            continue;
                        }
                    }

                    if (chunk != "")
                    {
                        ExpressionNode node = new ExpressionNode();
                        node.dato = chunk;
                        chunk = "";
                        datos.Push(node);
                    }

                    bool canOperate = true;
                    operadores.Push(current);


                    //tiene que verificar que si puede operar, de lo contrario continua agregando

                    while (canOperate == true)
                    {
                        canOperate = false;

                        if (operadores.Count > 0)
                        {
                            //validacion especial de | intermedio
                            if (operadores.Peek() == "|")
                            {
                                string temp = operadores.Pop();

                                if (operadores.Count > 0)
                                {

                                    if (operadores.Peek() == "." && datos.Count > 1)
                                    {
                                        ExpressionNode op = new ExpressionNode();
                                        op.dato = operadores.Pop();

                                        ExpressionNode node1 = datos.Pop();
                                        ExpressionNode node2 = datos.Pop();

                                        op.izquierdo = node2;
                                        op.derecho = node1;

                                        datos.Push(op);
                                        operadores.Push(temp);

                                        //se saca el operador y se guarda para validar si se puede operar la
                                        //siguiente expr y luego concatenar con |
                                        opAnteriores.Push(datos.Pop());
                                    }
                                    else if (datos.Count > 1)
                                    {
                                        ExpressionNode op = new ExpressionNode();
                                        op.dato = temp;

                                        ExpressionNode node1 = datos.Pop();
                                        ExpressionNode node2 = datos.Pop();

                                        op.izquierdo = node2;
                                        op.derecho = node1;

                                        datos.Push(op);
                                    }

                                    else
                                    {
                                        operadores.Push(current);
                                    }
                                }
                                else
                                {
                                    if (operadores.Count > 0 && datos.Count > 1)
                                    {
                                        if (operadores.Peek() == "|")
                                        {
                                            opAnteriores.Push(datos.Pop());
                                        }

                                    }
                                    operadores.Push(temp);
                                }

                            }
                            if (operadores.Peek() == ".")
                            {//necesita minimo 2 datos para operar
                                if (datos.Count > 1)
                                {
                                    canOperate = true;
                                }

                                if (canOperate)
                                {
                                    ExpressionNode op = new ExpressionNode();
                                    op.dato = operadores.Pop();

                                    ExpressionNode node1 = datos.Pop();
                                    ExpressionNode node2 = datos.Pop();

                                    op.izquierdo = node2;
                                    op.derecho = node1;

                                    datos.Push(op);
                                }
                            }

                            if (operadores.Peek() == "*" || operadores.Peek() == "+" || operadores.Peek() == "?")
                            {
                                if (datos.Count > 0)
                                {
                                    canOperate = true;
                                }

                                if (canOperate)
                                {
                                    ExpressionNode op = new ExpressionNode();
                                    op.dato = operadores.Pop();

                                    op.izquierdo = datos.Pop();

                                    datos.Push(op);
                                }
                            }
                        }

                    }

                }

                if (current == ")" && operadores.Count > 0)
                {
                    if (chunk != "")
                    {
                        ExpressionNode node = new ExpressionNode();
                        node.dato = chunk;
                        chunk = "";
                        datos.Push(node);
                    }


                    while (operadores.Peek() != "(" && operadores.Count > 1)
                    {

                        if (operadores.Peek() == ".")
                        {

                            ExpressionNode data = new ExpressionNode();
                            data.dato = operadores.Pop();

                            if (datos.Count >= 2)
                            {
                                ExpressionNode nodea = datos.Pop();
                                ExpressionNode nodeb = datos.Pop();

                                data.izquierdo = nodeb;
                                data.derecho = nodea;

                                datos.Push(data);
                            }
                            else if (datos.Count == 1 && opAnteriores.Count == 1)
                            {
                                ExpressionNode nodea = datos.Pop();
                                ExpressionNode nodeb = opAnteriores.Pop();

                                data.izquierdo = nodeb;
                                data.derecho = nodea;

                                datos.Push(data);
                            }
                            else if (datos.Count == 1 && regexAnterior.Count > 1)
                            {
                                ExpressionNode nodea = datos.Pop();
                                ExpressionNode nodeb = regexAnterior.Pop();

                                data.izquierdo = nodeb;
                                data.derecho = nodea;

                                datos.Push(data);
                            }
                            continue;
                        }

                        if (operadores.Peek() == "*" || operadores.Peek() == "+" || operadores.Peek() == "?")
                        {
                            ExpressionNode data = new ExpressionNode();
                            data.dato = operadores.Pop();

                            ExpressionNode nodea = datos.Pop();

                            data.izquierdo = nodea;
                            datos.Push(data);
                            continue;
                        }

                        if (operadores.Peek() == "|")
                        {// se trae un dato de regAnterior
                            ExpressionNode data = new ExpressionNode();
                            data.dato = operadores.Pop();

                            if (datos.Count == 2)
                            {
                                ExpressionNode nodea = datos.Pop();
                                ExpressionNode nodeb = datos.Pop();

                                data.izquierdo = nodeb;
                                data.derecho = nodea;

                            }
                            else
                            {
                                ExpressionNode nodea = opAnteriores.Pop();
                                ExpressionNode nodeb = datos.Pop();

                                data.izquierdo = nodea;
                                data.derecho = nodeb;

                            }



                            datos.Push(data);
                            continue;
                        }

                        operadores.Pop();
                    }

                    if (operadores.Count > 0)
                    {
                        if (operadores.Peek() == "(")
                        {
                            operadores.Pop();
                        }
                    }

                    if (operadores.Count > 0)
                    {
                        if (operadores.Peek() == "|")
                        {// se trae un dato de regAnterior
                         //p
                            ExpressionNode data = new ExpressionNode();
                            data.dato = operadores.Pop();

                            ExpressionNode nodea = regexAnterior.Pop();
                            ExpressionNode nodeb = datos.Pop();

                            data.izquierdo = nodea;
                            data.derecho = nodeb;

                            datos.Push(data);
                        }

                    }


                }


                if (current == "(")
                {
                    //si es un solo ( de salida, entonces ya opero e inserta al regex
                    if (datos.Count != 0 && operadores.Count == 1)
                    {
                        regexAnterior.Push(datos.Pop());
                    }
                    if (regexAnterior.Count == 2)
                    {
                        ExpressionNode opr = new ExpressionNode();
                        opr.dato = operadores.Pop();

                        ExpressionNode node1 = regexAnterior.Pop();
                        ExpressionNode node2 = regexAnterior.Pop();

                        opr.izquierdo = node2;
                        opr.derecho = node1;

                        regexAnterior.Push(opr);
                    }
                    else
                    {
                        if (datos.Count > 0)
                        {
                            regexAnterior.Push(datos.Pop());

                        }
                        operadores.Push(current);
                    }

                    continue;
                }





            }

            while (operadores.Count != 0)
            {
                if (chunk != "")
                {
                    ExpressionNode node = new ExpressionNode();
                    node.dato = chunk;
                    chunk = "";
                    datos.Push(node);
                }

                bool canOperate = false;

                if (operadores.Peek() == "|" || operadores.Peek() == ".")
                {//necesita minimo 2 datos para operar
                    if (datos.Count > 1)
                    {
                        canOperate = true;
                    }

                    if (canOperate)
                    {
                        ExpressionNode op = new ExpressionNode();
                        op.dato = operadores.Pop();

                        ExpressionNode node1 = datos.Pop();
                        ExpressionNode node2 = datos.Pop();

                        op.izquierdo = node2;
                        op.derecho = node1;

                        datos.Push(op);
                    }
                }

                if (operadores.Count > 0)
                {
                    if (operadores.Peek() == "*" || operadores.Peek() == "+" || operadores.Peek() == "?")
                    {
                        if (datos.Count > 0)
                        {
                            canOperate = true;
                        }

                        if (canOperate)
                        {
                            ExpressionNode op = new ExpressionNode();
                            op.dato = operadores.Pop();

                            op.izquierdo = datos.Pop();

                            datos.Push(op);
                        }
                    }
                }



            }

            return datos.Pop();
        }

        private void Operate(string v)
        {
            throw new NotImplementedException();
        }

        private bool isDigit(string dato)
        {
            try
            {
                string operators = "*|+.()?~";

                if (operators.Contains(dato))
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private bool isOperator(string op)
        {
            string operators = "*|+.?()~";

            if (operators.Contains(op))
            {
                return true;
            }

            return false;
        }

        private int Hierarchy(string op)
        {
            if (op == ")")
            {
                return int.MaxValue;
            }
            if (op == "(")
            {
                return int.MinValue;
            }
            if (op == "+" || op == "?" || op == "*")
                return 7;
            if (op == ".")
                return 6;
            if (op == "|")
                return 6;


            //nothing is selected and is returned a -1 instead.
            return -1;
        }

        public ExpressionNode assignRules(ExpressionNode root)
        {
            if (root.derecho == null && root.izquierdo == null)
            {
                root.firstPos.Add(values);
                root.lastPos.Add(values);
                values++;
            }
            else
            {
                assignRules(root.izquierdo);

                if (root.dato == "*")
                {
                    root.isNullable = true;
                }
                if (root.dato == "+")
                {
                    if (root.izquierdo.isNullable == false)
                    {
                        root.isNullable = false;
                    }
                    else
                    {
                        root.isNullable = true;
                    }
                }
                if (root.dato == "?")
                {
                    root.isNullable = true;
                }

                if (root.derecho != null)
                {
                    assignRules(root.derecho);
                }


                if (root.dato == ".")
                {
                    if (root.izquierdo != null && root.derecho != null)
                    {
                        if (root.izquierdo.isNullable == true && root.derecho.isNullable == true)
                        {
                            root.isNullable = true;
                        }
                        else
                        {
                            root.isNullable = false;
                        }
                    }
                }

                if (root.dato == "|")
                {
                    if (root.izquierdo != null && root.derecho != null)
                    {
                        if (root.izquierdo.isNullable == true || root.derecho.isNullable == true)
                        {
                            root.isNullable = true;
                        }
                        else
                        {
                            root.isNullable = false;
                        }
                    }
                }
            }

            return root;
        }

        public ExpressionNode AssignMidRules(ExpressionNode root)
        {
            if (root.izquierdo != null)
            {
                AssignMidRules(root.izquierdo);
            }

            if (root.derecho != null)
            {
                AssignMidRules(root.derecho);
            }

            if (root.izquierdo == null && root.derecho == null)
            {
                return root;
            }

            if (root.dato == "|" && root.derecho != null && root.izquierdo != null)
            {
                List<int> c1 = root.izquierdo.firstPos;
                List<int> c2 = root.derecho.firstPos;

                for (int i = 0; i < c1.Count; i++)
                {
                    root.firstPos.Add(c1.ElementAt(i));
                }

                for (int i = 0; i < c2.Count; i++)
                {
                    if (!root.firstPos.Contains(c2.ElementAt(i)))
                    {
                        root.firstPos.Add(c2.ElementAt(i));
                    }
                    else
                    {
                        continue;
                    }

                }

                //tomamos ahora los laspos

                c1 = root.izquierdo.lastPos;
                c2 = root.derecho.lastPos;

                for (int i = 0; i < c1.Count; i++)
                {
                    root.lastPos.Add(c1.ElementAt(i));
                }

                for (int i = 0; i < c2.Count; i++)
                {
                    if (!root.lastPos.Contains(c2.ElementAt(i)))
                    {
                        root.lastPos.Add(c2.ElementAt(i));
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            if (root.dato == "." && root.derecho != null && root.izquierdo != null)
            {
                //c1 es nullable
                if (root.izquierdo.isNullable == true)
                {
                    List<int> c1 = root.izquierdo.firstPos;
                    List<int> c2 = root.derecho.firstPos;

                    for (int i = 0; i < c1.Count; i++)
                    {
                        root.firstPos.Add(c1.ElementAt(i));
                    }

                    for (int i = 0; i < c2.Count; i++)
                    {
                        if (!root.firstPos.Contains(c2.ElementAt(i)))
                        {
                            root.firstPos.Add(c2.ElementAt(i));
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
                else
                {
                    root.firstPos = root.izquierdo.firstPos;
                }

                if (root.derecho.isNullable == true)
                {
                    List<int> c1 = root.izquierdo.lastPos;
                    List<int> c2 = root.derecho.lastPos;

                    for (int i = 0; i < c1.Count; i++)
                    {
                        root.lastPos.Add(c1.ElementAt(i));
                    }

                    for (int i = 0; i < c2.Count; i++)
                    {
                        if (!root.lastPos.Contains(c2.ElementAt(i)))
                        {
                            root.lastPos.Add(c2.ElementAt(i));
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
                else
                {
                    root.lastPos = root.derecho.lastPos;
                }
            }

            if (root.dato == "*" && root.izquierdo != null)
            {
                root.firstPos = root.izquierdo.firstPos;
                root.lastPos = root.izquierdo.lastPos;
            }

            if (root.dato == "+" && root.izquierdo != null)
            {
                root.firstPos = root.izquierdo.firstPos;
                root.lastPos = root.izquierdo.lastPos;
            }

            if (root.dato == "?" && root.izquierdo != null)
            {
                root.firstPos = root.izquierdo.firstPos;
                root.lastPos = root.izquierdo.lastPos;
            }

            return root;
        }


        public Dictionary<int, List<int>> AssignFollowPos(ExpressionNode root)
        {
            for (int i = 1; i < values; i++)
            {
                List<int> followpos = new List<int>();
                followPos.Add(i, followpos);
            }

            return followPos;
        }

        public Dictionary<int, List<int>> ComputeFPosConcat(ExpressionNode root, Dictionary<int, List<int>> followPos)
        {


            if (root.izquierdo == null && root.derecho == null)
            {
                return followPos;
            }

            if (root.izquierdo != null)
            {
              followPos =  ComputeFPosConcat(root.izquierdo, followPos);
            }
            if (root.derecho != null)
            {
                followPos = ComputeFPosConcat(root.derecho, followPos);
            }

            if (root.dato == ".")
            {
                List<int> firstc2 = root.derecho.firstPos;
                List<int> lastc1 = root.izquierdo.lastPos;

                for (int i = 0; i < lastc1.Count; i++)
                {
                    if (followPos[lastc1.ElementAt(i)].Count != 0)
                    {
                        
                        for (int j = 0; j < firstc2.Count; j++)
                        {
                            //si el nodo contiene algun elemento de la lista, entonces no lo agregara
                            if (followPos[lastc1.ElementAt(i)].Contains(firstc2.ElementAt(j)))
                            {
                                //followPos.ElementAt(lastc1.ElementAt(i)).Value.Contains(firstc2.ElementAt(j)
                                continue;
                            }
                            else
                            {
                                //followPos.ElementAt(lastc1.ElementAt(i)).Value.Add();
                                followPos[lastc1.ElementAt(i)].Add(firstc2.ElementAt(j));
                            }
                        }
                    }
                    else
                    {//si no tiene nada, entonces agregara
                        //followPos.ElementAt(lastc1.ElementAt(i)).Value = firstc2;

                        followPos[lastc1.ElementAt(i)] = firstc2;
                    }

                }
            }

            if ((root.dato == "*" || root.dato == "+" || root.dato == "?") && root.izquierdo != null)
            {
                List<int> firstc1 = root.izquierdo.firstPos;
                List<int> lastc1 = root.izquierdo.lastPos;

                for (int i = 0; i < lastc1.Count; i++)
                {
                    if (followPos.ElementAt(lastc1.ElementAt(i)).Value.Count != 0)
                    {
                        for (int j = 0; j < lastc1.Count; j++)
                        {
                            //si el nodo contiene algun elemento de la lista, entonces no lo agregara
                            if (followPos.ElementAt(lastc1.ElementAt(i)).Value.Contains(firstc1.ElementAt(j)))
                            {
                                continue;
                            }
                            else
                            {
                                followPos[lastc1.ElementAt(i)].Add(firstc1.ElementAt(j));
                            }
                        }
                    }
                    else
                    {//si no tiene nada, entonces agregara
                        //followPos.Add(lastc1.ElementAt(i), firstc1);
                        followPos[lastc1.ElementAt(i)] = firstc1;
                    }
                }

            }
            return followPos;
        }

        public Dictionary<int, string> ObtainLeafs(ExpressionNode root, Dictionary<int, string> datos)
        {
            if (root.izquierdo == null && root.derecho == null)
            {
                datos.Add(root.firstPos[0], root.dato);
            }

            if (root.izquierdo != null)
            {
                datos = ObtainLeafs(root.izquierdo, datos);
            }

            if (root.derecho != null)
            {
                datos = ObtainLeafs(root.derecho, datos);
            }

            return datos;
        }
    }
}
