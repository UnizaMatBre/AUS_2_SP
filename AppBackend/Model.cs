using Collections;

namespace AppBackend;

public interface ILocationTree {
    
    public bool Insert(Location location);

    public bool Remove(Location location);

    public bool Update(Location oldLocation, Location newValue);
    
    public IEnumerable<Location> EnumerateRange(Location lowerBound, Location upperBound);
    
}

public class Model {
    public const long INVALID_ID = -1;
    
    private long _idCounter = 0;
    
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

    // 1
    public IEnumerable<Location> DoGetLocationsInRectangle(Location upperLeft, Location lowerRight) {
        
        return this._locationTree.EnumerateRange(upperLeft, lowerRight);
    }

    // 2
    public IEnumerable<Location> DoGetLocationsInCircle(Location center, double radius) {

        var centerCoord = center.Coordinates;

        var northEastCorner = new Location(
            Model.INVALID_ID,centerCoord.MoveTo(-radius, -radius)
        );
        
        var southWestCorner = new Location(
            Model.INVALID_ID, centerCoord.MoveTo(radius, radius)
        );
        
        var rectLocations = this._locationTree.EnumerateRange(southWestCorner, northEastCorner);

        return rectLocations;
    }
    
    // 3
    public bool DoCreateLocation(GeoCoordinates coordinates, String name, String? description, String? address) {
        var newInformation = new LocalInformation {
            Name = name,
            Description = description,
            Address = address,
            
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        
        var newLocation = new Location(
            this._idCounter++,
            coordinates, 
            newInformation
        );

        return this._locationTree.Insert(newLocation);
    }
    
    
    // 4.1 - non-coordinate edit
    public bool DoEditLocationNonCoordinate(/* ADD PROPER PARAMETERS HERE */) {

        throw new NotImplementedException();
    }
    
    // 4.2 - coordinate edit
    public bool DoEditLocationCoordinates(Location oldLocation, GeoCoordinates coordinates) {
        var removeResult = this._locationTree.Remove(oldLocation);
        if (removeResult == false) {
            return false;
        }
        
        var newLocation = oldLocation with {
            Coordinates = coordinates
        };

        return this._locationTree.Insert(newLocation);
    }
    
    // 5
    public IEnumerable<Location> DoGetLocationsWithStringInName(Location upperLeft, Location lowerRight, String searchedString) {
        var rectLocations = this._locationTree.EnumerateRange(upperLeft, lowerRight);

        return rectLocations
            .Where((oneLocation) => oneLocation.Information!.Name.Contains(searchedString))
            .Select((oneLocation) => new Location(ref oneLocation));
    }
    
    // 6
    public bool DoRemoveLocation(Location location) {

        return this._locationTree.Remove(location);
    }
    
    
    
    
    
}