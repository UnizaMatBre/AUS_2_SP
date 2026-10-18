namespace Collections;

public interface IDimensionalComparable<in T> where T : IDimensionalComparable<T> {
	
	/// <summary>
	/// Returns number of dimensions type T has
	/// </summary>
	/// <returns>Number of dimensions</returns>
	public uint GetDimensionCount();
	
	// NOTE: This docstring is mostly modified from IComparable<T> interface
	/// <summary>
	/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance is larger, smaller or same in the specified dimension.
	/// </summary>
	/// <param name="dimension">The dimension to compare between objects, in from 0 to <see cref="GetDimensionCount"/> - 1</param>
	/// <param name="other">An object to compare with this instance</param>
	/// <returns>
	///		<list type="table">
	///			<item>
	///				<term>-1</term>
	///				<description> This instance is smaller than <paramref name="other" /> in specified <paramref name="dimension"/>.</description>
	///			</item>
	///			<item>
	///				<term>0</term>
	///				<description> This instance is equal to <paramref name="other" /> in specified <paramref name="dimension"/>.</description></item>
	///			<item>
	///				<term>+1</term>
	///				<description> This instance is larger than <paramref name="other" /> in specified <paramref name="dimension"/>.</description>
	///			</item>
	///		</list>
	/// </returns>
	public int DimensionalCompareTo(uint dimension, T other);


	public bool AllDimensionsEqual(T other) {
		for (uint dimension = 0; dimension < this.GetDimensionCount(); ++dimension) {
			if (this.DimensionalCompareTo(dimension, other) != 0) {
				return false;
			}
		}

		return true;
	}
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


	private readonly uint _allowedDimensionCount;
	
	private KdTreeNode? _root;

	public KdTree() {
		this._allowedDimensionCount = default(TValue).GetDimensionCount();
		
		this._root = null;
	}


	/// This method acts as replacement for calling comparator
	private int _Compare(uint layer, TValue a, TValue b) {
		
		return a.DimensionalCompareTo(layer % this._allowedDimensionCount, b);
	}

	private bool _AreAllDimensionsEqual(TValue a, TValue b) {
		
		return a.AllDimensionsEqual(b);
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
			// -1 => new value is larger than current node - goes to right subtree
			// +1 => new value is smaller than current node - goes to left subtree
			//  0 => new value is possibly equal to current node - if not, goes to left subtree
			int cmpResult = this._Compare(layer, current.Value, valueToInsert);
			
			// check for duplicate
			if (cmpResult == 0) {
				if (this._AreAllDimensionsEqual(valueToInsert, current.Value)) {
					return false;
				}
			}
            
			// pick proper branch
			// +1 or 0 => left (larger) subtree
			// -1 => right (larger) subtree
			ref KdTreeNode? branch = ref (cmpResult > 0 ? ref current.Left : ref current.Right);

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
			// -1 => new value is larger than current node - goes to right subtree
			// +1 => new value is smaller than current node - goes to left subtree
			//  0 => new value is possibly equal to current node - if not, goes to left subtree
			int cmpResult = this._Compare(layer, current.Value, valueToFind);
			
			// check for duplicate
			if (cmpResult == 0) {
				if (this._AreAllDimensionsEqual(valueToFind, current.Value)) {
					output = current.Value;
					return true;
				}
			}


			// pick proper branch
			// +1 or 0 => left (larger) subtree
			// -1 => right (larger) subtree
			current = (cmpResult > 0) ? current.Left : current.Right;
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
		
		bool CheckNode(KdTreeNode? node, uint layer, KdTreeNode?[] minNodes, KdTreeNode?[] maxNodes) {
			// no node, returns instantly with true
			if (node is null) {
				return true;
			}
			
			uint dimension = layer % this._allowedDimensionCount;

			// in left subtree, the node must be smaller or equal as maximum [ comp(node, max) = -1 or 0 ]
			// in right subtree, the node must be larger than minimum [ comp(node, min) = +1 ] 
			
			if (minNodes[dimension] is not null && this._Compare(dimension, node.Value, minNodes[dimension]!.Value) > 0) {
				return false;
			}

			if(maxNodes[dimension] is not null && this._Compare(dimension, node.Value, maxNodes[dimension]!.Value) <= 0) {
				return false;
			}

			// check left subtree (with this node as maximum for this dimension)
			var previousMaxNode = maxNodes[dimension];
			maxNodes[dimension] = node;
			var leftOk = CheckNode(node.Left, layer + 1, minNodes, maxNodes);
			maxNodes[dimension] = previousMaxNode;
			
			
			// check right subtree (with this node as minimum for this dimension)
			var previousMinNode = minNodes[dimension];
			minNodes[dimension] = node;
			var rightOk = CheckNode(node.Right, layer + 1, minNodes, maxNodes);
			minNodes[dimension] = previousMinNode;
			

			
			return leftOk && rightOk;
		}

		KdTreeNode?[] maxNodes = new KdTreeNode?[this._allowedDimensionCount];
		KdTreeNode?[] minNodes = new KdTreeNode?[this._allowedDimensionCount];
		
		for(int index = 0; index < this._allowedDimensionCount; index++) {
			maxNodes[index] = null;
			minNodes[index] = null;
		}
		

		return CheckNode(this._root, 0, minNodes, maxNodes);
	}

}