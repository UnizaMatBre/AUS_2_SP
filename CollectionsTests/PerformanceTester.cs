using System.Diagnostics;

namespace CollectionsTests;

public class PerformanceTester {
    private ITestable2DCollection _testedCollection;

    public PerformanceTester(ITestable2DCollection testedCollection) {
        
        this._testedCollection = testedCollection;
    }
    
    private double _Measure(Action action) {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();

        return stopwatch.Elapsed.TotalMicroseconds;
    }
}