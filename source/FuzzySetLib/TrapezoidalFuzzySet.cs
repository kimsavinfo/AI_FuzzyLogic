using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_FuzzyLogic.FuzzySetLib
{
    // Graphical :
    //    _____
    //   |      \
    //___|       \_____
    class TrapezoidalFuzzySet : FuzzySet
    {
        public TrapezoidalFuzzySet(double _min, double _max, double _baseLeft, double _heightLeft, double _heightRight, double _baseRight) 
            : base(_min, _max)
        {
            Add(new Point2D(_min, 0));
            Add(new Point2D(_baseLeft, 0));
            Add(new Point2D(_heightLeft, 1));
            Add(new Point2D(_heightRight, 1));
            Add(new Point2D(_baseRight, 0));
            Add(new Point2D(_max, 0));
        }
    }
}
