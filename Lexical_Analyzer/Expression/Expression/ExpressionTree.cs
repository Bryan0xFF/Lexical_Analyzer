using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expression
{
    class ExpressionTree
    {
        public string toPrefix(string expression)
        {

            string prefix = "";
            Stack<string> operators = new Stack<string>();

            for (int i = expression.Length; i > 0; i--)
            {
                string currentItem = expression.Substring(i - 1, 1);

                if (isDigit(currentItem))
                {
                    prefix = currentItem + prefix;
                    continue;
                }

                if (isOperator(currentItem))
                {
                    while (operators.Count != 0 && (Hierarchy(operators.Peek()) > Hierarchy(currentItem)))
                    {
                        prefix = operators.Pop() + prefix;
                    }
                    continue;
                }

                operators.Push(currentItem);
            }

            while (operators.Count != 0)
            {
                prefix = operators.Pop() + prefix;
            }

            return prefix;
        }

        /// <summary>
        /// retorna el arbol creado con expresion posfix
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public ExpressionNode PrefixToBinaryTree(string prefix, int i)
        {
            int index = i;
            ExpressionNode root = new ExpressionNode();

            string currentElement = prefix.Substring(index++, 1);

            if (isOperator(currentElement))
            {
                root.dato = currentElement;
                root.izquierdo = PrefixToBinaryTree(prefix, index);
                root.derecho = PrefixToBinaryTree(prefix, index);
                  
            }

            if (isDigit(currentElement))
            {
                root.dato = currentElement;
            }


            return root;
        }


        public bool isDigit(string dato)
        {
            try
            {
                string operators = "*/+.?";

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

        public bool isOperator(string op)
        {
            string operators = "*/+.?";

            if (operators.Contains(op))
            {
                return true;
            }

            return false;
        }

        public int Hierarchy(string op)
        {
            
            if (op == "+" || op == "?" || op == "." || op == "/")
                return 1;
            if (op == "*")
                return 2;
            //nothing is selected and is returned a -1 instead.
            return -1;
        }
    }

}
