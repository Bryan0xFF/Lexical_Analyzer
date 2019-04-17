using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    public class State
    {
        public Dictionary<string, List<int>> StateSet;

        public State()
        {
            StateSet = new Dictionary<string, List<int>>();
        }
    }
}
