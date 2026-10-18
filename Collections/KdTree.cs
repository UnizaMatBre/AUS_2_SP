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

		/// <summary>
		/// Returns the next node in an in-order traversal of the tree.
		/// </summary>
		/// <returns>The next node in the traversal, or null if there is no such node.</returns>
		public KdTreeNode? InOrderNext() {
			KdTreeNode? current = null;

			// has right subtree?
			// - Successor is leftmost node in it
			if (this.Right != null) {
				current = this.Right;
        
				// get leftmost node
				while (current.Left != null) {
					current = current.Left;
				}

				return current;
			}
    
			// doesn't have right subtree?
			// - Successor is first ancestor where this is in left subtree
			current = this;
			KdTreeNode? parent = current.Parent;
            
			// travel through ancestors until we find one that has us as left child
			while (parent != null && current != parent.Left) {
				current = parent;
				parent = parent.Parent;
			}

			return parent;
		}
	}
	

	private KdTreeNode? _root;

	public KdTree() {
		this._root = null;
	}


	/// This method acts as replacement for calling comparator
	private int _Compare(uint dimension, TValue a, TValue b) {

		return a.DimensionalCompareTo(dimension % a.GetDimensionCount(), b);
	}

	private bool _AreAllDimensionsEqual(TValue a, TValue b) {
		uint dimensionCount = a.GetDimensionCount();
		
		for (uint i = 0; i < dimensionCount; i++) {
			if (this._Compare(i, a, b) != 0) {
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
		while (true)
		{
			int cmpResult = this._Compare(layer, current.Value, valueToInsert); //current.Value.DimensionalCompareTo(layer % dimensionCount, valueToInsert);
			
			// handle duplicate
			if (cmpResult == 0) {
				if (this._AreAllDimensionsEqual(valueToInsert, current.Value)) {
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

			int cmpResult = this._Compare(layer, current.Value, valueToFind);
			

			if (cmpResult == 0) {
				if (this._AreAllDimensionsEqual(valueToFind, current.Value)) {
					output = current.Value;
					return true;
				}
			}


			current = (cmpResult <= 0) ? current.Left : current.Right;
			layer++;
		}
        
		output = default;
		return false;
	}
	
	
	public IEnumerable<TValue> EnumerateRange(TValue lowerBound, TValue upperBound) {

		
		throw new NotImplementedException();
	}
}