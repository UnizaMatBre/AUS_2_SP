namespace Collections;

/// <summary>
/// Represents ability to compare two object on multiple different dimensions.
/// </summary>
/// <typeparam name="T">Type of object to dimensionaly compare.</typeparam>
public interface IDimensionalComparer<in T> {

    /// <summary>
    /// Returns number of dimensions on which this comparer is operating. Must be larger than 0.
    /// </summary>
    /// <returns>Number of dimensions</returns>
    public uint GetDimensionCount();

    /// <summary>
    /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other in specified dimension.
    /// </summary>
    /// <param name="dimension">The dimension in which comparision happens</param>
    /// <param name="a">The first object to compare.</param>
    /// <param name="b">The second object to compare.</param>
    /// <exception cref="ArgumentOutOfRangeException"><param name="dimension" /> is larger than GetDimensionCount</exception>
    /// <returns>
    ///		<list type="table">
    ///			<item>
    ///				<term>-1</term>
    ///				<description><paramref name="a" /> is less than <paramref name="b" /> in specified <paramref name="dimension"/>.</description>
    ///			</item>
    ///			<item>
    ///				<term>0</term>
    ///				<description><paramref name="a" /> is is equal to <paramref name="b" /> in specified <paramref name="dimension"/>.</description></item>
    ///			<item>
    ///				<term>+1</term>
    ///				<description><paramref name="a" /> is more than <paramref name="b" /> in specified <paramref name="dimension"/>.</description>
    ///			</item>
    ///		</list>
    /// </returns>
    public int DimensionalCompare(uint dimension, T a, T b);

    
    /// <summary>
    /// Checks if value is between two boundaries inclusive in all dimensions.
    /// </summary>
    /// <param name="value">Value whose inclusion in range are we checking</param>
    /// <param name="lower">Lower boundary of range</param>
    /// <param name="upper">Upper boundary of range</param>
    /// <returns>True if value falls into the range in all dimensions, false otherwise</returns>
    public bool InclusiveBetween(T value, T lower, T upper) {
        for (uint dimension = 0; dimension < this.GetDimensionCount(); ++dimension) {
            var lowerBad = this.DimensionalCompare(dimension, value, lower) < 0;
            var upperBad = this.DimensionalCompare(dimension, value, upper) > 0;

            if (lowerBad || upperBad) {
                return false;
            }
            
            
        }

        return true;
        
        
    }
}