namespace AppBackend;

using Collections;

public readonly struct GeoCoordinates {
    private readonly double _latitude;
    private readonly double _longitude;
    
    public GeoCoordinates(double latitude, double longitude) {
        if (!double.IsFinite(latitude) || latitude < -90 || latitude > 90) {
            throw new ArgumentOutOfRangeException(nameof(latitude));
        }

        if (!double.IsFinite(longitude) ||longitude < -180 || longitude > 180) {
            throw new ArgumentOutOfRangeException(nameof(longitude));
        }

        this._latitude = latitude;
        this._longitude = longitude;
    }

    public double GetLatitude() => this._latitude;
    public double GetLongitude() => this._longitude;

    public bool IsInsideCircle(ref GeoCoordinates center, double radius) {
        
        if (!double.IsFinite(radius) || radius < 0) {
            throw new ArgumentOutOfRangeException(nameof(radius));
        }
        
        const double METERS_PER_DEGREE = 111320.0;

        // NOTE: We are allowed to completely ignore earth's curvature
        double deltaLat = (this._latitude - center._latitude) * METERS_PER_DEGREE;
        double deltaLon = (this._longitude - center._longitude) * METERS_PER_DEGREE;

        double deltaLatSquared = (deltaLat * deltaLat);
        double deltaLonSquared = (deltaLon * deltaLon);
        double radiusSquared = (radius * radius);
        
        // calculate circle using formula
        return deltaLatSquared + deltaLonSquared <= radiusSquared;
    }
    
}


public class Location {
    private readonly long _id;

    private GeoCoordinates _coordinates;

    private String _name;
    private String? _description;
    private String? _address;
    
    private readonly DateTime _createdAt;   
    private DateTime _updatedAt;

    public Location(GeoCoordinates coordinates) {
        // NOTE: Intentionaly not setting anything else, for consistency sake
        this._coordinates = coordinates;
    }
    
    public Location(long id, double latitude, double longitude, String name, String? description, String? address) {
        this._id = id;
        
        this._coordinates = new GeoCoordinates(latitude, longitude);
        
        this._name = name;
        this._description = description;
        this._address = address;
        
        this._createdAt = DateTime.Now;
        this._updatedAt = DateTime.Now;
    }

    
    
    public Location(ref Location original) {
        this._id = original._id;
        
        this._coordinates = original._coordinates;
        
        this._name = original._name;
        this._description = original._description;
        this._address = original._address;
        
        this._createdAt = original._createdAt;
        this._updatedAt = original._updatedAt;
    }
    
    public long GetId() {
        return this._id;
    }
    
    
    
    public double GetLatitude() => this._coordinates.GetLatitude();
    
    public double GetLongitude() => this._coordinates.GetLongitude();

    public ref GeoCoordinates GetCoordinates() => ref this._coordinates;
    
    
    
    public void SetLatitude(double latitude) {
        this._coordinates = new GeoCoordinates(latitude, this._coordinates.GetLongitude());
        this._updatedAt = DateTime.Now;
    }
    
    public void SetLongitude(double longitude) {
        this._coordinates = new GeoCoordinates(this._coordinates.GetLatitude(), longitude);
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
                0 => a.GetLatitude().CompareTo(b.GetLatitude()),
                1 => a.GetLongitude().CompareTo(b.GetLongitude()),
                _ => throw new ArgumentOutOfRangeException(nameof(dimension), "Dimension index is out of range.")
            };
        }
    }
    
    public class LocationLatitudeBoundary : IBoundary<Location> {
        private readonly double _latitude;

        public LocationLatitudeBoundary(double latitude) {
            this._latitude = latitude;
        }

        public int RelativePosition(Location value) {
            return value.GetLatitude().CompareTo(this._latitude);
        }

        public IBoundary<Location> Midpoint(IBoundary<Location> other) {
            if (other is not LocationLatitudeBoundary otherBoundary) {
                throw new ArgumentException("Other boundary must be of type LocationLatitudeBoundary.", nameof(other));
            }

            double midpointLatitude = (this._latitude + otherBoundary._latitude) / 2.0;
            return new LocationLatitudeBoundary(midpointLatitude);
        }

        public int CompareTo(IBoundary<Location> other) {
            if (other is not LocationLatitudeBoundary otherBoundary) {
                throw new ArgumentException("Other boundary must be of type LocationLatitudeBoundary.", nameof(other));
            }

            return this._latitude.CompareTo(otherBoundary._latitude);
        }
    }
    
    public class LocationLongitudeBoundary : IBoundary<Location> {
        private readonly double _longitude;

        public LocationLongitudeBoundary(double longitude) {
            this._longitude = longitude;
        }

        public int RelativePosition(Location value) {
            return value.GetLongitude().CompareTo(this._longitude);
        }

        public IBoundary<Location> Midpoint(IBoundary<Location> other) {
            if (other is not LocationLongitudeBoundary otherBoundary) {
                throw new ArgumentException("Other boundary must be of type LocationLongitudeBoundary.", nameof(other));
            }

            double midpointLongitude = (this._longitude + otherBoundary._longitude) / 2.0;
            return new LocationLongitudeBoundary(midpointLongitude);
        }

        public int CompareTo(IBoundary<Location> other) {
            if (other is not LocationLongitudeBoundary otherBoundary) {
                throw new ArgumentException("Other boundary must be of type LocationLongitudeBoundary.", nameof(other));
            }

            return this._longitude.CompareTo(otherBoundary._longitude);
        }
    }
}


public static class CsvHelpers {
    public static string LocationToCsvLine(Location location) {
        // CSV: id,name,description,latitude,longitude,address,createdAt,updatedAt
        return $"{location.GetId()},{location.GetName()},{location.GetDescription()},{location.GetLatitude()},{location.GetLongitude()},{location.GetAddress()},{location.GetCreatedAt()},{location.GetCreatedAt()},{location.GetUpdatedAt()}";
    }
    
    
    
    
}
