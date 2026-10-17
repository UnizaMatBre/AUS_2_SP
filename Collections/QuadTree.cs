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
}



public interface ISpatial {
	Position GetPosition();
}



public class QuadTree<TValue> : IBinarySearchTree<TValue> where TValue : ISpatial {
	private class QuadTreeNode {
		
		
	}


	public bool Insert(TValue valueToInsert) {
		
		throw new NotImplementedException();
	}

	public bool TryGetValue(TValue valueToFind, out TValue? output) {
		
		throw new NotImplementedException();
	}
}