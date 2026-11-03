using System.Drawing;
using System.Runtime.CompilerServices;
using Collections;
using nietras.SeparatedValues;

namespace CollectionsTests;

public enum TestType {
    FUNCTITONALITY_TEST,
    PERFORMANCE_TEST
}


class Program
{
    static void RunFunctionalityTests() {
        int iterationCount = 10_000;
        int width = (int)Math.Sqrt(iterationCount);
        
        (new FunctionalityTester(new KdTreeTestingWrapper(), new Random())).Run(iterationCount, 0.5, 0.2, 0.3);
        (new FunctionalityTester(new Breznican.QuadTreeWithListTestingWrapper(new TestPoint(-1, -1), new TestPoint(width + 2, width + 2)), new Random())).Run(iterationCount, 0.5, 0.2, 0.3);
        (new FunctionalityTester(new Breznican.QuadTreeWithKdTestingWrapper(new TestPoint(-1, -1), new TestPoint(width + 2, width + 2)), new Random())).Run(iterationCount, 0.5, 0.2, 0.3);

        /*
        var quadTreeWrap = new QuadTreeTestingWrapper(
            new(-1, -1), new(width + 1, width + 1)
        );
        
        (new FunctionalityTester(quadTreeWrap, new Random(1234))).Run(iterationCount, 0.5, 0.2, 0.3);
        */
        

    }

    static void RunPerformanceTests()
    {
        var benchmark = new Benchmark();
        benchmark.LoadTestData();

        {
            Console.WriteLine("=== breznican1.KdTree ===");
            var kdTreeWrapper = new KdTreeTestingWrapper();
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
        
        // breznican1.KdTree
        /*
        {
            var kdTreeWrapper = new KdTreeTestingWrapper();
            (new PerformanceTester("breznican1.KdTree", kdTreeWrapper, insertData, intervalData)).Run();
        }
        */
        
        /*
        
        // omnicollection.spatial.quadtree
        {
            var omnicollecionQuadTree = new OmnoCollectionQuadTreeWrapper(
                new TestPoint(minX, minY),
                new TestPoint(maxX, maxY)
            );
            (new PerformanceTester("omnicollection.QuadTree", omnicollecionQuadTree, insertData, intervalData)).Run();
            
        }
        

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


    public const TestType SELECTED_TESTS = TestType.PERFORMANCE_TEST;
    

    static void Main(string[] args) {
        switch (SELECTED_TESTS) {

             case TestType.FUNCTITONALITY_TEST: {
                RunFunctionalityTests();
                break;
            }

            case TestType.PERFORMANCE_TEST: {

                RunPerformanceTests();
                break;
            }
        }
        
    }
}