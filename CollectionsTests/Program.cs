using System.Drawing;
using Collections;

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
        
        //(new FunctionalityTester(new KdTreeTestingWrapper(), new Random())).Run(10_000, 0.5, 0.2, 0.3);

        var quadTreeWrap = new QuadTreeTestingWrapper(
            new(-1, -1), new(width + 1, width + 1)
        );
        
        (new FunctionalityTester(quadTreeWrap, new Random(1234))).Run(iterationCount, 0.5, 0.2, 0.3);
        
        

    }


    static void RunKdTreeExample() {
    }


    public const TestType SELECTED_TESTS = TestType.FUNCTITONALITY_TEST;
    

    static void Main(string[] args) {
        switch (SELECTED_TESTS) {

             case TestType.FUNCTITONALITY_TEST: {
                RunFunctionalityTests();
                break;
            }

            case TestType.PERFORMANCE_TEST: {

                throw new NotImplementedException();
            }
        }
        
    }
}