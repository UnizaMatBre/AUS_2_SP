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
        int iterationCount = 100_000;
        int width = (int)Math.Sqrt(iterationCount);
        
        (new FunctionalityTester(new KdTreeTestingWrapper(), new Random())).Run(iterationCount, 0.5, 0.2, 0.3);

        /*
        var quadTreeWrap = new QuadTreeTestingWrapper(
            new(-1, -1), new(width + 1, width + 1)
        );
        
        (new FunctionalityTester(quadTreeWrap, new Random(1234))).Run(iterationCount, 0.5, 0.2, 0.3);
        */
        

    }

    static void RunPerformanceTests()
    {
        double minX, minY, maxX, maxY;
        
        List<TestPoint> insertData = new();
        List<(TestPoint, TestPoint)> intervalData = new();

        {
            var pointReader = Sep.Reader().FromFile("../../../data/slovakia_nodes.csv");

            foreach (var row in pointReader) {
                insertData.Add(new(
                    row["lat"].Parse<double>(),
                    row["lon"].Parse<double>()
                ));
            }

            minX = insertData.Min(p => p.GetX());
            maxX = insertData.Max(p => p.GetX());
            minY = insertData.Min(p => p.GetY());
            maxY = insertData.Max(p => p.GetY());
        }

        {
            var intervalReader = Sep.Reader().FromFile("../../../data/testovacie_oblasti.csv");
            foreach (var row in intervalReader)
            {
                var min_lat = row["min_lat"].Parse<double>();
                var min_lon = row["min_lon"].Parse<double>();
                var max_lat = row["max_lat"].Parse<double>();
                var max_lon = row["max_lon"].Parse<double>();
                
                intervalData.Add((
                    new(min_lat, min_lon), new (max_lat, max_lon)
                ));
            }
        }

        // breznican1.QuadTree
        /*
        {
            var quadTreeWrapper = new QuadTreeTestingWrapper(
                new TestPoint(minX, minY),
                new TestPoint(maxX, maxY)
            );
            (new PerformanceTester("beznican1.QuadTree", quadTreeWrapper, insertData, intervalData)).Run();
        }
        */
        // omnicollection.spatial.quadtree
        /*
        {
            var omnicollecionQuadTree = new OmnoCollectionQuadTreeWrapper(
                new TestPoint(minX, minY),
                new TestPoint(maxX, maxY)
            );
            (new PerformanceTester("omnicollection.QuadTree", omnicollecionQuadTree, insertData, intervalData)).Run();
            
        }
        */

        // Auios.QuadTree
        /*
        {
            var auiosQuadTree = new AuiosQuadTreeWrapper(
                new TestPoint(minX, minY),
                new TestPoint(maxX, maxY)
            );
            (new PerformanceTester("Auios.QuadTree", auiosQuadTree, insertData, intervalData)).Run();

        }
        */
        // SpatialTrees.QuadTree
        {
            var hrogenQuadTree = new HrogerQuadTreeWrapper(
                new TestPoint(minX, minY),
                new TestPoint(maxX, maxY)
            );
            (new PerformanceTester("Hrogen.SpatialTrees.QuadTree", hrogenQuadTree, insertData, intervalData)).Run();
            
        }


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