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
}



public class RectangularBoundary<TX, TY> {
    
    private readonly IBoundary<TY> _north;
    private readonly IBoundary<TY> _south;
    
    private readonly IBoundary<TX> _east;
    private readonly IBoundary<TX> _west;
    
    
    public RectangularBoundary(IBoundary<TY> north, IBoundary<TY> south, IBoundary<TX> east, IBoundary<TX> west) {
        ArgumentNullException.ThrowIfNull(north);
        ArgumentNullException.ThrowIfNull(south);
        ArgumentNullException.ThrowIfNull(east);
        ArgumentNullException.ThrowIfNull(west);
        
        this._north = north;
        this._south = south;

        this._east = east;
        this._west = west;
    }

    
}
     