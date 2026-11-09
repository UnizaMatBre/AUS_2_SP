namespace CollectionsTests;

public interface ITestable2DCollection {
    public void Build(IEnumerable<TestPoint> points);
    
    public bool Insert(TestPoint point);
    
    public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper);

    public bool CheckValidity();

    // public bool Remove(TestPoint point);
}