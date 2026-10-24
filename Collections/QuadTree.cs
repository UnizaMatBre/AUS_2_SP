using System.Runtime.Intrinsics.X86;

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


	public Position GetMiddle(ref Position other) {
		return new Position(
			(this.GetX() + other.GetX()) / 2,
			(this.GetY() + other.GetY()) / 2
		);
	}

	public (int x, int y) GetDirectionTo(Position target) {
		return (
			x: target.GetX() <= this.GetX() ? 0 : 1,
			y: target.GetY() <= this.GetY() ? 0 : 1
		);
	}
	
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



public class QuadTree<TValue> where TValue : ISpatial {
	private class QuadTreeNode {
		public readonly Position UpperLeftCorner;
		public readonly Position LowerRightCorner;
		public readonly Position Middle;

		public QuadTreeNode? UpperLeftChild;
		public QuadTreeNode? UpperRightChild;
		public QuadTreeNode? LowerLeftChild;
		public QuadTreeNode? LowerRightChild;

		public Boolean HasValue;
		public TValue? Value;

		public QuadTreeNode(Position upperLeft, Position lowerRight) {
			this.HasValue = false;
			this.UpperLeftCorner = upperLeft;
			this.LowerRightCorner = lowerRight;

			this.Middle = this.UpperLeftCorner.GetMiddle(ref this.LowerRightCorner);
		}
		public ref QuadTreeNode? GetChildRef(int x, int y) {
			switch ((x, y)) {
				case (0, 0): return ref this.UpperLeftChild;
				case (1, 0): return ref this.UpperRightChild;
				case (0, 1): return ref this.LowerLeftChild;
				case (1, 1): return ref this.LowerRightChild;
				default: throw new ArgumentOutOfRangeException(nameof(x));
			};
		}
	}

	private readonly IDimensionalComparer<Position> _comparer = new Position.DimensionalComparer();
	private QuadTreeNode _root;
	
	

	
	/// FEATURE: Tree must EXPLICITLY allow universal key. So no conversion to "Position" type, we need to explicitly allow value type
	public QuadTree(Position upperLeft, Position lowerRight) {
		this._root = new QuadTreeNode(upperLeft, lowerRight);
	}

	public bool Insert(TValue valueToInsert) {
		ArgumentNullException.ThrowIfNull(valueToInsert);

		if (this._root.HasValue == false) {
			this._root.Value = valueToInsert;
			this._root.HasValue = true;

			return true;

		}
		
		var current = this._root;

		while (true) {
			if (current.HasValue) {
				// if value is same to the one we want to insert, it is duplicate
				
				var directionToCurrentVal = current.Middle.GetDirectionTo(current.Value.GetPosition());
				ref var newHomeForCurrent = ref current.GetChildRef(directionToCurrentVal.x, directionToCurrentVal.y);
				
				var upperLeft = new Position(
					directionToCurrentVal.x == 0 ? current.UpperLeftCorner.GetX() : current.Middle.GetX(),
					directionToCurrentVal.y == 0 ? current.UpperLeftCorner.GetY() : current.Middle.GetY()
				);
				var lowerRight = new Position(
					directionToCurrentVal.x == 0 ? current.Middle.GetX() : current.LowerRightCorner.GetX(),
					directionToCurrentVal.y == 0 ? current.Middle.GetY() : current.LowerRightCorner.GetY()
				);

				newHomeForCurrent = new QuadTreeNode(upperLeft, lowerRight);

				current.HasValue = false;
			}
			
			
			
			
			
			var direction = current.Middle.GetDirectionTo(valueToInsert.GetPosition());
			ref QuadTreeNode? child = ref current.GetChildRef(direction.x, direction.y);

			if (child is null) {
				var upperLeft = new Position(
					direction.x == 0 ? current.UpperLeftCorner.GetX() : current.Middle.GetX(),
					direction.y == 0 ? current.UpperLeftCorner.GetY() : current.Middle.GetY()
				);
				var lowerRight = new Position(
					direction.x == 0 ? current.Middle.GetX() : current.LowerRightCorner.GetX(),
					direction.y == 0 ? current.Middle.GetY() : current.LowerRightCorner.GetY()
				);

				child = new QuadTreeNode(upperLeft, lowerRight);
				child.Value = valueToInsert;
				child.HasValue = false;
				return true;
			}

			current = child;
			
		}
	}

	public bool TryGetValue(TValue valueToFind, out TValue? output) {
		
		throw new NotImplementedException();
	}

	public IEnumerable<TValue> EnumerateRange(TValue upperLeft, TValue lowerRight) {
		
		throw new NotImplementedException();
	}
}