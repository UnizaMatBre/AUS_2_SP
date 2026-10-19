using Collections;

namespace CollectionsTests;


public class Town {
    private readonly string _name;
    private readonly int _x;
    private readonly int _y;

    public Town(string name, int x, int y) {
        this._name = name;
        this._x = x;
        this._y = y;
    }
    
    public override string ToString() => $"{this._name}({this._x}, {this._y})";
    
    public class DimensionalComparer : IDimensionalComparer<Town> {
        public uint GetDimensionCount() => 2;

        public int DimensionalCompare(uint dimension, Town a, Town b) {
            return dimension switch {
                0 => a._x.CompareTo(b._x),
                1 => a._y.CompareTo(b._y),
                _ => throw new ArgumentOutOfRangeException(nameof(dimension))
            };
        }
    }
}

public class KdExampleTest
{
    private readonly List<Town> _towns = [
        new("Nitra", 23, 35),
        new("Sereď", 20, 33),
        new("Topolčany", 25, 36),
        new("Galanta", 16, 31),
        new("Senica", 14, 39),
        new("Tlmače", 28, 34),
        new("Bošany", 24, 40),
        new("Bratislava", 13, 32),
        new("Hodonin", 12, 41),
        new("Trnava", 17, 42),
        new("Moravce", 26,35),
        new("Levice", 30,33),
        new("Bojnice", 29, 46),
        new("Novaky", 27, 43)
    ];

    private KdTree<Town> _tree;

    public KdExampleTest()
    {
        this._tree = new(new Town.DimensionalComparer());

        foreach (var oneTown in this._towns) {
            
            this._tree.Insert(oneTown);
        }


        Town lowerBound = new Town("lower", 12, 35);
        Town upperBound = new Town("upper", 26, 48);
        
        
        foreach (var oneTown in this._tree.EnumerateRange(lowerBound, upperBound)) {
            
            Console.WriteLine(oneTown);
        }
        
        return;
    }


}