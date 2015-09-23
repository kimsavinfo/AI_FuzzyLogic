using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_FuzzyLogic.FuzzySystemLib
{
    class FuzzyExpression
    {
        private LinguisticVariable TheLinguisticVariable;
        private String Name;

        public FuzzyExpression(LinguisticVariable _linguisticVariable, String _name)
        {
            TheLinguisticVariable = _linguisticVariable;
            Name = _name;
        }

        public LinguisticVariable getLinguisticVariable()
        {
            return TheLinguisticVariable;
        }

        public String getName()
        {
            return Name;
        }
    }
}
