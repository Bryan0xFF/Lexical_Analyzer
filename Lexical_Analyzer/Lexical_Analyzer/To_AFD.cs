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
        
    }
}
