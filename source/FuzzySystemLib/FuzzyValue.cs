using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_FuzzyLogic.FuzzySystemLib
{
    class FuzzyValue
    {
        private LinguisticVariable TheLinguisticVariable;
        private double Value;

        public FuzzyValue(LinguisticVariable _linguisticVariable, double _value)
        {
            TheLinguisticVariable = _linguisticVariable;
            Value = _value;
        }

        public LinguisticVariable getLinguisticVariable()
        {
            return TheLinguisticVariable;
        }

        public double getValue()
        {
            return Value;
        }
    }
}
