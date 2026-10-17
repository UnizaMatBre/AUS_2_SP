namespace Collections;

public interface IDimensionalComparable<in T> {
	
	public uint GetDimensionCount();
	public int DimensionalCompareTo(uint dimension, T other);
}

public class KdTree<TValue> : IBinarySearchTree<TValue> 
	where TValue : IDimensionalComparable<TValue> {

	private class KdTreeNode {
		public TValue Value;
		
		public KdTreeNode? Parent;
		public KdTreeNode? Left;
		public KdTreeNode? Right;

		public KdTreeNode(TValue value, KdTreeNode? parent) {
			this.Value = value;
			this.Parent = parent;
		}
	}
	

	private KdTreeNode? _root;

	public KdTree() {
		this._root = null;
	}

	
	private bool _AreAllDimensionsEqual(TValue a, TValue b) {
		uint dimensionCount = a.GetDimensionCount();
		
		for (uint i = 0; i < dimensionCount; i++) {
			if (a.DimensionalCompareTo(i, b) != 0) {
				return false;
			}
		}
		
		return true;
	}
	
	public bool Insert(TValue valueToInsert) {
		uint dimensionCount = valueToInsert.GetDimensionCount();
		
		
		// is tree empty? Create node as root
		if (this._root is null) {
			this._root = new KdTreeNode(valueToInsert, null);
			
			return true;
		}

		// initialize variables
		uint layer = 0;
		KdTreeNode current = this._root;


		// traverse tree
		while (true) {
			int cmpResult = current.Value.DimensionalCompareTo(layer % dimensionCount, valueToInsert);
			
			// handle duplicate
			if (cmpResult == 0) {
				if (this._AreAllDimensionsEqual(current.Value, valueToInsert)) {
					return false;
				}
			}
            
			// pick proper branch
			ref KdTreeNode? branch = ref (cmpResult <= 0 ? ref current.Left : ref current.Right);

			// branch doesn't exist, we will insert into it
			if (branch is null) {
				KdTreeNode newNode = new KdTreeNode(valueToInsert, current);
				branch = newNode;
                
				return true;
			}
            
			// move to next level
			current = branch;
			layer++;
		}
	}

	public bool TryGetValue(TValue valueToFind, out TValue? output) {
		uint dimensionCount = valueToFind.GetDimensionCount();
		
		uint layer = 0;
		KdTreeNode? current = this._root;

		while (current != null) {

			int cmpCurrent = current.Value.DimensionalCompareTo(layer % dimensionCount, valueToFind);

			if (cmpCurrent == 0) {
				if(this._AreAllDimensionsEqual(current.Value, valueToFind)) {
					output = current.Value;
					return true;
				}
			}


			current = (cmpCurrent <= 0) ? current.Left : current.Right;
			layer++;
		}
        
		output = default;
		return false;
	}
	
	
	public IEnumerable<TValue> EnumerateRange(TValue lowerBound, TValue upperBound) {

		
		throw new NotImplementedException();
	}
}