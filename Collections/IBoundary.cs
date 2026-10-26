using System.Text;

namespace Collections;

public interface IBoundary<TValue> {

    /// <summary>
    /// Determines on which side of boundary is value located
    /// </summary>
    /// <param name="value">Value whose relative position we want to calculate.</param>
    /// <returns>
    ///		<list type="table">
    ///			<item>
    ///				<term>-1</term>
    ///				<description><paramref name="value" /> is on left side relatively to boundary.</description>
    ///			</item>
    ///			<item>
    ///				<term>0</term>
    ///				<description><paramref name="value" /> is right on the boundary.</description>
    ///			</item>
    ///			<item>
    ///				<term>+1</term>
    ///				<description><paramref name="value" /> is on right side relatively to boundary.</description>
    ///			</item>
    ///		</list>
    /// </returns>
    public int RelativePosition(TValue value);

    /// <summary>
    /// Calculates boundary located in middle position between two boundaries.
    /// </summary>
    /// <param name="other">Other boundary to calculate midpoint from</param>
    /// <returns>New boundary which is right in the middle between original boundaries.</returns>
    public IBoundary<TValue> Midpoint(IBoundary<TValue> other);


    public int CompareTo(IBoundary<TValue> other);
}



public class RectangularBoundary<TX, TY> {
    
    private readonly IBoundary<TY> _north;
    private readonly IBoundary<TY> _south;
    private readonly IBoundary<TY> _midY;
    
    private readonly IBoundary<TX> _east;
    private readonly IBoundary<TX> _west;
    private readonly IBoundary<TX> _midX;
    
    public IBoundary<TX> GetEastBound() => this._east;
    public IBoundary<TX> GetWestBound() => this._west;
    public IBoundary<TX> GetMidXBound() => this._midX;

    public IBoundary<TY> GetNorthBound() => this._north;
    public IBoundary<TY> GetSouthBound() => this._south;
    public IBoundary<TY> GetMidYBound() => this._midY;
    
    
    public RectangularBoundary(IBoundary<TY> north, IBoundary<TY> south, IBoundary<TX> east, IBoundary<TX> west) {
        ArgumentNullException.ThrowIfNull(north);
        ArgumentNullException.ThrowIfNull(south);
        ArgumentNullException.ThrowIfNull(east);
        ArgumentNullException.ThrowIfNull(west);
        
        this._north = north;
        this._south = south;
        this._midY = north.Midpoint(south);
        
        this._east = east;
        this._west = west;
        this._midX = east.Midpoint(west);
    }
    
    
    public RectangularBoundary<TX, TY> SubBoundary(Quadrant quadrant) {
       return quadrant switch {
            Quadrant.NORTH_WEST => new RectangularBoundary<TX, TY>(this._north, this._midY, this._midX, this._west),
            Quadrant.NORTH_EAST => new RectangularBoundary<TX, TY>(this._north, this._midY, this._east, this._midX),
            Quadrant.SOUTH_EAST => new RectangularBoundary<TX, TY>(this._midY, this._south, this._east, this._midX),
            Quadrant.SOUTH_WEST => new RectangularBoundary<TX, TY>(this._midY, this._south, this._midX, this._west),
            _ => throw new ArgumentOutOfRangeException(nameof(quadrant), quadrant, null)
        };
    }

    public bool Contains(TX x, TY y) {
        var northIn = this._north.RelativePosition(y) > 0;
        var southIn = this._south.RelativePosition(y) <= 0;
        
        var westIn = this._west.RelativePosition(x) > 0;
        var eastIn = this._east.RelativePosition(x) <= 0;
        
        return northIn && southIn && eastIn && westIn;
    }


    public Quadrant LocateQuadrantPosition(TX x, TY y) {
        bool inNorth = this._midY.RelativePosition(y) <= 0;
        bool inEast = this._midX.RelativePosition(x) <= 0;

        return (inNorth, inEast) switch {
            (true, true) => Quadrant.NORTH_EAST,
            (true, false) => Quadrant.NORTH_WEST,
            (false, true) => Quadrant.SOUTH_EAST,
            (false, false) => Quadrant.SOUTH_WEST
        };
    }

    public bool Intersect(RectangularBoundary<TX, TY> other) {
        ArgumentNullException.ThrowIfNull(other);
        
        bool overlapX =
            this._west.CompareTo(other._east) <= 0 &&
            this._east.CompareTo(other._west) >= 0;

        bool overlapY =
            this._south.CompareTo(other._north) <= 0 &&
            this._north.CompareTo(other._south) >= 0;

        return overlapX && overlapY;
    }




    public bool CanSplit() {
        bool northValid = this._north.CompareTo(this._midY) < 0;
        bool southValid = this._midY.CompareTo(this._south) < 0;
        bool westValid = this._west.CompareTo(this._midX) < 0;
        bool eastValid = this._midX.CompareTo(this._east) < 0;
        
        return northValid && southValid && westValid && eastValid;
    }
}
     