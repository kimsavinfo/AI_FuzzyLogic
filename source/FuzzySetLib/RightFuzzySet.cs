using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_FuzzyLogic.FuzzySetLib
{
    // Graphical :
    //      ___
    //    /
    //   /
    //___
    class RightFuzzySet : FuzzySet
    {
        public RightFuzzySet(double _min, double _max, double _heightMin, double _baseMax)
            : base(_min, _max)
        {
            Add(new Point2D(_min, 0));
            Add(new Point2D(_heightMin, 0));
            Add(new Point2D(_baseMax, 1));
            Add(new Point2D(_max, 1));
        }
    }
}
