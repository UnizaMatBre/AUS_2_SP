using Collections;

namespace AppBackend;

public struct Location {
    private readonly long _id;
    
    private double _latitude;
    private double _longitude;

    private String _name;
    private String? _description;
    private String? _address;
    
    private readonly DateTime _createdAt;   
    private DateTime _updatedAt;
    
    public Location(long id, double latitude, double longitude, String name, String? description, String? address) {
        this._id = id;
        
        this._latitude = latitude;
        this._longitude = longitude;
        
        this._name = name;
        this._description = description;
        this._address = address;
        
        this._createdAt = DateTime.Now;
        this._updatedAt = DateTime.Now;
    }
    
    public long GetId() {
        return this._id;
    }
    
    
    
    public double GetLatitude() {
        return this._latitude;
    }
    
    public void SetLatitude(double latitude) {
        this._latitude = latitude;
        this._updatedAt = DateTime.Now;
    }
    
    
    
    public double GetLongitude() {
        return this._longitude;
    }
    
    public void SetLongitude(double longitude) {
        this._longitude = longitude;
        this._updatedAt = DateTime.Now;
    }
    
    
    
    public String GetName() {
        return this._name;
    }
    
    public void SetName(String name) {
        this._name = name;
        this._updatedAt = DateTime.Now;
    }

    
    
    public String? GetDescription() {
        return this._description;
    }
    
    public void SetDescription(String? description) {
        this._description = description;
        this._updatedAt = DateTime.Now;
    }
    
    
    public String? GetAddress() {
        return this._address;
    }
    
    public void SetAddress(String? address) {
        this._address = address;
        this._updatedAt = DateTime.Now;
    }
    
    
    public DateTime GetCreatedAt() {
        return this._createdAt;
    }
    
    public DateTime GetUpdatedAt() {
        return this._updatedAt;
    }

    public class LocationDimensionalComparer : IDimensionalComparer<Location> {
        public uint GetDimensionCount() => 2;

        public int DimensionalCompare(uint dimension, Location a, Location b) {
            return dimension switch {
                0 => a._latitude.CompareTo(b._latitude),
                1 => a._longitude.CompareTo(b._longitude),
                _ => throw new ArgumentOutOfRangeException(nameof(dimension), "Dimension index is out of range.")
            };
        }
    }
}
