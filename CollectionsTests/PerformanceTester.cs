using System.Diagnostics;

namespace CollectionsTests;

public class PerformanceTester {
    private ITestable2DCollection _testedCollection;
    private List<TestPoint> _insertData;
    private List<(TestPoint, TestPoint)> _intervalData;
    
    
    public PerformanceTester(ITestable2DCollection testedCollection, List<TestPoint> insertData, List<(TestPoint, TestPoint)> intervalData) {
        
        this._testedCollection = testedCollection;
        this._insertData = insertData;
        this._intervalData = intervalData;
    }
    
    private double _Measure(Action action) {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();

        return stopwatch.Elapsed.TotalMicroseconds;
    }


    public Dictionary<string, double> Run(List<TestPoint> data, int findCount, int findIntervalCount) {
        Dictionary<string, double> results = new();

        
        results["Insert"] = this._Measure(() =>
        {
            foreach (var testPoint in this._insertData) {
                this._testedCollection.Insert(testPoint);
            }
        }) / this._intervalData.Count;
        
        
        results["Interval"] = this._Measure(() =>
        {
            foreach (var pointPair in this._intervalData) {
                var result = this._testedCollection.EnumerateRange(pointPair.Item1, pointPair.Item2).ToArray();
            }
        }) / this._intervalData.Count;
        
        foreach (var kvp in results) {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        
        return results;
    }
}