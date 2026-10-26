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
    
    private readonly IBoundary<TY> _minY;
    private readonly IBoundary<TY> _maxY;
    private readonly IBoundary<TY> _midY;
    
    private readonly IBoundary<TX> _maxX;
    private readonly IBoundary<TX> _minX;
    private readonly IBoundary<TX> _midX;
    
    
    public RectangularBoundary(IBoundary<TY> minY, IBoundary<TY> maxY, IBoundary<TX> maxX, IBoundary<TX> minX) {
        ArgumentNullException.ThrowIfNull(minY);
        ArgumentNullException.ThrowIfNull(maxY);
        ArgumentNullException.ThrowIfNull(maxX);
        ArgumentNullException.ThrowIfNull(minX);
        
        this._minY = minY;
        this._maxY = maxY;
        this._midY = minY.Midpoint(maxY);
        
        this._maxX = maxX;
        this._minX = minX;
        this._midX = maxX.Midpoint(minX);
    }
    
    
    public RectangularBoundary<TX, TY> SubBoundary(Quadrant quadrant) {
       return quadrant switch {
            Quadrant.NORTH_WEST => new RectangularBoundary<TX, TY>(this._minY, this._midY, this._midX, this._minX),
            Quadrant.NORTH_EAST => new RectangularBoundary<TX, TY>(this._minY, this._midY, this._maxX, this._midX),
            Quadrant.SOUTH_EAST => new RectangularBoundary<TX, TY>(this._midY, this._maxY, this._maxX, this._midX),
            Quadrant.SOUTH_WEST => new RectangularBoundary<TX, TY>(this._midY, this._maxY, this._midX, this._minX),
            _ => throw new ArgumentOutOfRangeException(nameof(quadrant), quadrant, null)
        };
    }

    public bool Contains(TX x, TY y) {
        var minYIn = this._minY.RelativePosition(y) > 0;
        var maxYIn = this._maxY.RelativePosition(y) <= 0;
        
        var minXIn = this._minX.RelativePosition(x) > 0;
        var maxXIn = this._maxX.RelativePosition(x) <= 0;
        
        return minYIn && maxYIn && maxXIn && minXIn;
    }


    public Quadrant LocateQuadrantPosition(TX x, TY y) {
        bool inNorth = this._midY.RelativePosition(y) <= 0;
        bool inWest = this._midX.RelativePosition(x) <= 0;

        return (inNorth, inWest) switch {
            (true, true) => Quadrant.NORTH_WEST,
            (true, false) => Quadrant.NORTH_EAST,
            (false, true) => Quadrant.SOUTH_WEST,
            (false, false) => Quadrant.SOUTH_EAST
        };
    }

    public bool Intersect(RectangularBoundary<TX, TY> other) {
        ArgumentNullException.ThrowIfNull(other);
        
        bool overlapX =
            this._minX.CompareTo(other._maxX) <= 0 &&
            this._maxX.CompareTo(other._minX) >= 0;

        bool overlapY =
            this._minY.CompareTo(other._maxY) <= 0 &&
            this._maxY.CompareTo(other._minY) >= 0;

        return overlapX && overlapY;
    }

    public bool HasSameBounds(RectangularBoundary<TX, TY> other) {
        ArgumentNullException.ThrowIfNull(other);

        return
            this._minY.CompareTo(other._minY) == 0 &&
            this._maxY.CompareTo(other._maxY) == 0 &&
            this._minX.CompareTo(other._minX) == 0 &&
            this._maxX.CompareTo(other._maxX) == 0;
    }


    public bool CanSplit() {
        bool minYValid = this._minY.CompareTo(this._midY) < 0;
        bool maxYValid = this._midY.CompareTo(this._maxY) < 0;
        bool minXValid = this._minX.CompareTo(this._midX) < 0;
        bool maxXValid = this._midX.CompareTo(this._maxX) < 0;
        
        return minYValid && maxYValid && minXValid && maxXValid;
    }
}
     