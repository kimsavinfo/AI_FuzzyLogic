using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AI_FuzzyLogic.FuzzySetLib;

namespace AI_FuzzyLogic.FuzzySystemLib
{
    class LinguisticVariable
    {
        private String Name;
        List<LinguisticValue> Values;
        private Double MinValue;
        private Double MaxValue;

        public LinguisticVariable(String _name, double _min, double _max)
        {
            Values = new List<LinguisticValue>();
            Name = _name;
            MinValue = _min;
            MaxValue = _max;
        }

        public void AddValue(LinguisticValue lv)
        {
            Values.Add(lv);
        }

        public void AddValue(String _name, FuzzySet _fuzzySet)
        {
            Values.Add(new LinguisticValue(_name, _fuzzySet));
        }

        public void ClearValues()
        {
            Values.Clear();
        }

        public LinguisticValue LinguisticValueByName(string _name)
        {
            LinguisticValue linguisticValue = null;

            _name = _name.ToUpper();
            foreach (LinguisticValue aLV in Values)
            {
                if (aLV.getName().ToUpper().Equals(_name))
                {
                    linguisticValue = aLV;
                }
            }

            return linguisticValue;
        }

        public Double getMinValue()
        {
            return MinValue;
        }

        public Double getMaxValue()
        {
            return MaxValue;
        }

        public String getName()
        {
            return Name;
        }
    }
}
