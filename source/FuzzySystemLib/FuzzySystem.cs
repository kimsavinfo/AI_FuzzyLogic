using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AI_FuzzyLogic.FuzzySetLib;

namespace AI_FuzzyLogic.FuzzySystemLib
{
    class FuzzySystem
    {
        String Name;
        List<LinguisticVariable> Inputs;
        LinguisticVariable Output;
        List<FuzzyRule> Rules;
        List<FuzzyValue> Problem;

        public FuzzySystem(String _name)
        {
            Name = _name;
            Inputs = new List<LinguisticVariable>();
            Rules = new List<FuzzyRule>();
            Problem = new List<FuzzyValue>();
        }

        public void addInputVariable(LinguisticVariable _linguisticVariable)
        {
            Inputs.Add(_linguisticVariable);
        }

        public void addOutputVariable(LinguisticVariable _linguisticVariable)
        {
            Output = _linguisticVariable;
        }

        public void addFuzzyRule(FuzzyRule _fuzzyRule)
        {
            Rules.Add(_fuzzyRule);
        }

        public void addFuzzyRule(string _ruleString)
        {
            FuzzyRule rule = new FuzzyRule(_ruleString, this);
            Rules.Add(rule);
        }
        public void SetInputVariable(LinguisticVariable _linguisticVariable, double _value)
        {
            Problem.Add(new FuzzyValue(_linguisticVariable, _value));
        }


        public double Solve()
        {
            // Apply the rules then calculate the fuzzy set
            FuzzySet res = new FuzzySet(Output.getMinValue(), Output.getMaxValue());
            res.Add(Output.getMinValue(), 0);
            res.Add(Output.getMaxValue(), 0);

            foreach (FuzzyRule rule in Rules)
            {
                // Calculate
                res = res | rule.Apply(Problem);
            }

            // Defuzzification !
            return res.Centroid();
        }

        public LinguisticVariable LinguisticVariableByName(string _name)
        {
            LinguisticVariable linguisticVariable = null;

            foreach (LinguisticVariable input in Inputs)
            {
                if (input.getName().ToUpper().Equals(_name))
                {
                    linguisticVariable = input;
                }
            }
            if (Output.getName().ToUpper().Equals(_name))
            {
                linguisticVariable = Output;
            }

            return linguisticVariable;
        }

        public void ResetCase()
        {
            Problem.Clear();
        }
    }
}
