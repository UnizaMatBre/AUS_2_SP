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

    public QuadTreeTestingWrapper(TestPoint northWestCorner, TestPoint southEastCorner) {
        var north = new TestPoint.YBoundary(northWestCorner.GetY());
        var south = new TestPoint.YBoundary(southEastCorner.GetY());
        
        var west = new TestPoint.XBoundary(northWestCorner.GetX());
        var east = new TestPoint.XBoundary(southEastCorner.GetX());

        var rect = new RectangularBoundary<TestPoint, TestPoint>(
            north, south, east, west
        );
        
        this._tree = new QuadTree<TestPoint>(rect);
    }
    
    
    public bool Insert(TestPoint point) => this._tree.Insert(point);
    
    public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper) => this._tree.EnumerateRange(lower, upper);

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
        var kdFunctionalityTester = new FunctionalityTester(
            new KdTreeTestingWrapper(), 100_000
        );
        kdFunctionalityTester.Run(0.5, 0.2, 0.3);


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