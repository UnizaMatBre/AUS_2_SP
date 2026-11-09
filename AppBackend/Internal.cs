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

