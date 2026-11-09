namespace AppBackend;

using Collections;



public enum TreeChoices {
    KdTree,
    QuadTreeWithList,
    QuadTreeWithKd
}

public readonly struct GeoCoordinates {
    public const double METERS_PER_DEGREE = 111320.0;
    
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

    public GeoCoordinates(ref GeoCoordinates other) {
        this._latitude = other._latitude;
        this._longitude = other._longitude;
    }
    
    public double GetLatitude() => this._latitude;
    public double GetLongitude() => this._longitude;

    public bool IsInsideCircle(ref GeoCoordinates center, double radius) {
        
        if (!double.IsFinite(radius) || radius < 0) {
            throw new ArgumentOutOfRangeException(nameof(radius));
        }

        // NOTE: We are allowed to completely ignore earth's curvature
        double deltaLat = (this._latitude - center._latitude) * GeoCoordinates.METERS_PER_DEGREE;
        double deltaLon = (this._longitude - center._longitude) * GeoCoordinates.METERS_PER_DEGREE;

        double deltaLatSquared = (deltaLat * deltaLat);
        double deltaLonSquared = (deltaLon * deltaLon);
        double radiusSquared = (radius * radius);
        
        // calculate circle using formula
        return deltaLatSquared + deltaLonSquared <= radiusSquared;
    }
    
    public GeoCoordinates MoveTo(double deltaLatMetres, double deltaLonMetres) {

        double newLat = this._latitude + (deltaLatMetres / GeoCoordinates.METERS_PER_DEGREE);
        double newLon = this._longitude + (deltaLonMetres / GeoCoordinates.METERS_PER_DEGREE);

        return new GeoCoordinates(newLat, newLon);
    }
    
}

public class LocalInformation {
    public String Name;
    public String? Description;
    public String? Address;

    public DateTime CreatedAt = DateTime.Now;
    public DateTime UpdatedAt = DateTime.Now;
    
    public void UpdateName(String newName) {
        this.Name = newName;
        this.UpdatedAt = DateTime.Now;
    }
    
    public void UpdateDescription(String? newDescription) {
        this.Description = newDescription;
        this.UpdatedAt = DateTime.Now;
    }
    
    public void UpdateAddress(String? newAddress) {
        this.Address = newAddress;
        this.UpdatedAt = DateTime.Now;
    }
}



public struct Location {
    public long Id;
    
    public GeoCoordinates Coordinates;
    public LocalInformation? Information;

    
    public Location(ref Location other) {
        this.Id = other.Id;
        this.Coordinates = other.Coordinates;
        this.Information = null;
        
        if (other.Information is not null) {
            this.Information = new LocalInformation {
                Name = other.Information.Name,
                
                Description = other.Information.Description,
                Address = other.Information.Address,
                
                CreatedAt = other.Information.CreatedAt,
                UpdatedAt = other.Information.UpdatedAt
            };
        }
    }
    
    public Location(long id, GeoCoordinates coordinates, LocalInformation? information = null) {
        this.Id = id;
        this.Coordinates = coordinates;
        this.Information = information;
    }
    
    public Location(long id, double latitude, double longitude, LocalInformation? information = null) {
        this.Id = id;
        this.Coordinates = new GeoCoordinates(latitude, longitude);
        this.Information = information;
    }
    

    public bool HasInformation() => this.Information is not null;
    

    public class LocationDimensionalComparer : IDimensionalComparer<Location> {
        public uint GetDimensionCount() => 2;

        public int DimensionalCompare(uint dimension, Location a, Location b) {
            var aCoord = a.Coordinates;
            var bCoord = b.Coordinates;
            
            return dimension switch {
                0 => aCoord.GetLatitude().CompareTo(bCoord.GetLatitude()),
                1 => aCoord.GetLongitude().CompareTo(bCoord.GetLongitude()),
                _ => throw new ArgumentOutOfRangeException(nameof(dimension), "Dimension index is out of range.")
            };
        }
    }

