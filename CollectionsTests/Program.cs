using System.Drawing;
using Collections;

namespace CollectionsTests;

class KdTreeTestingWrapper : ITestable2DCollection {
    private KdTree<TestPoint> _tree = new(new TestPoint.DimensionalComparer());


    public bool Insert(TestPoint point) => this._tree.Insert(point);
    
    public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper) => this._tree.EnumerateRange(lower, upper);

    public bool CheckValidity() => this._tree.Debug_IsKdTree();
}

class QuadTreeTestingWrapper : ITestable2DCollection {
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

public enum TestType {
    EXAMPLE_TEST,
    FUNCTITONALITY_TEST,
    PERFORMANCE_TEST
}


class Program
{
    static void RunFunctionalityTests() {
        (new FunctionalityTester(new KdTreeTestingWrapper(), new Random())).Run(10_000, 0.5, 0.2, 0.3);

        
        

    }


    static void RunKdTreeExample() {
    }


    public const TestType SELECTED_TESTS = TestType.FUNCTITONALITY_TEST;
    

    static void Main(string[] args)
    {
        switch (SELECTED_TESTS)
        {
            case TestType.EXAMPLE_TEST: {
                var testobj = new KdExampleTest();
                break;

            }

            case TestType.FUNCTITONALITY_TEST: {
                RunFunctionalityTests();
                break;
            }

            case TestType.PERFORMANCE_TEST: {

                throw new NotImplementedException();
            }
        }
        
    }
}