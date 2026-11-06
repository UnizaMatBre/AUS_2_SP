namespace AppBackend;

public interface ILocationTree {
    
    public bool Insert(Location location);
    
    public IEnumerable<Location> EnumerateRange(Location lowerBound, Location upperBound);
    
}

public class Model {
    private readonly ILocationTree _locationTree;

    public Model(ILocationTree locationTree) {
        this._locationTree = locationTree;
    }
    
    
}