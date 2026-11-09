using System.Runtime.CompilerServices;
using Collections;

namespace AppBackend;

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