using System.Runtime.CompilerServices;
using Collections;

namespace AppBackend;

public interface ILocationTree {
    
    public bool Insert(Location location);

    public bool Remove(Location location);

    public bool Update(Location oldLocation, Location newValue);
    
    public IEnumerable<Location> EnumerateRange(Location lowerBound, Location upperBound);

    public void DumpCsv(string fileName, Func<Location, string> toCsvString);

    public void LoadCsv(string fileName, Func<string, Location> fromCsvString);
}

public class KdTreeWrapper : ILocationTree {
    private readonly KdTree<Location> _tree;

    public KdTreeWrapper(KdTree<Location> tree) {

        this._tree = tree;
    }

    public bool Insert(Location location) 
        => this._tree.Insert(location);

    public bool Remove(Location location) 
        => this._tree.Remove(location);

    public bool Update(Location oldLocation, Location newValue) 
        => this._tree.Update(oldLocation, newValue);

    public IEnumerable<Location> EnumerateRange(Location lowerBound, Location upperBound) 
        => this._tree.EnumerateRange(lowerBound, upperBound);

    public void DumpCsv(string fileName, Func<Location, string> toCsvString) =>
        this._tree.DumbCsv(fileName, toCsvString);

    public void LoadCsv(string fileName, Func<string, Location> fromCsvString)
        => this._tree.LoadCsv(fileName, fromCsvString);

}

public class QuadTreeWrapper : ILocationTree {
    private readonly QuadTree<Location, IQuadTreeValueStorage<Location>> _tree;

    public QuadTreeWrapper(QuadTree<Location, IQuadTreeValueStorage<Location>> tree) {

        this._tree = tree;
    }

    public bool Insert(Location location) 
        => this._tree.Insert(location);

    public bool Remove(Location location) {
                
        throw new NotImplementedException();
    }

    public bool Update(Location oldLocation, Location newValue) {
        
        throw new NotImplementedException();
    }

    public IEnumerable<Location> EnumerateRange(Location lowerBound, Location upperBound) {
        var rect = Location.MakeRect(lowerBound, upperBound);

        return this._tree.EnumerateBoundary(rect);
    }

    public void DumpCsv(string fileName, Func<Location, string> toCsvString) =>
        this._tree.DumbCsv(fileName, toCsvString);

    public void LoadCsv(string fileName, Func<string, Location> fromCsvString)
        => this._tree.LoadCsv(fileName, fromCsvString);

}


public class LocationTreeWrapper {

    private readonly TreeChoices _treeType;
    private readonly object _treeObject;

    // --- CONSTRUCTORS ---

    public LocationTreeWrapper(KdTree<Location> tree) {
        this._treeType = TreeChoices.KdTree;
        this._treeObject = tree;
    }

    public LocationTreeWrapper(QuadTreeWithList<Location> tree) {
        this._treeType = TreeChoices.QuadTreeWithList;
        this._treeObject = tree;
    }
    
    public LocationTreeWrapper(QuadTreeWithKdTree<Location> tree) {
        this._treeType = TreeChoices.QuadTreeWithKd;
        this._treeObject = tree;
    }


    public bool Insert(Location location) {

        return this._treeType switch {
            TreeChoices.KdTree => ((KdTree<Location>)this._treeObject).Insert(location),
            TreeChoices.QuadTreeWithList => ((QuadTreeWithList<Location>)this._treeObject).Insert(location),
            TreeChoices.QuadTreeWithKd => ((QuadTreeWithKdTree<Location>)this._treeObject).Insert(location),
            _ => throw new ArgumentOutOfRangeException(nameof(this._treeType))
        };

    }

    public bool Remove(Location location) {
        return this._treeType switch {
            TreeChoices.KdTree => ((KdTree<Location>)this._treeObject).Remove(location),
            TreeChoices.QuadTreeWithList => throw new NotImplementedException(),
            TreeChoices.QuadTreeWithKd => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(this._treeType))
        };
    }

    public bool Update(Location oldLocation, Location newValue) {
        return this._treeType switch {
            TreeChoices.KdTree => ((KdTree<Location>)this._treeObject).Update(oldLocation, newValue),
            TreeChoices.QuadTreeWithList => throw new NotImplementedException(),
            TreeChoices.QuadTreeWithKd => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(this._treeType))
        };
        
    }

    public IEnumerable<Location> EnumerateRange(Location lowerBound, Location upperBound) {
        return this._treeType switch {
            TreeChoices.KdTree 
                => ((KdTree<Location>)this._treeObject).EnumerateRange(lowerBound, upperBound),
            
            TreeChoices.QuadTreeWithList 
                => ((QuadTreeWithList<Location>)this._treeObject).EnumerateBoundary( Location.MakeRect(lowerBound, upperBound) ),
            
            TreeChoices.QuadTreeWithKd
                => ((QuadTreeWithKdTree<Location>)this._treeObject).EnumerateBoundary( Location.MakeRect(lowerBound, upperBound) ),
            
            _ => throw new ArgumentOutOfRangeException(nameof(this._treeType))
        };
        
        
    }

    public void DumpCsv(string fileName, Func<Location, string> toCsvString) {
        switch (this._treeType) {
            case TreeChoices.KdTree: {
                ((KdTree<Location>)this._treeObject).DumbCsv(fileName, toCsvString);
                break;
            }

            case TreeChoices.QuadTreeWithList: {
                ((QuadTreeWithList<Location>)this._treeObject).DumbCsv(fileName, toCsvString);
                break;
            }

            case TreeChoices.QuadTreeWithKd: {
                ((QuadTreeWithKdTree<Location>)this._treeObject).DumbCsv(fileName, toCsvString);
                break;
            }

            default: {
                throw new ArgumentOutOfRangeException(nameof(this._treeType));
            }
        }
        
    }

    public void LoadCsv(string fileName, Func<string, Location> fromCsvString) {
        
        switch (this._treeType) {
            case TreeChoices.KdTree: {
                ((KdTree<Location>)this._treeObject).LoadCsv(fileName, fromCsvString);
                break;
            }

            case TreeChoices.QuadTreeWithList: {
                ((QuadTreeWithList<Location>)this._treeObject).LoadCsv(fileName, fromCsvString);
                break;
            }

            case TreeChoices.QuadTreeWithKd: {
                ((QuadTreeWithKdTree<Location>)this._treeObject).LoadCsv(fileName, fromCsvString);
                break;
            }

            default: {
                throw new ArgumentOutOfRangeException(nameof(this._treeType));
            }
        }
        
    }
    
}