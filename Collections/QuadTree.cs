using System.Collections.Immutable;
using System.Drawing;
using System.Runtime.Intrinsics.X86;

namespace Collections;






public class QuadTree<TValue> where TValue : IComparable<TValue>
{

	private static readonly Quadrant[] TRAVERSAL_ORDER = [
		Quadrant.NORTH_WEST,
		Quadrant.NORTH_EAST,
		Quadrant.SOUTH_EAST,
		Quadrant.SOUTH_WEST
	];
	
	
	
	private class QuadTreeNode {
		private readonly RectangularBoundary<TValue, TValue> _myBoundary;

		private readonly uint _depth;
		
		public QuadTreeNode? NorthWest;
		public QuadTreeNode? NorthEast;
		public QuadTreeNode? SouthEast;
		public QuadTreeNode? SouthWest;
		
		private List<TValue>? _items;
		
		public QuadTreeNode(RectangularBoundary<TValue, TValue> myBoundary, uint depth) {
			
			this._myBoundary = myBoundary;
			this._depth = depth;
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
			if (this.IsLeaf() == false || this._items.Count > 0) {
				return false;
			}

			this._items.Add(value);
			return true;
		}

		public void Split() {
			if (this.IsLeaf() == false) {
				throw new InvalidOperationException("Only leaf can be split.");
			}
			
			var childrenMap = new Dictionary<Quadrant, QuadTreeNode>();
			foreach (var oneQuadrant in TRAVERSAL_ORDER) {
				var childrenBoundary = this._myBoundary.SubBoundary(oneQuadrant);

				childrenMap[oneQuadrant] = new QuadTreeNode(childrenBoundary, this._depth + 1);
			}
			
			
			
			foreach (var oneItem in this._items) {
				var itemQuadrant = this._myBoundary.LocateQuadrantPosition(oneItem, oneItem);
				
				childrenMap[itemQuadrant]._items.Add(oneItem);
			}
			
			this.NorthWest = childrenMap[Quadrant.NORTH_WEST];
			this.NorthEast = childrenMap[Quadrant.NORTH_EAST];
			this.SouthEast = childrenMap[Quadrant.SOUTH_EAST];
			this.SouthWest = childrenMap[Quadrant.SOUTH_WEST];

			// is this really the best approach?
			this._items = null;
		}

		public QuadTreeNode? GetChildAt(Quadrant quadrant)
		{
			return quadrant switch {
				Quadrant.NORTH_WEST => this.NorthWest,
				Quadrant.NORTH_EAST => this.NorthEast,
				Quadrant.SOUTH_EAST => this.SouthEast,
				Quadrant.SOUTH_WEST => this.SouthWest,
				_ => throw new ArgumentOutOfRangeException(nameof(quadrant), quadrant, null)
			};

		}

		public QuadTreeNode? GetChildFor(TValue value) {
			return this.GetChildAt(
				this._myBoundary.LocateQuadrantPosition(value, value)
			);
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
			this._root = new QuadTreeNode(this._outerBoundary, 0);
			return true;
		}
		
		QuadTreeNode? currentNode = this._root;
		while (currentNode.Insert(valueToInsert) == false) {
			if (currentNode.IsLeaf()) {
				currentNode.Split();
			}
			
			currentNode = currentNode.GetChildFor(valueToInsert);
			if (currentNode is null) {
				throw new InvalidOperationException("Child node should not be null after split");
			}
		}

		return true;
	}
	
	public IEnumerable<TValue> EnumerateRange(TValue upperLeft, TValue lowerRight) {
		
		throw new NotImplementedException();
	}
}