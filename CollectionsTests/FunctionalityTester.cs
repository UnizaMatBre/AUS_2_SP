using Collections;

namespace CollectionsTests;

public class TestPoint : IDimensionalComparable<TestPoint> {
    public static uint GetDimensionCount() {
        return 2;
    }

    private readonly int _x;
    private readonly int _y;
    
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
}


public interface ITestable2DCollection {
    public bool Insert(TestPoint point);
    public bool TryGetValue(TestPoint point, out TestPoint? output);
    
    public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper);

    public bool CheckValidity();

    // public bool Remove(TestPoint point);
}




public class FunctionalityTester {
    
    private readonly int _iterationCount;
 
    private Random? _chanceRandom;
    private Random? _valueRandom;
    
    private Dictionary<string, int> _counters;
    private readonly List<int> _dataStorage;
    
    private void _CheckInsert() {
        this._counters["Insert"]++;
        
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

            foreach (var (limit, action) in checks) {
                if (chance < limit) {
                    action();
                    break;
                }
                
            }

        }
    }

    public FunctionalityTester(int iterationCount) {
        
        this._iterationCount = iterationCount;
        this._dataStorage = new();
        this._counters = null;
    }
    
}