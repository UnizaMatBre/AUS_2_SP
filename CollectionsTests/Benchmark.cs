using System.Diagnostics;
using nietras.SeparatedValues;

namespace CollectionsTests;

public class Benchmark {
    private double _minX, _minY, _maxX, _maxY;
    
    private readonly List<TestPoint> _insertData;
    private readonly List<(TestPoint, TestPoint)> _intervalData;


    public double GetMinX() => this._minX;
    public double GetMaxX() => this._maxX;
    
    public double GetMinY() => this._minY;
    public double GetMaxY() => this._maxY;


    public TestPoint GetNorthWestCorner() => new TestPoint(this._minX, this._minY);

    public TestPoint GetSouthEastCorner() => new TestPoint(this._maxX, this._maxY);
    

    public Benchmark() {
        this._insertData = new();
        this._intervalData = new();
        
        this._minX = this._maxX = this._minY = this._maxY = 0;
    }

    /// load data from csv files
    public void LoadTestData() {
        this._insertData.Clear();
        this._intervalData.Clear();
        
        var pointReader = Sep.Reader().FromFile("../../../data/slovakia_nodes.csv");
        
        foreach (var row in pointReader) {
            this._insertData.Add(new(
                row["lat"].Parse<double>(),
                row["lon"].Parse<double>()
            ));
        }
        
        
        var intervalReader = Sep.Reader().FromFile("../../../data/testovacie_oblasti.csv");
        foreach (var row in intervalReader) {
            var min_lat = row["min_lat"].Parse<double>();
            var min_lon = row["min_lon"].Parse<double>();
            var max_lat = row["max_lat"].Parse<double>();
            var max_lon = row["max_lon"].Parse<double>();
                
            this._intervalData.Add((
                new(min_lat, min_lon), new (max_lat, max_lon)
            ));
        }
        
        this._minX = this._insertData.Min(p => p.GetX());
        this._maxX = this._insertData.Max(p => p.GetX());
        this._minY = this._insertData.Min(p => p.GetY());
        this._maxY = this._insertData.Max(p => p.GetY());
    }
    
    public double TestInserts(ITestable2DCollection testedCollection) {
        var stopwatch = Stopwatch.StartNew();
        foreach (var testPoint in this._insertData) {
            testedCollection.Insert(testPoint);
        }
        stopwatch.Stop();

        return stopwatch.Elapsed.TotalMicroseconds / this._insertData.Count;
    }

    public double TestIntervals(ITestable2DCollection testedCollection) {
        var stopwatch = Stopwatch.StartNew();
        foreach (var pointPair in this._intervalData) {
            var result = testedCollection.EnumerateRange(pointPair.Item1, pointPair.Item2);
            
            // this is done to force materialization of result
            foreach (var testPoint in result) {
                var stored = testPoint.GetX();
            }
        }
        stopwatch.Stop();
        
        return stopwatch.Elapsed.TotalMicroseconds / this._intervalData.Count; 
    }
}