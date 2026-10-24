using System.Runtime.Intrinsics.X86;

namespace Collections;


enum Quadrant {

	// x =< 0; y =< 0
	NORTH_WEST,
	
	// x > 0; y =< 0 
	NORTH_EAST,
	
	// x > 0; y > 0
	SOUTH_EAST,
	
	// x =< 0; y > 0
	SOUTH_WEST
}

public interface IBoundary<TImplementer, in TValue>
	where TImplementer : IBoundary<TImplementer, TValue>
	where TValue : IComparable<TValue>
{

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
	public TImplementer Midpoint(TImplementer other);
}



public class QuadTree<TValue> {
	

	public bool Insert(TValue valueToInsert) {
		
		throw new NotImplementedException();
	}
	
	public IEnumerable<TValue> EnumerateRange(TValue upperLeft, TValue lowerRight) {
		
		throw new NotImplementedException();
	}
}