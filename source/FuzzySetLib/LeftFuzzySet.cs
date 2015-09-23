using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_FuzzyLogic.FuzzySetLib
{
    // Graphical :
    //___
    //    \
    //     \
    //      ___
    class LeftFuzzySet : FuzzySet
    {
        public LeftFuzzySet(double _min, double _max, double _heightMax, double _baseMin) : base(_min, _max)
        {
            Add(new Point2D(_min, 1));
            Add(new Point2D(_heightMax, 1));
            Add(new Point2D(_baseMin, 0));
            Add(new Point2D(_max, 0));
        }
    }
}
