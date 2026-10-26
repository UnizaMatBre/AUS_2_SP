using Collections;

namespace CollectionsTests;


public class KdTreeTestingWrapper : ITestable2DCollection {
    private KdTree<TestPoint> _tree = new(new TestPoint.DimensionalComparer());


    public bool Insert(TestPoint point) => this._tree.Insert(point);
    
    public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper) => this._tree.EnumerateRange(lower, upper);

    public bool CheckValidity() => this._tree.Debug_IsKdTree();
}

public class QuadTreeTestingWrapper : ITestable2DCollection {
    private readonly QuadTree<TestPoint> _tree;

    private static RectangularBoundary<TestPoint, TestPoint> FromCorners(TestPoint northWestCorner, TestPoint southEastCorner) {
        var north = new TestPoint.YBoundary(northWestCorner.GetY());
        var south = new TestPoint.YBoundary(southEastCorner.GetY());
        
        var west = new TestPoint.XBoundary(northWestCorner.GetX());
        var east = new TestPoint.XBoundary(southEastCorner.GetX());

        return new RectangularBoundary<TestPoint, TestPoint>(
            north, south, east, west
        );
    }
    
    public QuadTreeTestingWrapper(TestPoint northWestCorner, TestPoint southEastCorner) {
        var rect = FromCorners(northWestCorner, southEastCorner);
        
        this._tree = new QuadTree<TestPoint>(rect);
    }
    
    
    public bool Insert(TestPoint point) => this._tree.Insert(point);

    public IEnumerable<TestPoint> EnumerateRange(TestPoint northWestCorner, TestPoint southEastCorner) {
        var rect = FromCorners(northWestCorner, southEastCorner);


        return this._tree.EnumerateBoundary(rect);
    }

    public bool CheckValidity() => this._tree.Debug_IsQuadTree();
}
