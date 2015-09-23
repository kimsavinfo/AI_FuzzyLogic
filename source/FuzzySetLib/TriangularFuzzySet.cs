using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_FuzzyLogic.FuzzySetLib
{
    // Graphical :
    // __/\__
    class TriangularFuzzySet : FuzzySet
    {
        public TriangularFuzzySet(double _min, double _max, double _triangleBegin, double _triangleCenter, double _triangleEnd) : base(_min, _max)
        {
            Add(new Point2D(_min, 0));
            Add(new Point2D(_triangleBegin, 0));
            Add(new Point2D(_triangleCenter, 1));
            Add(new Point2D(_triangleEnd, 0));
            Add(new Point2D(_max, 0));
        }
    }
}
