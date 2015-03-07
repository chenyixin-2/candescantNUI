using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using NDtw;
namespace NDtw.FeatureVector
    {
    public interface IFeatureVectorsData : IScalar, IVect3
    //public interface IFeatureVectorsData
    {
    }

    public interface IScalar
    {
        double Value { get; }
    }
    public interface IVect3
    {
        float X { get; }
        float Y { get; }
        float Z { get; }
    }

    public class Vec3 : IFeatureVectorsData
    {
        float _x, _y, _z;
        public Vec3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        public float X { get { return _x; } }
        public float Y { get { return _y; } }
        public float Z { get { return _z; } }
        public double Value
        {
            get
            {
                return double.NaN;
            }
        }
    }

    public class Scalar<DataType> : IFeatureVectorsData
        where DataType: IConvertible
    {
        private DataType data;
        public Scalar(DataType initValue)
        {
            this.data = initValue;
        }
        public float X { get { return float.NaN; } }
        public float Y { get { return float.NaN; } }
        public float Z { get { return float.NaN; } }
        public double Value
        {
            get
            {
                return data.ToDouble(NumberFormatInfo.CurrentInfo);
            }
        }
    }
    public interface IFeatureVector<DataType>
        where DataType : IFeatureVectorsData
    {
        DataType Data { get; }
        double GetDistance(DistanceMeasure measureType, IFeatureVector<DataType> theOther);
    }

    public class FeatureVector<DataType> : 
        IFeatureVector<DataType>, 
        IComparable<FeatureVector<DataType>>
        where DataType : IFeatureVectorsData
    {
        private DataType data;
        public FeatureVector(DataType initValue)
        {
            this.data = initValue;
        }

        public DataType Data
        {
            get { return data; }
        }
        public int CompareTo(FeatureVector<DataType> other)
        {
            return (int)(this.Data.Value - other.Data.Value);
        }
        public double GetDistance(DistanceMeasure measureType, IFeatureVector<DataType> theOther)
        {
            var xVal = this.Data.Value;
            var yVal = theOther.Data.Value;
            var diff = 0.0d;
            if ( measureType == DistanceMeasure.Manhattan || measureType == DistanceMeasure.Maximum )
            {
                diff = Math.Abs(xVal - yVal);
            }
            else if ( measureType == DistanceMeasure.SquaredEuclidean || measureType == DistanceMeasure.Euclidean )
            {
                diff = (xVal - yVal);
            }
            else if ( measureType == DistanceMeasure.Cosine)
            {
                var x = this.Data;
                var y = theOther.Data;
                var vec1 = new Vector(x.X, x.Y, x.Z);
                var vec2 = new Vector(y.X, y.Y, y.Z);
                var similarity = Vector.DotProduct(vec1, vec2) / ( vec1.Length * vec2.Length );
                diff = 1 - similarity;
            }
            return diff;
        }
        
        public static double operator -(FeatureVector<DataType> v1, FeatureVector<DataType> v2)
        {
            return v1.GetDistance(DistanceMeasure.Manhattan, v2);
        }

        //public FeatureVector(double initValue)
        //{
        //    data.Value = initValue;
        //}
        public static implicit operator double(FeatureVector<DataType> v1)
        {
            return v1.data.Value;
        }
        //public static FeatureVector<Type> operator /(FeatureVector<Type> v1, FeatureVector<Type> v2)
        //{
        //    var diff = (Type)(Object)(v1.value.ToDouble(NumberFormatInfo.CurrentInfo) / v2.value.ToDouble(NumberFormatInfo.CurrentInfo));
        //    return new FeatureVector<Type>(diff);
        //}
    }

    [Serializable]
    public struct Point
    {
        public float X;
        public float Y;
        public float Z;

        public Point(float x, float y, float z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point(Point p)
            : this()
        {
            this.X = p.X;
            this.Y = p.Y;
            this.Z = p.Z;
        }

        public void Adapt(Point point)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Z = point.Z;
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}", this.X, this.Y, this.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Point))
            {
                return false;
            }
            var point = ((Point)obj);
            return point.X == this.X && point.Y == this.Y && point.Z == this.Z;            
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Y.GetHashCode();
        }

        public static Point Zero
        {
            get { return zero; }
        }

        public static bool IsZero(Point point)
        {
            return Zero.Equals(point);
        }

        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }

        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        public static double Distance2D(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static Point Center(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
        }

        public static Point Center(IList<Point> points)
        {
            var center = Point.Zero;
            if (points.Count > 0)
            {
                for (int index = 0; index < points.Count; index++)
                {
                    var p = points[index];
                    center.X += p.X;
                    center.Y += p.Y;
                    center.Z += p.Z;
                }

                center.X /= points.Count;
                center.Y /= points.Count;
                center.Z /= points.Count;
            }
            return center;
        }

        public static Point FindNearestPoint(Point target, IEnumerable<Point> points)
        {
            var pointList = points.ToList();
            return pointList[FindIndexOfNearestPoint(target, pointList)];
        }

        public static int FindIndexOfNearestPoint(Point target, IList<Point> points)
        {
            int index = 0;
            int resultIndex = -1;
            double minDist = double.MaxValue;
            foreach (Point p in points)
            {
                var distance = Distance(p.X, p.Y, target.X, target.Y);
                if (distance < minDist)
                {
                    resultIndex = index;
                    minDist = distance;
                }
                index++;
            }
            return resultIndex;
        }

        public static Vector Subtract(Point point1, Point point2)
        {
            return new Vector(point1.X - point2.X, point1.Y - point2.Y, point1.Z - point2.Z);
        }

        private static Point zero = new Point(0, 0, 0);
    }
    public struct Vector
    {
        public float X;
        public float Y;
        public float Z;

        public Vector(float x, float y, float z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector GetNormalizedVector()
        {
            var length = this.Length;
            return new Vector(this.X /= length, this.Y /= length, this.Z /= length);
        }
        public static float DotProduct(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }
        public float Length
        {
            get { return (float)Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2) + Math.Pow(this.Z, 2)); }
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}", this.X, this.Y, this.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector))
            {
                return false;
            }
            var vector = ((Vector)obj);
            return vector.X == this.X && vector.Y == this.Y && vector.Z == this.Z;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Y.GetHashCode();
        }

        public static Vector Zero
        {
            get { return zero; }
        }

        private static Vector zero = new Vector(0, 0, 0);
    }
}
