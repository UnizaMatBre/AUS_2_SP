using Collections;

namespace AppBackend;

public interface ILocationTree {
    
    public bool Insert(Location location);

    public bool Remove(Location location);
    
    public IEnumerable<Location> EnumerateRange(Location lowerBound, Location upperBound);
    
}

public class Model {
    private readonly ILocationTree _locationTree;

    public Model(ILocationTree locationTree) {
        this._locationTree = locationTree;
    }
    


    public bool DoGenerateRandomData() {

        throw new NotImplementedException();
    }
    
    public bool DoDumpDataIntoFile() {
        
        throw new NotImplementedException();
    }
    
    public bool DoLoadDataFromFile() {
        
        throw new NotImplementedException();
    }

    

    // 2
    public IEnumerable<Location> DoGetLocationsInCircle(GeoCoordinates center, double radius) {

        throw new NotImplementedException();
    }
    
    // 3
    public bool DoCreateLocation(Location location) {
        var localCopy = new Location(ref location);


        return this._locationTree.Insert(localCopy);
    }
    
    
    // 4.1 - non-coordinate edit
    public bool DoEditLocationNonCoordinate(/* ADD PROPER PARAMETERS HERE */) {

        throw new NotImplementedException();
    }
    
    // 4.2 - coordinate edit
    public bool DoEditLocationCoordinates(/* ADD PROPER PARAMETERS HERE */) {

        throw new NotImplementedException();
    }
    
    // 5
    public IEnumerable<Location> DoGetLocationsWithStringInName(Location upperLeft, Location lowerRight, String searchedString) {
        var rectLocations = this._locationTree.EnumerateRange(upperLeft, lowerRight);

        return rectLocations
            .Where((oneLocation) => oneLocation.GetName().Contains(searchedString))
            .Select((oneLocation) => new Location(ref oneLocation));
    }
    
    // 6
    public bool DoRemoveLocation(Location location) {

        return this._locationTree.Remove(location);
    }
    
    
    
    
    
}