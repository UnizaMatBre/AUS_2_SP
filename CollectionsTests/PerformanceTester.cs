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


    public Dictionary<string, double> Run(List<TestPoint> data, int findCount, int findIntervalCount) {
        Dictionary<string, double> results = new();

        
        results["Insert"] = this._Measure(() => {
            for (var index = 0; index < data.Count; ++index) {
                
                if (index % 25_000 == 0) {
                    Console.WriteLine($"Add {index}");
                }
                
                this._testedCollection.Insert(data[index]);    
            }
        }) / data.Count;
        
        results["Find"] = this._Measure(() => {
            for (int index = 0; index < findCount; ++index) {

                if (index % 250_000 == 0) {
                    Console.WriteLine($"Find {index}");
                }
                this._testedCollection.TryGetValue(data[index], out TestPoint result);
            }
        }) / findCount;
        
        foreach (var kvp in results)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        
        return results;
    }
}