    public class LocationEqualityComparer : IEqualityComparer<Location> {
        public bool Equals(Location x, Location y) {
            return x.Id == y.Id;
        }

        public int GetHashCode(Location obj) {
            return obj.Id.GetHashCode();
        }
    }
    
    public class LocationWestEastBoundary : IBoundary<Location> {
        private readonly double _longitude;

        public LocationWestEastBoundary(double longitude) {
            
            this._longitude = longitude;
        }

        public LocationWestEastBoundary(Location location) {
            
            this._longitude = location.Coordinates.GetLongitude();
        }

        public int RelativePosition(Location value) {
            
            return value.Coordinates.GetLongitude().CompareTo(this._longitude);
        }

        public IBoundary<Location> Midpoint(IBoundary<Location> other) {
            if (other is not LocationWestEastBoundary otherBoundary) {
                throw new ArgumentException("Other boundary must be of type LocationWestEastBoundary.", nameof(other));
            }

            double midpointLongitude = (this._longitude + otherBoundary._longitude) / 2.0;
            return new LocationWestEastBoundary(midpointLongitude);
        }

        public int CompareTo(IBoundary<Location> other) {
            if (other is not LocationWestEastBoundary otherBoundary) {
                throw new ArgumentException("Other boundary must be of type LocationWestEastBoundary.", nameof(other));
            }

            return this._longitude.CompareTo(otherBoundary._longitude);
        }
    }
    
    public class LocationNorthSouthBoundary : IBoundary<Location> {
        private readonly double _latitude;

        public LocationNorthSouthBoundary(double latitude) {
            
            this._latitude = latitude;
        }

        public LocationNorthSouthBoundary(Location location) {

            this._latitude = location.Coordinates.GetLatitude();
        }

        public int RelativePosition(Location value) {
            
            return value.Coordinates.GetLatitude().CompareTo(this._latitude);
        }

        public IBoundary<Location> Midpoint(IBoundary<Location> other) {
            if (other is not LocationNorthSouthBoundary otherBoundary) {
                throw new ArgumentException("Other boundary must be of type LocationNorthSouthBoundary.", nameof(other));
            }

            double midpointLatitude = (this._latitude + otherBoundary._latitude) / 2.0;
            return new LocationNorthSouthBoundary(midpointLatitude);
        }

        public int CompareTo(IBoundary<Location> other) {
            if (other is not LocationNorthSouthBoundary otherBoundary) {
                throw new ArgumentException("Other boundary must be of type LocationNorthSouthBoundary.", nameof(other));
            }

            return this._latitude.CompareTo(otherBoundary._latitude);
        }
    }
}


public static class CsvHelpers {
    public static string LocationToCsvLine(Location location) {
        var coord = location.Coordinates;
        var info = location.Information;

        if (info is null) {
            throw new ArgumentNullException(nameof(location.Information), "Location must have non-null LocalInformation to convert to CSV line.");
        }
        
        // CSV: id,name,description,latitude,longitude,address,createdAt,updatedAt
        return $"{location.Id},{info.Name},{info.Description},{coord.GetLatitude()},{coord.GetLongitude()},{info.Address},{info.CreatedAt},{info.UpdatedAt}";
    }
    
    public static Location LocationFromCsvLine(string csvLine) {
        var parts = csvLine.Split(',');
        if (parts.Length != 8) {
            throw new FormatException("Invalid CSV line for Location.");
        }


        var info = new LocalInformation {
            Name = parts[1],
            
            Description = string.IsNullOrEmpty(parts[2]) ? null : parts[2],
            Address = string.IsNullOrEmpty(parts[5]) ? null : parts[5],
            
            CreatedAt = DateTime.Parse(parts[6]),
            UpdatedAt = DateTime.Parse(parts[7])
        };
        
        var coordinates = new GeoCoordinates(
            double.Parse(parts[3]), double.Parse(parts[4])
        );

        
        var id = long.Parse(parts[0]);
        
        return new Location(id, coordinates, info);
    }
    
    
    
    
}
