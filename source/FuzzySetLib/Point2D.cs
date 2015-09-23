using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_FuzzyLogic.FuzzySetLib
{
    /// Usual 2D point with X and Y axes
    class Point2D : IComparable, IToString
    {
        private double X;
        private double Y;

        public double getX()
        {
            return X;
        }

        public double getY()
        {
            return Y;
        }

        public Point2D(double _x, double _y)
        {
            X = _x;
            Y = _y;
        }

        
        string IToString.ToString()
        {
            return "(" + this.X + ";" + this.Y + ")";
        }

        /// <summary>
        ///  Only need to compare the abscissa value
        /// </summary>
        /// <param name="_object">The object we want to compare to</param>
        /// <returns>The object is smaller, the same size or bigger</returns>
        int IComparable.CompareTo(object obj)
        {
            return (int)(X - ((Point2D)obj).X);
        }
    }
}
