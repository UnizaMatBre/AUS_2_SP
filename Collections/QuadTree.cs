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

	private class QuadTreeNode {
		private readonly RectangularBoundary<TValue, TValue> _myBoundary;

		public QuadTreeNode? NorthWest;
		public QuadTreeNode? NorthEast;
		public QuadTreeNode? SouthEast;
		public QuadTreeNode? SouthWest;
		
		public QuadTreeNode(RectangularBoundary<TValue, TValue> myBoundary) {
			
			this._myBoundary = myBoundary;
		}
		
	}
	
	

	private readonly RectangularBoundary<TValue, TValue> _outerBoundary;


	private QuadTreeNode? _root;
	
	public QuadTree(RectangularBoundary<TValue, TValue> outerBoundary) {
		
		this._outerBoundary = outerBoundary;
		this._root = null;
	}
	
	
	public bool Insert(TValue valueToInsert) {
		
		throw new NotImplementedException();
	}
	
	public IEnumerable<TValue> EnumerateRange(TValue upperLeft, TValue lowerRight) {
		
		throw new NotImplementedException();
	}
}