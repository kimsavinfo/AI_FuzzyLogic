using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AI_FuzzyLogic.FuzzySetLib;

namespace AI_FuzzyLogic.FuzzySystemLib
{
    class LinguisticValue
    {
        private FuzzySet TheFuzzySet;
        private String Name;

        public LinguisticValue(String _name, FuzzySet _fuzzySet)
        {
            Name = _name;
            TheFuzzySet = _fuzzySet;
        }

        public FuzzySet getFuzzySet()
        {
            return TheFuzzySet;
        }

        public String getName()
        {
            return Name;
        }

        public double DegreeAtValue(double _value)
        {
            return TheFuzzySet.DegreeAtValue(_value);
        }
    }
}
