using Collections;

namespace CollectionsTests;

class KdTreeTestingWrapper : ITestable2DCollection {
    private KdTree<TestPoint> _tree = new(new TestPoint.DimensionalComparer());


    public bool Insert(TestPoint point) => this._tree.Insert(point);

    public bool TryGetValue(TestPoint point, out TestPoint? output) => this._tree.TryGetValue(point, out output);

    public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper) => this._tree.EnumerateRange(lower, upper);

    public bool CheckValidity() => this._tree.Debug_IsKdTree();
}




class Program
{
    static void RunFunctionalityTests() {
        var kdFunctionalityTester = new FunctionalityTester(
            new KdTreeTestingWrapper(), 100_000
        );
        kdFunctionalityTester.Run(0.4, 0.2, 0.3, 0.1);


    }


    public const bool IS_INTEGRITY_TESTING = true;
    

    static void Main(string[] args) {
        if (IS_INTEGRITY_TESTING) {
            RunFunctionalityTests();
            
            
            return;
        }
        
    }
}