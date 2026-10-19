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

    public override String ToString() {

        return $"({this._x} : {this._y})";
    }
}