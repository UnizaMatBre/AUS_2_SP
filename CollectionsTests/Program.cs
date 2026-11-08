using System.Drawing;
using System.Runtime.CompilerServices;
using Collections;
using nietras.SeparatedValues;

namespace CollectionsTests;

public enum TestType {
    FUNCTITONALITY_TEST,
    PERFORMANCE_TEST_LOCAL_ONLY,
    PERFORMANCE_TEST_FOREIGN_ONLY,
    PERFORMANCE_TEST_ALL
}


class Program
{
    static void RunFunctionalityTests() {
        int iterationCount = 10_000;
        int width = (int)Math.Sqrt(iterationCount);
        
        
        (new FunctionalityTester("K-d Tree",
            new Breznican.KdTreeTestingWrapper(), 
            new Random())
        ).Run(iterationCount, 0.5, 0.2, 0.3);
        
        (new FunctionalityTester("Quad Tree With List",
            new Breznican.QuadTreeWithListTestingWrapper(new TestPoint(-1, -1), new TestPoint(width + 2, width + 2)), 
            new Random())
        ).Run(iterationCount, 0.5, 0.2, 0.3);
        
        (new FunctionalityTester("Quad Tree with Kd Tree",
            new Breznican.QuadTreeWithKdTestingWrapper(new TestPoint(-1, -1), new TestPoint(width + 2, width + 2)), 
            new Random())
        ).Run(iterationCount, 0.5, 0.2, 0.3);

    }


    private static void _RunPerformanceTestsLocal(ref Benchmark benchmark) {
        
        {
            Console.WriteLine("=== breznican1.KdTree ===");
            var kdTreeWrapper = new Breznican.KdTreeTestingWrapper();
            Console.WriteLine("+ Inserts: {0}", benchmark.TestInserts(kdTreeWrapper));
            Console.WriteLine("+ Intervals: {0}", benchmark.TestIntervals(kdTreeWrapper));
        }

        {
            Console.WriteLine("=== breznican1.QuadTreeWithList ===");
            var quadTreeWrapper = new Breznican.QuadTreeWithListTestingWrapper(  
                benchmark.GetNorthWestCorner(),
                benchmark.GetSouthEastCorner()
            );

            Console.WriteLine("+ Inserts: {0}", benchmark.TestInserts(quadTreeWrapper)); 
            Console.WriteLine("+ Intervals: {0}", benchmark.TestIntervals(quadTreeWrapper));
        }
        
        {
            Console.WriteLine("=== breznican1.QuadTreeWithKd ===");
            var quadTreeWrapper = new Breznican.QuadTreeWithKdTestingWrapper(  
                benchmark.GetNorthWestCorner(),
                benchmark.GetSouthEastCorner()
            );

            Console.WriteLine("+ Inserts: {0}", benchmark.TestInserts(quadTreeWrapper)); 
            Console.WriteLine("+ Intervals: {0}", benchmark.TestIntervals(quadTreeWrapper));
        }
        
    }
    
    private static void _RunPerformanceTestsForeign(ref Benchmark benchmark) {
        
        {
            Console.WriteLine("=== omnicollection.spatial.quadtree ===");
            var omnicollecionQuadTree = new OmnoCollectionQuadTreeWrapper(
                benchmark.GetNorthWestCorner(),
                benchmark.GetSouthEastCorner()
            );
            Console.WriteLine("+ Inserts: {0}", benchmark.TestInserts(omnicollecionQuadTree)); 
            Console.WriteLine("+ Intervals: {0}", benchmark.TestIntervals(omnicollecionQuadTree));
        }
        
        {
            Console.WriteLine("=== hrogen.SpatialTrees.QuadTree ===");
            var hrogenQuadTree = new HrogerQuadTreeWrapper(
                benchmark.GetNorthWestCorner(),
                benchmark.GetSouthEastCorner()
            );
            Console.WriteLine("+ Inserts: {0}", benchmark.TestInserts(hrogenQuadTree)); 
            Console.WriteLine("+ Intervals: {0}", benchmark.TestIntervals(hrogenQuadTree));
        }

        /*

       // omnicollection.spatial.quadtree
       // Auios.QuadTree
       {
           var auiosQuadTree = new AuiosQuadTreeWrapper(
               new TestPoint(minX, minY),
               new TestPoint(maxX, maxY)
           );
           (new PerformanceTester("Auios.QuadTree", auiosQuadTree, insertData, intervalData)).Run();

       }

       // SpatialTrees.QuadTree
       {
           var hrogenQuadTree = new HrogerQuadTreeWrapper(
               new TestPoint(minX, minY),
               new TestPoint(maxX, maxY)
           );
           (new PerformanceTester("Hrogen.SpatialTrees.QuadTree", hrogenQuadTree, insertData, intervalData)).Run();

       }
       */
    }


    static void RunPerformanceTests(bool includeLocalTypes, bool includeForeignTypes) {
        
        var benchmark = new Benchmark();
        benchmark.LoadTestData();

        if (includeLocalTypes) {
            _RunPerformanceTestsLocal(ref benchmark);
        }

        if (includeForeignTypes) {
            _RunPerformanceTestsForeign(ref benchmark);
        }
        
       

    }

    

    static void Main(string[] args) {
        if (args.Length == 0) {
            throw new ArgumentException("Missing test type argument.");
        }
        
        var testTypeStr = args[0];
        TestType selectedTestType = testTypeStr switch {
            "func" => TestType.FUNCTITONALITY_TEST,
            "perf_local" => TestType.PERFORMANCE_TEST_LOCAL_ONLY,
            "perf_foreign" => TestType.PERFORMANCE_TEST_FOREIGN_ONLY,
            "perf_all" => TestType.PERFORMANCE_TEST_ALL,
            _ => throw new ArgumentException($"Invalid test type '{testTypeStr}'.")
        };
        

        switch (selectedTestType) {
            case TestType.FUNCTITONALITY_TEST: {
                
                RunFunctionalityTests();
                break;
            }

            case TestType.PERFORMANCE_TEST_LOCAL_ONLY: {

                RunPerformanceTests(includeLocalTypes: true ,includeForeignTypes: false);
                break;
            }

            case TestType.PERFORMANCE_TEST_FOREIGN_ONLY: {
                
                RunPerformanceTests(includeLocalTypes: false ,includeForeignTypes: true);
                break;
            }
            
            case TestType.PERFORMANCE_TEST_ALL: {
                
                RunPerformanceTests(includeLocalTypes: true, includeForeignTypes: true);
                break;
            }
        }
        
    }
}