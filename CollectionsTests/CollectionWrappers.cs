using System.Numerics;
using System.Security.AccessControl;
using Auios.QuadTree;
using Collections;
using Omni.Collections.Spatial;
using QuadTrees;
using QuadTrees.Common;
using SpatialTrees.Quadtrees;

namespace CollectionsTests;


public class KdTreeTestingWrapper : ITestable2DCollection {
    private KdTree<TestPoint> _tree = new(new TestPoint.DimensionalComparer());


    public bool Insert(TestPoint point) => this._tree.Insert(point);
    
    public IEnumerable<TestPoint> EnumerateRange(TestPoint lower, TestPoint upper) => this._tree.EnumerateRange(lower, upper);

    public bool CheckValidity() => this._tree.Debug_IsKdTree();
}

public class QuadTreeTestingWrapper : ITestable2DCollection {
    private readonly Collections.QuadTree<TestPoint> _tree;

    private static RectangularBoundary<TestPoint, TestPoint> FromCorners(TestPoint northWestCorner, TestPoint southEastCorner) {
        var north = new TestPoint.YBoundary(northWestCorner.GetY());
        var south = new TestPoint.YBoundary(southEastCorner.GetY());
        
        var west = new TestPoint.XBoundary(northWestCorner.GetX());
        var east = new TestPoint.XBoundary(southEastCorner.GetX());

        return new RectangularBoundary<TestPoint, TestPoint>(
            north, south, east, west
        );
    }
    
    public QuadTreeTestingWrapper(TestPoint northWestCorner, TestPoint southEastCorner) {
        var rect = FromCorners(northWestCorner, southEastCorner);
        
        this._tree = new Collections.QuadTree<TestPoint>(rect);
    }
    
    
    public bool Insert(TestPoint point) => this._tree.Insert(point);

    public IEnumerable<TestPoint> EnumerateRange(TestPoint northWestCorner, TestPoint southEastCorner) {
        var rect = FromCorners(northWestCorner, southEastCorner);


        return this._tree.EnumerateBoundary(rect);
    }

    public bool CheckValidity() => this._tree.Debug_IsQuadTree();
}


public class OmnoCollectionQuadTreeWrapper : ITestable2DCollection {
    private readonly Omni.Collections.Spatial.QuadTree<TestPoint> _tree;

    private static Omni.Collections.Spatial.Rectangle MakeRectangle(TestPoint northWestCorner, TestPoint southEastCorner) {
        double width = southEastCorner.GetX() - northWestCorner.GetX();
        double height = southEastCorner.GetY() - northWestCorner.GetY();
        
        
        return new Omni.Collections.Spatial.Rectangle(
            northWestCorner.GetX(), northWestCorner.GetY(),
            width, height
        );
        
    }
    
    public OmnoCollectionQuadTreeWrapper(TestPoint northWestCorner, TestPoint southEastCorner) {
        var rect = MakeRectangle(northWestCorner, southEastCorner);

        this._tree = new(rect);
    }

    public bool Insert(TestPoint point) {

        try {
            // this one throws on invalid input
            this._tree.Insert(
                new(point.GetX(), point.GetY()), point
            );
        }
        catch (ArgumentException e) {
            return false;
        }

        return true;
    }

    public IEnumerable<TestPoint> EnumerateRange(TestPoint northWestCorner, TestPoint southEastCorner) {
        var rect = MakeRectangle(northWestCorner, southEastCorner);

        var result = this._tree.Query(rect);
        
        return result;
    }

    public bool CheckValidity() {
        throw new NotImplementedException();
    }
}


public class AuiosQuadTreeWrapper : ITestable2DCollection {
    private class CustomBounds : Auios.QuadTree.IQuadTreeObjectBounds<TestPoint>
    {
        public float GetTop(TestPoint obj) => (float)obj.GetY();

        public float GetBottom(TestPoint obj) => (float)obj.GetY();

        public float GetLeft(TestPoint obj) => (float)obj.GetX();

        public float GetRight(TestPoint obj) => (float)obj.GetX();
    }


    private readonly Auios.QuadTree.QuadTree<TestPoint> _tree;

    public AuiosQuadTreeWrapper(TestPoint northWestCorner, TestPoint southEastCorner) {
        float width =  (float)(southEastCorner.GetX() - northWestCorner.GetX());
        float height = (float)(southEastCorner.GetY() - northWestCorner.GetY());
        
        
        this._tree = new( 
            new Vector2((float)northWestCorner.GetX(), (float)northWestCorner.GetY()),
            new Vector2(width, height),
            new CustomBounds()
        );
    }

    public bool Insert(TestPoint point) => this._tree.Insert(point);

    public IEnumerable<TestPoint> EnumerateRange(TestPoint northWestCorner, TestPoint southEastCorner) {
        
        float width =  (float)(southEastCorner.GetX() - northWestCorner.GetX());
        float height = (float)(southEastCorner.GetY() - northWestCorner.GetY());

        Auios.QuadTree.QuadTreeRect searchArea = new(
            (float)northWestCorner.GetX(), (float)northWestCorner.GetY(),
            width, height
        );
        
        
        return this._tree.FindObjects(searchArea);
    }

    public bool CheckValidity()
    {
        throw new NotImplementedException();
    }
}

public class HrogerQuadTreeWrapper : ITestable2DCollection {
    private static FastG.AARectangle MakeRect(TestPoint northWestCorner, TestPoint southEastCorner) {
        float width =  (float)(southEastCorner.GetX() - northWestCorner.GetX());
        float height = (float)(southEastCorner.GetY() - northWestCorner.GetY());
        
        return new  FastG.AARectangle(
            (float)northWestCorner.GetX(), (float)northWestCorner.GetY(),
            width, height    
        );
    }
    
    private readonly SpatialTrees.Quadtrees.Quadtree _tree;

    public HrogerQuadTreeWrapper(TestPoint northWestCorner, TestPoint southEastCorner) {
        var rect = MakeRect(northWestCorner, southEastCorner);
        
        this._tree = new(rect);
    }

    public bool Insert(TestPoint point) {

        try {
            // this one throws on invalid input
            this._tree.AddItem(point);
        }
        catch (ArgumentException e) {
            return false;
        }

        return true;
    }

    public IEnumerable<TestPoint> EnumerateRange(TestPoint northWestCorner, TestPoint southEastCorner) {
        var rect = MakeRect(northWestCorner, southEastCorner);
        
        
        var found = new List<IMapObject2d>();
        bool anyFound = this._tree.GetCollidingItems(rect, 1, found);

        return found.Select((value) => (TestPoint)value);
    }

    public bool CheckValidity() {
        
        throw new NotImplementedException();
    }
}