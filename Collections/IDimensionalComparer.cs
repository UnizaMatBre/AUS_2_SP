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
    /// Determines whether the specified objects are equal in all dimensions.
    /// </summary>
    /// <param name="a">The first object to compare</param>
    /// <param name="b">The second object to compare.</param>
    /// <returns>True if specified objects are equal in all dimensions, false otherwise</returns>
    public bool AllDimensionsEquals(T a, T b) {
        for (uint dimension = 0; dimension < this.GetDimensionCount(); ++dimension) {
            
            if (this.DimensionalCompare(dimension, a, b) != 0) {
                
                return false;
            }
        }

        return true;
    }
}