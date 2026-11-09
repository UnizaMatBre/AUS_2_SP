using Collections;

namespace AppBackend;



public class Model {
    public const long INVALID_ID = -1;
    
    private long _idCounter = 0;

    private readonly LocationTreeWrapper _treeWrapper;
    
    public Model(TreeChoices treeChoice) {

        this._treeWrapper = LocationTreeWrapper.FromTreeChoice(INVALID_ID, treeChoice);
    }
    


    public bool DoGenerateRandomData() {

        throw new NotImplementedException();
    }
    
    public bool DoDumpDataIntoFile() {

        this._treeWrapper.DumpCsv("locations.csv", CsvHelpers.LocationToCsvLine);
        return true;
    }
    
    public bool DoLoadDataFromFile() {
        
        this._treeWrapper.LoadCsv("locations.csv", CsvHelpers.LocationFromCsvLine);
        return true;
    }

    // 1
    public IEnumerable<Location> DoGetLocationsInRectangle(Location upperLeft, Location lowerRight) {
        
        return this._treeWrapper.EnumerateRange(upperLeft, lowerRight);
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
        
        var rectLocations = this._treeWrapper.EnumerateRange(southWestCorner, northEastCorner);

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

        return this._treeWrapper.Insert(newLocation);
    }
    
    
    // 4.1 - non-coordinate edit
    public bool DoEditLocationNonCoordinate(Location targetLocation, InformationDiff infoDiff) {
        
        var copyInfo = targetLocation.Information!.Copy();
        copyInfo.UpdateAll(infoDiff);
        
        var newLocation = new Location(
            targetLocation.Id,
            targetLocation.Coordinates,
            copyInfo
        );

        return this._treeWrapper.Update(targetLocation, newLocation);
    }
    
    // 4.2 - coordinate edit
    public bool DoEditLocationCoordinates(Location oldLocation, GeoCoordinates coordinates) {
        var removeResult = this._treeWrapper.Remove(oldLocation);
        if (removeResult == false) {
            return false;
        }
        
        var newLocation = oldLocation with {
            Coordinates = coordinates
        };

        return this._treeWrapper.Insert(newLocation);
    }
    
    // 5
    public IEnumerable<Location> DoGetLocationsWithStringInName(Location upperLeft, Location lowerRight, String searchedString) {
        var rectLocations = this._treeWrapper.EnumerateRange(upperLeft, lowerRight);

        return rectLocations
            .Where((oneLocation) => oneLocation.Information!.Name.Contains(searchedString))
            .Select((oneLocation) => new Location(ref oneLocation));
    }
    
    // 6
    public bool DoRemoveLocation(Location location) {

        return this._treeWrapper.Remove(location);
    }
    
    
    
    
    
}