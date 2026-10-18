namespace Collections;

public interface IDimensionalComparable<in T> {
	
	public static abstract uint GetDimensionCount();
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
	private int _Compare(uint layer, TValue a, TValue b) {
		
		return a.DimensionalCompareTo(layer % TValue.GetDimensionCount(), b);
	}

	private bool _AreAllDimensionsEqual(TValue a, TValue b) {
		for (uint dimension = 0; dimension < TValue.GetDimensionCount(); dimension++) {
			if (this._Compare(dimension, a, b) != 0) {
				return false;
			}
		}
		
		return true;
	}
	
	public bool Insert(TValue valueToInsert) {
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

	/// <summary>
	/// Tests if this tree still follows rules of KdTree - for debugging purposes only
	/// TODO: Currently the method if flawed - fix it
	/// </summary>
	/// <returns>True if the tree is a valid KdTree, false otherwise</returns>
	public bool Debug_IsKdTree() {
		bool CheckNode(KdTreeNode? node, uint layer) {
			// no node, returns instantly with true
			if (node is null) {
				return true;
			}
		
			// left subtree must be smaller or equal than node
			if(node.Left is not null && this._Compare(layer, node.Left.Value, node.Value) > 0) {
				return false;
			}

			// right subtree must be larger than node
			if (node.Right is not null && this._Compare(layer, node.Right.Value, node.Value) <= 0) {
				return false;
			}
			
			// perform same check on children
			var leftOk = CheckNode(node.Left, layer + 1);
			var rightOk = CheckNode(node.Right, layer + 1);
			
			return leftOk && rightOk;
		}


		return CheckNode(this._root, 0);
	}

}