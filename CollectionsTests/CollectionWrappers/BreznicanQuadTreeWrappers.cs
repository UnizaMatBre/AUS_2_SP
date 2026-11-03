using Collections;

namespace CollectionsTests.Breznican {
    public class QuadTreeWithListTestingWrapper : ITestable2DCollection {
        private readonly Collections.QuadTreeWithList<TestPoint> _treeWithList;

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
        
            this._treeWithList = new Collections.QuadTreeWithList<TestPoint>(rect);
        }
    
    
        public bool Insert(TestPoint point) => this._treeWithList.Insert(point);

        public IEnumerable<TestPoint> EnumerateRange(TestPoint northWestCorner, TestPoint southEastCorner) {
            var rect = FromCorners(northWestCorner, southEastCorner);


            return this._treeWithList.EnumerateBoundary(rect);
        }

        public bool CheckValidity() => this._treeWithList.Debug_IsQuadTree();
    }
}
