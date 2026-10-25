using Collections;

namespace CollectionsTests;

struct DataSource {
    private int _sourceWidth;
    private TestPoint[] _sourceDataset;
    private uint _sourceCurrentIndex;

    public DataSource(int dataCount, int seed) {
        var rng = new Random(seed);
        var sourceWidth = (int)Math.Sqrt(dataCount);

        this._sourceCurrentIndex = 0;
        this._sourceWidth = sourceWidth;
        this._sourceDataset = Enumerable
            .Range(0, dataCount)
            .Select((index) => {
                return new TestPoint(
                    (index % sourceWidth), (index / sourceWidth) 
                );
            })
            .ToArray();
        
        
        // Fisher–Yates shuffle
        for (int counter = this._sourceDataset.Length - 1; counter > 0; --counter) {
                
            int j = rng.Next(counter + 1);
                
            (this._sourceDataset[counter], this._sourceDataset[j]) = (this._sourceDataset[j], this._sourceDataset[counter]);
        }
    }


    public TestPoint NextPoint() {
        return this._sourceDataset[this._sourceCurrentIndex++];
    }

    public int GetDataWidth() => this._sourceWidth;
}


public class FunctionalityTester {
    private readonly Random _masterRandom;
    private readonly List<TestPoint> _insertedPoints;
    
    private int _iterationCount;


    private Random? _valueRandom;

    private ITestable2DCollection _testedCollection;

    private DataSource _dataSource;

    
    private Dictionary<string, int> _counters;
    
    private void _CheckInsert() {
        this._counters["Insert"]++;

        // generate new unique point
        var newTestPoint = this._dataSource.NextPoint();
        
        this._insertedPoints.Add(newTestPoint);
        if (this._testedCollection.Insert(newTestPoint) == false) {
            
            throw new InvalidDataException($"Tree Insert Failed => Data {newTestPoint} is already in the tree");
        }
    }

    private void _CheckRemove() {
        if (this._insertedPoints.Count < 1) {
            this._CheckInsert();
            return;

        }
        
        //this._counters["Remove"]++;
    }
    
    private void _CheckFindInterval() {
        if(this._insertedPoints.Count < 2) {
            this._CheckInsert();
            return;
        }
        this._counters["Interval"]++;
        
        int xMax = (this._insertedPoints.Count < this._dataSource.GetDataWidth()) ? this._insertedPoints.Count : this._dataSource.GetDataWidth();
        int yMax = (this._insertedPoints.Count / this._dataSource.GetDataWidth());

        (int, int) GenerateRange(int maximum) {
            int halfPoint = maximum / 2;
            
            int a = this._valueRandom.Next(halfPoint);
            int b = a + this._valueRandom.Next(halfPoint);

            return (a, b);
        }


        var xRange = GenerateRange(xMax);
        var yRange = GenerateRange(yMax);

        var lowerBoundary = new TestPoint(xRange.Item1, yRange.Item1);
        var upperBoundary = new TestPoint(xRange.Item2, yRange.Item2);


        IDimensionalComparer<TestPoint> comparer = new TestPoint.DimensionalComparer();

        var expectedValues = this._insertedPoints.Where((item) => {
            return comparer.InclusiveBetween(item, lowerBoundary, upperBoundary);
        }).ToList();
        
        var foundValues = this._testedCollection.EnumerateRange(lowerBoundary, upperBoundary).ToList();
        
        foreach (var oneFoundValue in foundValues) {
            if (expectedValues.Contains(oneFoundValue) == false) {
                throw new InvalidDataException($"Tree Find Interval failed => Data {oneFoundValue} is not in result interval {lowerBoundary}-{upperBoundary}");
            }
        }
        
    }



    public void Run(int iterationCount, double insertsChance, double removesChance, double intervalsChance) {
        this._iterationCount = iterationCount;
        
        // initialize random generators

        this._valueRandom = new Random(this._masterRandom.Next());
        
        // initialize counters
        this._counters = new() {
            ["Insert"] = 0,
            ["Remove"] = 0,
            ["Interval"] = 0
        };
        
        // generate data 
        {
            this._dataSource = new(iterationCount, this._masterRandom.Next());   
            this._insertedPoints.Clear();
            Console.WriteLine("Source Data generated and initialized.");
        }
        
        


        // initialize opereration picker
        var insertsChanceCml = insertsChance;
        var removesChanceCml = insertsChanceCml + removesChance;
        var intervalsChanceCml = removesChanceCml + intervalsChance;
        
        var checks = new (double cumulativeChance, Action checkAction)[] {
            (insertsChanceCml, this._CheckInsert),
            (removesChanceCml, this._CheckRemove),
            (intervalsChanceCml, this._CheckFindInterval)
        };

        
        var actionRandom = new Random(this._masterRandom.Next());

        for (var counter = 0; counter < this._iterationCount; ++counter) {
            var chance = actionRandom.NextDouble();

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

    public FunctionalityTester(ITestable2DCollection collection, Random masterRandom) {
        this._masterRandom = masterRandom;
        this._testedCollection = collection;
        this._insertedPoints = new();
        this._counters = new Dictionary<string, int>();
    }
    
}