using System.Drawing;
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



public class QuadTree<TValue> where TValue : IComparable<TValue> {


	private readonly RectangularBoundary<TValue, TValue> _outerBoundary;
	
	public QuadTree(RectangularBoundary<TValue, TValue> outerBoundary) {
		this._outerBoundary = outerBoundary;
	}
	
	
	public bool Insert(TValue valueToInsert) {
		
		throw new NotImplementedException();
	}
	
	public IEnumerable<TValue> EnumerateRange(TValue upperLeft, TValue lowerRight) {
		
		throw new NotImplementedException();
	}
}