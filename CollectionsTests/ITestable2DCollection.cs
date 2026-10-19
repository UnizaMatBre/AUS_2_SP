namespace CollectionsTests;

public interface ITestable2DCollection {
    public bool Insert(TestPoint point);
    public bool TryGetValue(TestPoint point, out TestPoint? output);
    
    public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper);

    public bool CheckValidity();

    // public bool Remove(TestPoint point);
}