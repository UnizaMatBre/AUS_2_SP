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
		
		private List<TValue> _items;
		
		public QuadTreeNode(RectangularBoundary<TValue, TValue> myBoundary) {
			
			this._myBoundary = myBoundary;
			this._items = new List<TValue>();
		}

		public bool IsLeaf() {
			return 
				this.SouthWest == null && 
				this.SouthEast == null && 
				this.NorthEast == null && 
				this.NorthWest == null;
		}

		public bool Insert(TValue value) {
			if (this.IsLeaf() == false) {
				return false;
			}

			if (this._items.Any()) {
				return false;
			}
			
			this._items.Add(value);
			return true;
		}
	}
	
	

	private readonly RectangularBoundary<TValue, TValue> _outerBoundary;


	private QuadTreeNode? _root;
	
	public QuadTree(RectangularBoundary<TValue, TValue> outerBoundary) {
		
		this._outerBoundary = outerBoundary;
		this._root = null;
	}
	
	
	public bool Insert(TValue valueToInsert) {
		ArgumentNullException.ThrowIfNull(valueToInsert, nameof(valueToInsert));
		
		if(this._outerBoundary.Contains(valueToInsert, valueToInsert) == false){
			return false;
		}

		if (this._root == null) {
			this._root = new QuadTreeNode(this._outerBoundary);
			return true;
		}
		
		throw new NotImplementedException();
	}
	
	public IEnumerable<TValue> EnumerateRange(TValue upperLeft, TValue lowerRight) {
		
		throw new NotImplementedException();
	}
}