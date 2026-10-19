namespace Collections;

public readonly struct Position {
	private readonly double _x;
	private readonly double _y;

	public Position(double x, double y)
	{
		this._x = x;
		this._y = y;
	}
	
	public double GetX() => this._x;
	public double GetY() => this._y;
	
	
	public class DimensionalComparer : IDimensionalComparer<Position> {
		public uint GetDimensionCount() => 2;

		public int DimensionalCompare(uint dimension, Position a, Position b) {
			return dimension switch {
				0 => a._x.CompareTo(b._x),
				1 => a._y.CompareTo(b._y),
				_ => throw new ArgumentOutOfRangeException(nameof(dimension))
			};
		}
	}
}



public interface ISpatial {
	Position GetPosition();
}



public class QuadTree<TValue> : IBinarySearchTree<TValue> where TValue : ISpatial {
	private class QuadTreeNode {
		public readonly Position UpperLeftCorner;
		public readonly Position LowerRightCorner;

		public QuadTreeNode? UpperLeftChild;
		public QuadTreeNode? UpperRightChild;
		public QuadTreeNode? LowerLeftChild;
		public QuadTreeNode? LowerRightChild;
		
		public TValue? Value;

		public QuadTreeNode(TValue value, Position upperLeft, Position lowerRight) {
			this.Value = value;
			this.UpperLeftCorner = upperLeft;
			this.LowerRightCorner = lowerRight;
		}
	}

	private readonly IDimensionalComparer<Position> _comparer = new Position.DimensionalComparer();
	private QuadTreeNode? _root;


	public QuadTree(Position upperLeft, Position lowerRight) {
		this._root = new QuadTreeNode(default!, upperLeft, lowerRight);
	}

	public bool Insert(TValue valueToInsert) {
		
		throw new NotImplementedException();
	}

	public bool TryGetValue(TValue valueToFind, out TValue? output) {
		
		throw new NotImplementedException();
	}

	public IEnumerable<TValue> EnumerateRange(TValue upperLeft, TValue lowerRight) {
		
		throw new NotImplementedException();
	}
}