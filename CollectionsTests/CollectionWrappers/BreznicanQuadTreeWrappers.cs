using Collections;

namespace CollectionsTests.Breznican {
    public class QuadTreeWithListTestingWrapper : ITestable2DCollection {
        public class ListStorage : Collections.IQuadTreeValueStorage<TestPoint> {
            private readonly List<TestPoint> _list = new();
            
            public int Count() => this._list.Count;
            
            public bool Insert(TestPoint value) {
                this._list.Add(value);
                return true;
            }

            public IEnumerable<TestPoint> EnumerateAll() => this._list;
        }
        
        private readonly Collections.QuadTree<TestPoint, ListStorage> _treeWithList;

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
        private class KdTreeStorage : Collections.IQuadTreeValueStorage<TestPoint> {
            private readonly KdTree<TestPoint> _tree = new(new TestPoint.DimensionalComparer());
            
            public int Count() => this._tree.Count();
            
            public bool Insert(TestPoint value) => this._tree.Insert(value);

            public IEnumerable<TestPoint> EnumerateAll() => this._tree.EnumerateAll();
        }
        
        private readonly Collections.QuadTree<TestPoint, KdTreeStorage> _treeWithList;

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
        
            this._treeWithList = new(rect);
        }
    
    
        public bool Insert(TestPoint point) => this._treeWithList.Insert(point);

        public IEnumerable<TestPoint> EnumerateRange(TestPoint northWestCorner, TestPoint southEastCorner) {
            var rect = FromCorners(northWestCorner, southEastCorner);


            return this._treeWithList.EnumerateBoundary(rect);
        }

        public bool CheckValidity() => this._treeWithList.Debug_IsQuadTree();
    }
    
}
