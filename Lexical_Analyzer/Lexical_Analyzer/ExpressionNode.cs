using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class ExpressionNode
    {
        public List<int> firstPos;
        public List<int> lastPos;
        public List<int> followPos;
        public ExpressionNode parent;
        public string dato;
        public ExpressionNode derecho;
        public ExpressionNode izquierdo;
        public bool isNullable;

        public ExpressionNode()
        {
            firstPos = new List<int>();
            lastPos = new List<int>();
            followPos = new List<int>();
            parent = null;
            derecho = null;
            izquierdo = null;
            dato = "";
            isNullable = false;
        }
    }
}
