using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class ExpressionTree
    {
        static int i = 0;
        public string toPrefix(string expression)
        {

            string prefix = "";
            Stack<string> operators = new Stack<string>();

            for (int i = expression.Length; i > 0; i--)
            {
                string currentItem = expression.Substring(i - 1, 1);

                if (currentItem == "(" || currentItem == ")")
                {
                    continue;
                }

                if (isDigit(currentItem))
                {
                    prefix = currentItem + prefix;
                    continue;
                }

                if (isOperator(currentItem))
                {
                    
                    while (operators.Count != 0 && (Hierarchy(operators.Peek()) > Hierarchy(currentItem)) && (operators.Peek() != "("))
                    {
                        prefix = operators.Pop() + prefix;
                    }


                    /*
                    if (currentItem == "(")
                    {
                        while (operators.Peek() != ")")
                        {
                            prefix = operators.Pop() + prefix;
                            
                        }
                        //We take out the closing parentesis (needed because the while does not
                        //pops it out)
                        operators.Pop();
                        continue;
                    }
                    */
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
        public ExpressionNode PrefixToBinaryTree(string prefix)
        {
            try
            {
                ExpressionNode root = new ExpressionNode();

                string currentElement = prefix.Substring(i++, 1);

                if (currentElement == "(" || currentElement == ")")
                {
                    currentElement = prefix.Substring(i++, 1);
                }

                if (currentElement == "*" || currentElement == "+" || currentElement == "?")
                {
                    root.dato = currentElement;
                    root.izquierdo = PrefixToBinaryTree(prefix);
                }

                if (isOperator(currentElement) && currentElement != "*" && currentElement != "+" && currentElement != "?")
                {
                    root.dato = currentElement;
                    root.izquierdo = PrefixToBinaryTree(prefix);
                    root.derecho = PrefixToBinaryTree(prefix);

                }

                if (isDigit(currentElement))
                {
                    root.dato = currentElement;
                }


                return root;
            }
            catch (Exception)
            {

                return null;
            }
           
        }


        private bool isDigit(string dato)
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

        private bool isOperator(string op)
        {
            string operators = "*/+.?()";

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
                return 2;
            if (op == "/")
                return 2;
            //nothing is selected and is returned a -1 instead.
            return -1;
        }
    }
}
