using Collections;

namespace CollectionsTests.Breznican {
    public class KdTreeTestingWrapper : ITestable2DCollection {
        private KdTree<TestPoint> _tree = new(
            new TestPoint.DimensionalComparer(),
            new TestPoint.EqualityComparer()
        );


        public bool Insert(TestPoint point) => this._tree.Insert(point);
    
        public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper) => this._tree.EnumerateRange(lower, upper);

        public bool CheckValidity() => this._tree.Debug_IsKdTree();
    }

    
    public class QuadTreeWithListTestingWrapper : ITestable2DCollection {
        
        
        private readonly QuadTreeWithList<TestPoint> _treeWithList;

        private static RectangularBoundary<TestPoint, TestPoint> FromCorners(TestPoint northWestCorner, TestPoint southEastCorner) {
            var north = new TestPoint.YBoundary(northWestCorner.GetY());
            var south = new TestPoint.YBoundary(southEastCorner.GetY());
        
            var west = new TestPoint.XBoundary(northWestCorner.GetX());
            var east = new TestPoint.XBoundary(southEastCorner.GetX());

            return new RectangularBoundary<TestPoint, TestPoint>(
                north, south, east, west
            );
        }
    
        public QuadTreeWithListTestingWrapper(TestPoint northWestCorner, TestPoint southEastCorner) {
            var rect = FromCorners(northWestCorner, southEastCorner);
        
            this._treeWithList = new(rect);
        }
    
    
        public bool Insert(TestPoint point) => this._treeWithList.Insert(point);

        public IEnumerable<TestPoint> EnumerateRange(TestPoint northWestCorner, TestPoint southEastCorner) {
            var rect = FromCorners(northWestCorner, southEastCorner);


            return this._treeWithList.EnumerateBoundary(rect);
        }

        public bool CheckValidity() => this._treeWithList.Debug_IsQuadTree();
    }
    
    public class QuadTreeWithKdTestingWrapper : ITestable2DCollection {
        
        private readonly QuadTreeWithKdTree<TestPoint> _treeWithList;

        private static RectangularBoundary<TestPoint, TestPoint> FromCorners(TestPoint northWestCorner, TestPoint southEastCorner) {
            var north = new TestPoint.YBoundary(northWestCorner.GetY());
            var south = new TestPoint.YBoundary(southEastCorner.GetY());
        
            var west = new TestPoint.XBoundary(northWestCorner.GetX());
            var east = new TestPoint.XBoundary(southEastCorner.GetX());

            return new RectangularBoundary<TestPoint, TestPoint>(
                north, south, east, west
            );
        }
    
        public QuadTreeWithKdTestingWrapper(TestPoint northWestCorner, TestPoint southEastCorner) {
            var rect = FromCorners(northWestCorner, southEastCorner);
        
            this._treeWithList = new(
                rect, new TestPoint.DimensionalComparer(), new TestPoint.EqualityComparer()
            );
        }
    
    
        public bool Insert(TestPoint point) => this._treeWithList.Insert(point);

        public IEnumerable<TestPoint> EnumerateRange(TestPoint northWestCorner, TestPoint southEastCorner) {
            var rect = FromCorners(northWestCorner, southEastCorner);


            return this._treeWithList.EnumerateBoundary(rect);
        }

        public bool CheckValidity() => this._treeWithList.Debug_IsQuadTree();
    }
    
}
