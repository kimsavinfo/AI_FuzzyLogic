using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_FuzzyLogic.FuzzySetLib
{
    class FuzzySet : IToString
    {
        protected List<Point2D> Points;
        protected double Min;
        protected double Max;

        public FuzzySet(double _min, double _max)
        {
            Min = _min;
            Max = _max;
            Points = new List<Point2D>();
        }

        #region getter

        public double getMin()
        {
            return Min;
        }

        public double getMax()
        {
            return Max;
        }

        public List<Point2D> getPoints()
        {
            return Points;
        }
        #endregion

        #region Add a point
        public void Add(Point2D _point)
        {
            Points.Add(_point);
            Points.Sort();
        }

        public void Add(double _x, double _y)
        {
            Add(new Point2D(_x, _y));
        }
        #endregion

        #region ToString
        string IToString.ToString()
        {
            // Show Min and Max
            String text = "[" + Min + "-" + Max + "]:";
            // Show all points with this format : (point.X;point.Y)
            foreach (Point2D aPoint in Points)
            {
                text += aPoint.ToString();
            }

            return text;
        }
        #endregion

        #region Degree
        // ========================================
        //      DEGREE
        // ========================================

        /// <summary>
        /// 3 cases possible :
        /// - the X value is out of the [Min;Max] range : degree = 0
        /// - the X value is in the fuzzy set : return Y value
        /// - there isn't ant point with this X value : calculate the interpolated value
        /// </summary>
        /// <param name="_xValue">X point's value</param>
        /// <returns>Point's deegre</returns>
        public double DegreeAtValue(double _xValue)
        {
            double degree = 0;

            // Is the X value in the [Min;Max] range ?
            if ((_xValue > Min && _xValue < Max))
            {
                degree = GetInterpolatedValue(_xValue);
            }

            return degree;
        }

        /// <summary>
        /// Calculate the interpolated value of a X value
        /// We can either already have a Y value or we have to use Thales algorithm.
        /// </summary>
        /// <param name="_xValue">X point's value</param>
        /// <returns>The interpolated value</returns>
        private double GetInterpolatedValue(double _xValue)
        {
            double interpolatedValue = 0;

            // Locating the previous and next point
            Point2D previous = Points.LastOrDefault(pt => pt.getX() <= _xValue);
            Point2D next = Points.FirstOrDefault(pt => pt.getX() >= _xValue);

            if (previous.Equals(next))
            {
                // X is already the coordinate of a defined point
                interpolatedValue = previous.getY();
            }
            else
            {
                // X is between two points, we have to use Thales algorithm
                interpolatedValue = (((previous.getY() - next.getY()) * (next.getX() - _xValue) / (next.getX() - previous.getX())) + next.getY());
            }

            return interpolatedValue;
        }
        #endregion

        #region Merge
        // ========================================
        //      Merge
        // ========================================

        /// <summary>
        /// Tells the sign value of the Y difference beetween the two points
        /// </summary>
        /// <param name="_pointA">A point</param>
        /// <param name="_pointB">Another point</param>
        /// <returns>
        /// -1 : (A.Y - B.Y) < 0
        /// 0 : (A.Y - B.Y) = 0
        /// 1 : (A.Y - B.Y) > 0
        /// </returns>
        private static int GetSignDifference(Point2D _pointA, Point2D _pointB)
        {
            return Math.Sign(_pointA.getY() - _pointB.getY());
        }

        /// <summary>
        /// Check if a position has changed
        /// </summary>
        /// <param name="_relativePosition">former relative position</param>
        /// <param name="_newRelativePosition">the new relatice position</param>
        /// <returns></returns>
        private static bool ArePositionDifferent(int _relativePosition, int _newRelativePosition)
        {
            return (_relativePosition != _newRelativePosition 
                    && _relativePosition != 0 
                    && _newRelativePosition != 0);
        }

        /// <summary>
        /// Merge two fuzzy set into one
        /// </summary>
        /// <param name="_fuzzySetA">A fuzzy set</param>
        /// <param name="_fuzzySetB">Another fuzzy set</param>
        /// <param name="_mergeFonction">Function for merging (Min or Max for example), 
        /// 3 doubles parameters :
        /// - 2 paramters for two double paramters
        /// - 1 paramter for a double to keep
        /// </param>
        /// <returns>the merged fuzzy set</returns>
        private static FuzzySet Merge(FuzzySet _fuzzySetA, FuzzySet _fuzzySetB, Func<double, double, double> _mergeFonction)
        {
            // ==== 1. Init
            FuzzySet mergedFuzzySet = new FuzzySet(Math.Min(_fuzzySetA.Min, _fuzzySetB.Min), Math.Max(_fuzzySetA.Max, _fuzzySetB.Max));
            
            List<Point2D>.Enumerator enumeratorA = _fuzzySetA.Points.GetEnumerator();
            List<Point2D>.Enumerator enumeratorB = _fuzzySetB.Points.GetEnumerator();
            enumeratorA.MoveNext();
            enumeratorB.MoveNext();
            Point2D oldPointA = enumeratorA.Current;

            int relativePosition = 0; // When do the fuzzy sets intersect ?
            int newRelativePosition = GetSignDifference(enumeratorA.Current, enumeratorB.Current);

            // ==== 2. Loop until the end of one enumerator
            Boolean endOfListA = false;
            Boolean endOfListB = false;
            while (!endOfListA && !endOfListB)
            {
                // Currents values
                double xA = enumeratorA.Current.getX();
                double xB = enumeratorB.Current.getX();
                relativePosition = newRelativePosition;
                newRelativePosition = GetSignDifference(enumeratorA.Current, enumeratorB.Current);

                if (ArePositionDifferent(relativePosition, newRelativePosition))
                {
                    // Calcule the points coordinates
                    double xMin = (xA == xB ? oldPointA.getX() : Math.Min(xA, xB));
                    double xMax = Math.Max(xA, xB);
                    // Find the intersection point
                    double slopeA = (_fuzzySetA.DegreeAtValue(xMax) - _fuzzySetA.DegreeAtValue(xMin)) / (xMax - xMin);
                    double slopeB = (_fuzzySetB.DegreeAtValue(xMax) - _fuzzySetB.DegreeAtValue(xMin)) / (xMax - xMin);
                    double delta = (_fuzzySetB.DegreeAtValue(xMin) - _fuzzySetA.DegreeAtValue(xMin)) / (slopeA - slopeB);
                    // Add the intersection point
                    mergedFuzzySet.Add(xMin + delta, _fuzzySetA.DegreeAtValue(xMin + delta));
                    
                    // Update numerators
                    if (xA < xB)
                    {
                        oldPointA = enumeratorA.Current;
                        endOfListA = !(enumeratorA.MoveNext());
                    }
                    else if (xA > xB)
                    {
                        endOfListB = !(enumeratorB.MoveNext());
                    }
                }
                else if (xA == xB)
                {
                    // They both share the same X coordinate
                    // We use the merge function to choose wich Y value we keep
                    mergedFuzzySet.Add(xA, _mergeFonction(enumeratorA.Current.getY(), enumeratorB.Current.getY()));
                    oldPointA = enumeratorA.Current;
                    endOfListA = !(enumeratorA.MoveNext());
                    endOfListB = !(enumeratorB.MoveNext());
                }
                else if (xA < xB)
                {
                    // The point from _fuzzySetA is the first
                    // We had its X value with the Y value returned by the merge fonction
                    // The merge function will choose beetween the Y enumeratorA's point and the _fuzzySetB's degreee
                    mergedFuzzySet.Add(xA, _mergeFonction(enumeratorA.Current.getY(), _fuzzySetB.DegreeAtValue(xA)));
                    oldPointA = enumeratorA.Current;
                    endOfListA = !(enumeratorA.MoveNext());
                }
                else
                {
                    // The point from _fuzzySetB is the first
                    // We had its X value with the Y value returned by the merge fonction
                    // The merge function will choose beetween the Y enumeratorB's point and the _fuzzySetA's degreee
                    mergedFuzzySet.Add(xB, _mergeFonction(enumeratorB.Current.getY(), _fuzzySetA.DegreeAtValue(xB)));
                    endOfListB = !(enumeratorB.MoveNext());
                }
            }

            // ==== 3. Merge the end of the other enumerator if needed
            if (!endOfListA)
            {
                while (!endOfListA)
                {
                    mergedFuzzySet.Add(enumeratorA.Current.getX(), _mergeFonction(enumeratorA.Current.getY(), 0));
                    endOfListA = !enumeratorA.MoveNext();
                }
            }
            else if (!endOfListB)
            {
                while (!endOfListB)
                {
                    mergedFuzzySet.Add(enumeratorB.Current.getX(), _mergeFonction(enumeratorB.Current.getY(), 0));
                    endOfListB = !enumeratorB.MoveNext();
                }
            }

            return mergedFuzzySet;
        }
        #endregion

        #region Centroid
        // ========================================
        //      Centroid
        // ========================================

        private bool IsThereAGravityCenter()
        {
            return (Points.Count < 2 ? false : true);
        }

        private void CalculateLocalCentroid(ref double _ponderatedArea, ref double _totalArea)
        {
            double localArea;
            double widthLocalArea;
            Point2D previousPoint = null;

            // Go throught all points couple which delemit the shape
            foreach (Point2D nextPoint in Points)
            {
                if (previousPoint != null)
                {
                    widthLocalArea = nextPoint.getX() - previousPoint.getX();

                    if (previousPoint.getY() == nextPoint.getY())
                    {
                        // The area is a rectangle : the local centroid is at the half
                        localArea = previousPoint.getY() * (nextPoint.getX() - previousPoint.getX());
                        _totalArea += localArea;
                        _ponderatedArea += ((widthLocalArea) / 2 + previousPoint.getX()) * localArea;
                    }
                    else
                    {
                        // The genral polygon is composed by a rectangle and a triangle
                        // So we have to calulate for each shape individualy

                        // For the rectangle : the local centroid is at the half
                        localArea = Math.Min(previousPoint.getY(), nextPoint.getY()) * (widthLocalArea);
                        _totalArea += localArea;
                        _ponderatedArea += ((widthLocalArea) / 2 + previousPoint.getX()) * localArea;

                        // For the triangle : the local centroid is 1/3 or 2/3. It depends on the slope
                        localArea = (widthLocalArea) * (Math.Abs(nextPoint.getY() - previousPoint.getY())) / 2;
                        _totalArea += localArea;
                        if (nextPoint.getY() > previousPoint.getY())
                        {
                            _ponderatedArea += (2.0 / 3.0 * (widthLocalArea) + previousPoint.getX()) * localArea;
                        }
                        else
                        {
                            _ponderatedArea += (1.0 / 3.0 * (widthLocalArea) + previousPoint.getX()) * localArea;
                        }
                    }
                }
                previousPoint = nextPoint;
            }
        }

        /// <summary>
        /// Calculate the centroid : the X coordinate of the gravity's center of a fuzzy set
        /// </summary>
        /// <returns>The centroid's X coordinate</returns>
        public double Centroid()
        {
            double xCentroid = 0;
            
            if (IsThereAGravityCenter())
            {
                // We compute the total area, and the ponderated one
                double ponderatedArea = 0;
                double totalArea = 0;

                CalculateLocalCentroid(ref ponderatedArea, ref totalArea);

                // The centroid is the result of the division of the two areas
                xCentroid = ponderatedArea / totalArea;
            }

            return xCentroid;
        }
        #endregion

        #region operators
        // ========================================
        //      OPERATORS
        // ========================================

        public static Boolean operator ==(FuzzySet _fuzzySetA, FuzzySet _fuzzySetB)
        {
            return _fuzzySetA.ToString().Equals(_fuzzySetB.ToString());
        }

        public static Boolean operator !=(FuzzySet _fuzzySetA, FuzzySet _fuzzySetB)
        {
            return !(_fuzzySetA == _fuzzySetB);
        }

        /// <summary>
        /// This function will multiply each y coordinate's points of a Fuzzy Set by the coefficent
        /// </summary>
        /// <param name="_fuzzySet">A fuzzy set</param>
        /// <param name="_coefficient">the coefficient will mutiply each y coordinate's points</param>
        /// <returns>the new fuzzy set</returns>
        public static FuzzySet operator *(FuzzySet _fuzzySet, double _coefficient)
        {
            FuzzySet multipliedFuzzySet = new FuzzySet(_fuzzySet.getMin(), _fuzzySet.getMax());

            foreach (Point2D aPoint in _fuzzySet.getPoints())
            {
                multipliedFuzzySet.Add(new Point2D(aPoint.getX(), aPoint.getY() * _coefficient));
            }

            return multipliedFuzzySet;
        }

        /// <summary>
        /// Create a fuzzy set with new y-intercept. Each points will have y-intercept = 1 - point.Y
        /// </summary>
        /// <param name="_fuzzySet">The fuzzy set to change</param>
        /// <returns>the new fuzzy set</returns>
        public static FuzzySet operator !(FuzzySet _fuzzySet)
        {
            FuzzySet newFuzzySet = new FuzzySet(_fuzzySet.getMin(), _fuzzySet.getMax());

            foreach (Point2D aPoint in _fuzzySet.getPoints())
            {
                newFuzzySet.Add(new Point2D(aPoint.getX(), 1 - aPoint.getY()));
            }

            return newFuzzySet;
        }

        public static FuzzySet operator &(FuzzySet _fuzzySetA, FuzzySet _fuzzySetB)
        {
            return Merge(_fuzzySetA, _fuzzySetB, Math.Min);
        }

        public static FuzzySet operator |(FuzzySet _fuzzySetA, FuzzySet _fuzzySetB)
        {
            return Merge(_fuzzySetA, _fuzzySetB, Math.Max);
        }
        #endregion
    }
}
