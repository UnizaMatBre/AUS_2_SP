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

        throw new NotImplementedException();
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
    public IEnumerable<Location> DoGetLocationsWithStringInName(String searchedString) {

        throw new NotImplementedException();
    }
    
    // 6
    public bool DoRemoveLocation(long locationId) {

        throw new NotImplementedException();
    }
    
    
    
    
    
}