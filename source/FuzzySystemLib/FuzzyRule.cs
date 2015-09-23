using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AI_FuzzyLogic.FuzzySetLib;

namespace AI_FuzzyLogic.FuzzySystemLib
{
    class FuzzyRule
    {
        List<FuzzyExpression> Premises;
        FuzzyExpression Conclusion;

        public FuzzyRule(List<FuzzyExpression> _premises, FuzzyExpression _conclusion)
        {
            Premises = _premises;
            Conclusion = _conclusion;
        }

        public FuzzyRule(string _ruleString, FuzzySystem _fuzzySystem)
        {
            _ruleString = _ruleString.ToUpper();

            // Split premises and conclusion
            String[] rule = _ruleString.Split(new String[] { " THEN " }, StringSplitOptions.RemoveEmptyEntries);
            if (rule.Length == 2)
            {
                // Compute and add premises
                rule[0] = rule[0].Remove(0, 2); // Remove tje "IF"
                String[] prem = rule[0].Trim().Split(new String[] { " AND " }, StringSplitOptions.RemoveEmptyEntries);
                Premises = new List<FuzzyExpression>();
                foreach (String exp in prem)
                {
                    String[] res = exp.Split(new String[] { " IS " }, StringSplitOptions.RemoveEmptyEntries);
                    if (res.Length == 2)
                    {
                        FuzzyExpression fexp = new FuzzyExpression(_fuzzySystem.LinguisticVariableByName(res[0]), res[1]);
                        Premises.Add(fexp);
                    }
                }
                // Add the conclusion
                String[] conclu = rule[1].Split(new String[] { " IS " }, StringSplitOptions.RemoveEmptyEntries);
                if (conclu.Length == 2)
                {
                    Conclusion = new FuzzyExpression(_fuzzySystem.LinguisticVariableByName(conclu[0]), conclu[1]);
                }
            }
        }

        public FuzzySet Apply(List<FuzzyValue> Problem)
        {
            FuzzySet fuzzySet = null;
            bool haveInfoForProblem = false;
            // Compute the degree for the whole rule : the min degree from all the premises
            double degree = 1;

            foreach (FuzzyExpression premise in Premises)
            {
                double localDegree = 0;
                LinguisticValue linguisticValue = null;

                // Search the premise in the problem :
                // Is there a value for this problem ? (For the human : yes but...)
                foreach (FuzzyValue pb in Problem)
                {
                    if (premise.getLinguisticVariable() == pb.getLinguisticVariable())
                    {
                        // A Linguistic Variable is found
                        // Search for the Linguistic Value
                        linguisticValue = premise.getLinguisticVariable().LinguisticValueByName(premise.getName());
                        if (linguisticValue != null)
                        {
                            // This is the fuzzyfication !
                            localDegree = linguisticValue.DegreeAtValue(pb.getValue());
                            break;
                        }
                    }
                }
                if (linguisticValue != null)
                {
                    // Save the degree and deal with the following degree
                    degree = Math.Min(degree, localDegree);
                    haveInfoForProblem = true;
                }
            }

            if (haveInfoForProblem)
            {
                //The result the fuzzy set * degree
                LinguisticValue lv = Conclusion.getLinguisticVariable().LinguisticValueByName(Conclusion.getName());
                fuzzySet = lv.getFuzzySet() * degree;
            }
            
            return fuzzySet;
        }

    }
}
