namespace AppBackend;

public interface ILocationTree {
    
    public bool Insert(Location location);

    public bool Remove(Location location);

    public bool Update(Location oldLocation, Location newValue);
    
    public IEnumerable<Location> EnumerateRange(Location lowerBound, Location upperBound);

    public void DumpCsv(string fileName, Func<Location, string> toCsvString);

    public void LoadCsv(string fileName, Func<string, Location> fromCsvString);
}

