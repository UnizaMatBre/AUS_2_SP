namespace Collections;




public class KdTree<TValue> : IBinarySearchTree<TValue> {

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

	private readonly IDimensionalComparer<TValue> _comparer;
	private KdTreeNode? _root;
	
	
	public KdTree(IDimensionalComparer<TValue> comparer) {
		if (comparer.GetDimensionCount() == 0) {
			
			throw new ArgumentException("Comparer with 0 dimensions is not allowed");
		}
		
		this._comparer = comparer;
		this._root = null;
	}

	
	
	public bool Insert(TValue valueToInsert) {
		ArgumentNullException.ThrowIfNull(valueToInsert);
		
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
			uint dimension = layer % this._comparer.GetDimensionCount();
			
			// -1 => new value is larger than current node - goes to right subtree
			// +1 => new value is smaller than current node - goes to left subtree
			//  0 => new value is possibly equal to current node - if not, goes to left subtree
			int cmpResult = this._comparer.DimensionalCompare(dimension, current.Value, valueToInsert);
			
			// check for duplicate
			if (cmpResult == 0) {
				if (this._comparer.AllDimensionsEquals(valueToInsert, current.Value)) {
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

	public bool Remove(TValue valueToRemove) {

		throw new NotImplementedException();
	}
	

	public bool TryGetValue(TValue valueToFind, out TValue? output) {
		
		uint layer = 0;
		KdTreeNode? current = this._root;

		while (current != null) {
			uint dimension = layer % this._comparer.GetDimensionCount();
			
			// -1 => new value is larger than current node - goes to right subtree
			// +1 => new value is smaller than current node - goes to left subtree
			//  0 => new value is possibly equal to current node - if not, goes to left subtree
			int cmpResult = this._comparer.DimensionalCompare(dimension, current.Value, valueToFind);
			
			// check for duplicate
			if (cmpResult == 0) {
				if (this._comparer.AllDimensionsEquals(valueToFind, current.Value)) {
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
	
	/// <summary>
	/// Returns values that fit into inclusive range between lower bound and upper bound. Items will have inorder ordering. 
	/// </summary>
	/// <param name="lowerBound">lower bound of range</param>
	/// <param name="upperBound">upper bound of range</param>
	/// <returns>IEnumerable containing selected values</returns>
	public IEnumerable<TValue> EnumerateRange(TValue lowerBound, TValue upperBound) {
		if (this._root is null) {
			
			yield break;
		}
		
		KdTreeNode? current = this._root;
		KdTreeNode? minimum = null;
		uint layer = 0;
		
		while (current is not null) {
			uint dimension = layer = this._comparer.GetDimensionCount();

			int cmpResult = this._comparer.DimensionalCompare(dimension, lowerBound, current.Value);

			if (cmpResult <= 0) {
				// current is more than lower bound?
				// remember current and go left
				minimum = current;
				current = current.Left;
			}
			else {
				// current is less than lower bound?
				// we went too far - go right
				current = current.Right;
			}

			++layer;
		}
		
		
		// from minimum, do inorder traveral until we pass upper bound
		current = minimum;
		while (current is not null) {
			if (this._comparer.InclusiveBetween(current.Value, lowerBound, upperBound)) {
				yield return current.Value;
			}

			current = current.InOrderNext();
		}
	}

	/// <summary>
	/// Tests if this tree still follows rules of KdTree - for debugging purposes only
	/// </summary>
	/// <returns>True if the tree is a valid KdTree, false otherwise</returns>
	public bool Debug_IsKdTree() {
		
		bool CheckNode(KdTreeNode? node, uint layer, KdTreeNode?[] minNodes, KdTreeNode?[] maxNodes) {
			// no node, returns instantly with true
			if (node is null) {
				return true;
			}
			
			uint dimension = layer % this._comparer.GetDimensionCount();

			// in left subtree, the node must be smaller or equal as maximum [ comp(node, max) = -1 or 0 ]
			// in right subtree, the node must be larger than minimum [ comp(node, min) = +1 ] 
			
			if (minNodes[dimension] is not null && this._comparer.DimensionalCompare(dimension, node.Value, minNodes[dimension]!.Value) < 0) {
				return false;
			}

			if(maxNodes[dimension] is not null && this._comparer.DimensionalCompare(dimension, node.Value, maxNodes[dimension]!.Value) >= 0) {
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

		KdTreeNode?[] maxNodes = new KdTreeNode?[this._comparer.GetDimensionCount()];
		KdTreeNode?[] minNodes = new KdTreeNode?[this._comparer.GetDimensionCount()];
		
		for(int index = 0; index < this._comparer.GetDimensionCount(); index++) {
			maxNodes[index] = null;
			minNodes[index] = null;
		}
		

		return CheckNode(this._root, 0, minNodes, maxNodes);
	}

}