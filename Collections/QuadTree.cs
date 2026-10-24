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



public class QuadTree<TValue> {
	

	public bool Insert(TValue valueToInsert) {
		
		throw new NotImplementedException();
	}
	
	public IEnumerable<TValue> EnumerateRange(TValue upperLeft, TValue lowerRight) {
		
		throw new NotImplementedException();
	}
}