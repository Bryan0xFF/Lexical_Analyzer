using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class ExpressionTree
    {



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

                                data.izquierdo = nodeb;
                                data.derecho = nodea;

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

                    if (operadores.Count > 0 )
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

        
    }
}
