using Collections;

namespace CollectionsTests;

class KdTreeTestingWrapper : ITestable2DCollection {
    private KdTree<TestPoint> _tree = new(new TestPoint.DimensionalComparer());


    public bool Insert(TestPoint point) => this._tree.Insert(point);

    public bool TryGetValue(TestPoint point, out TestPoint? output) => this._tree.TryGetValue(point, out output);

    public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper) => this._tree.EnumerateRange(lower, upper);

    public bool CheckValidity() => this._tree.Debug_IsKdTree();
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
        kdFunctionalityTester.Run(0.4, 0.2, 0.3, 0.1);


    }


    static void RunKdTreeExample() {
    }


    public const TestType SELECTED_TESTS = TestType.EXAMPLE_TEST;
    

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