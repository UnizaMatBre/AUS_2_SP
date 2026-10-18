using Collections;

namespace CollectionsTests;

public class TestPoint {


    private readonly int _x;
    private readonly int _y;

    public TestPoint() {
        this._x = 0;
        this._y = 0;
    }

    public TestPoint(int x, int y) {
        this._x = x;
        this._y = y;
    }

    public int GetX() => this._x;
    public int GetY() => this._y;


    public int DimensionalCompareTo(uint dimension, TestPoint other) {
        return dimension switch {
            0 => this._x.CompareTo(other._x),
            1 => this._y.CompareTo(other._y),
            _ => throw new ArgumentOutOfRangeException(nameof(dimension))
        };
    }


    public class DimensionalComparer : IDimensionalComparer<TestPoint> {
        public uint GetDimensionCount() => 2;

        public int DimensionalCompare(uint dimension, TestPoint a, TestPoint b) {
            return dimension switch {
                0 => a._x.CompareTo(b._x),
                1 => a._y.CompareTo(b._y),
                _ => throw new ArgumentOutOfRangeException(nameof(dimension))
            };
        }
    }

    public override String ToString() {

        return $"({this._x} : {this._y})";
    }
}


public interface ITestable2DCollection {
    public bool Insert(TestPoint point);
    public bool TryGetValue(TestPoint point, out TestPoint? output);
    
    public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper);

    public bool CheckValidity();

    // public bool Remove(TestPoint point);
}




public class FunctionalityTester {
    
    public const int GRID_WIDTH = 20;
    
    
    private readonly int _iterationCount;
 
    private Random? _chanceRandom;
    private Random? _valueRandom;

    private ITestable2DCollection _testedCollection;
    
    private Dictionary<string, int> _counters;
    private readonly List<TestPoint> _dataStorage;
    private int _dataCountMax;
    
    private void _CheckInsert() {
        this._counters["Insert"]++;

        // generate new unique point
        TestPoint newTestPoint = new(
            this._dataCountMax % GRID_WIDTH,
            this._dataCountMax / GRID_WIDTH
        );
        ++this._dataCountMax;
        
        this._dataStorage.Add(newTestPoint);

        if (this._testedCollection.Insert(newTestPoint) == false) {
            
            throw new InvalidDataException($"Tree Insert Failed => Data {newTestPoint} is already in the tree");
        }
    }

    private void _CheckRemove() {
        if (this._dataStorage.Count < 1) {
            this._CheckInsert();
            return;

        }
        
        this._counters["Remove"]++;
    }

    private void _CheckFind() {
        if (this._dataStorage.Count < 1) {
            this._CheckInsert();
            return;
        }
        
        this._counters["Find"]++;
        
        int index = this._valueRandom.Next(this._dataStorage.Count);
        var dataForSearch = this._dataStorage[index];
        
        bool success = this._testedCollection.TryGetValue(dataForSearch, out TestPoint? dataActuallyFound);

        if (!success) {
            
            throw new InvalidDataException($"Tree Find Failed => Data {dataForSearch} not found");
        }
        
        if (dataForSearch != dataActuallyFound) {
            throw new InvalidDataException($"Tree Find Failed => Found data {dataActuallyFound} is not same as expected data {dataForSearch}");
        }
    }

    private void _CheckFindInterval() {
        if(this._dataStorage.Count < 2) {
            this._CheckInsert();
            return;
        }
        
        this._counters["Interval"]++;
    }



    public void Run(double insertsChance, double removesChance, double findsChance, double intervalsChance) {
        // initialize random generators
        // TODO: Should i refactor this into one main generator?
        this._chanceRandom = new();
        this._valueRandom = new();
        
        // initialize counters
        this._counters = new() {
            ["Insert"] = 0,
            ["Remove"] = 0,
            ["Find"] = 0,
            ["Interval"] = 0
        };
        
        this._dataStorage.Clear();
        this._dataCountMax = 0;
        
        // initialize opereration picker
        var insertsChanceCml = insertsChance;
        var removesChanceCml = insertsChanceCml + removesChance;
        var findsChanceCml = removesChanceCml + findsChance;
        var intervalsChanceCml = findsChanceCml + intervalsChance;
        
        var checks = new (double cumulativeChance, Action checkAction)[] {
            (insertsChanceCml, this._CheckInsert),
            (removesChanceCml, this._CheckRemove),
            (findsChanceCml, this._CheckFind),
            (intervalsChanceCml, this._CheckFindInterval)
        };


        for (var counter = 0; counter < this._iterationCount; ++counter) {
            var chance = this._chanceRandom.NextDouble();

            // each 10 iterations check tree validity
            if (this._testedCollection.CheckValidity() == false) {
                
                throw new InvalidDataException("Tree is invalid.");
            }
            
            if (counter % (this._iterationCount / 100) == 0) {
                Console.WriteLine($"Iteration: {counter}");
            }

            foreach (var (limit, action) in checks) {
                if (chance < limit) {
                    action();
                    break;
                }
                
            }

        }
        Console.WriteLine($"Iteration: {this._iterationCount}");

        
        Console.WriteLine();
        Console.WriteLine("== TEST DONE ==");
        Console.WriteLine("===============");
        foreach (var keyValue in this._counters) {
            Console.WriteLine($"{keyValue.Key}: {keyValue.Value}");
        }
    }

    public FunctionalityTester(ITestable2DCollection collection  ,int iterationCount) {
        this._testedCollection = collection;
        this._iterationCount = iterationCount;
        this._dataStorage = new();
        this._counters = null;
    }
    
}