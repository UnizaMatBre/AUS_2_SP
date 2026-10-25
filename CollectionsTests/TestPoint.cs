using Collections;

namespace CollectionsTests;

public class TestPoint {


    private readonly int _x;
    private readonly int _y;

    public TestPoint() {
        this._x = 0;
        this._y = 0;
    }

    public TestPoint(int x, int y) {
        this._x = x;
        this._y = y;
    }

    public int GetX() => this._x;
    public int GetY() => this._y;


    public int DimensionalCompareTo(uint dimension, TestPoint other) {
        return dimension switch {
            0 => this._x.CompareTo(other._x),
            1 => this._y.CompareTo(other._y),
            _ => throw new ArgumentOutOfRangeException(nameof(dimension))
        };
    }


    public class DimensionalComparer : IDimensionalComparer<TestPoint> {
        public uint GetDimensionCount() => 2;

        public int DimensionalCompare(uint dimension, TestPoint a, TestPoint b) {
            return dimension switch {
                0 => a._x.CompareTo(b._x),
                1 => a._y.CompareTo(b._y),
                _ => throw new ArgumentOutOfRangeException(nameof(dimension))
            };
        }
    }
    
    public class XBoundary : IBoundary<TestPoint> {
        private readonly int _delimiter;
        
        public XBoundary(int delimiter) {
            this._delimiter = delimiter;
        }

        public int RelativePosition(TestPoint value) {

            return this._delimiter.CompareTo(value._x);
        }

        public IBoundary<TestPoint> Midpoint(IBoundary<TestPoint> other) {
            if (other is not XBoundary xBoundaryOther) {
                throw new ArgumentException(nameof(other) + " has incompatible type");
            }
            
            var realMidDelimiter = (this._delimiter + xBoundaryOther._delimiter) / 2.0;
            
            return new XBoundary((int)realMidDelimiter);
        }

        public int CompareTo(IBoundary<TestPoint> other) {
            if (other is not XBoundary xBoundaryOther) {
                throw new ArgumentException(nameof(other) + " has incompatible type");
            }
            
            return this._delimiter.CompareTo(xBoundaryOther._delimiter);
        }
    }
    
    
    public class YBoundary : IBoundary<TestPoint> {
        private readonly int _delimiter;
        
        public YBoundary(int delimiter) {
            this._delimiter = delimiter;
        }

        public int RelativePosition(TestPoint value) {

            return this._delimiter.CompareTo(value._x);
        }

        public IBoundary<TestPoint> Midpoint(IBoundary<TestPoint> other) {
            if (other is not YBoundary yBoundaryOther) {
                throw new ArgumentException(nameof(other) + " has incompatible type");
            }
            
            var realMidDelimiter = (this._delimiter + yBoundaryOther._delimiter) / 2.0;
            
            return new YBoundary((int)realMidDelimiter);
        }

        public int CompareTo(IBoundary<TestPoint> other) {
            if (other is not YBoundary yBoundaryOther) {
                throw new ArgumentException(nameof(other) + " has incompatible type");
            }

            return this._delimiter.CompareTo(yBoundaryOther._delimiter);
        }
    }
    
    public override String ToString() {

        return $"({this._x} : {this._y})";
    }
}