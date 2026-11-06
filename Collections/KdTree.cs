namespace Collections;




public class KdTree<TValue> {

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

	private readonly IDimensionalComparer<TValue> _dimensionalComparer;
	private int _itemCount;
	private KdTreeNode? _root;
	
	
	public KdTree(IDimensionalComparer<TValue> dimensionalComparer) {
		if (dimensionalComparer.GetDimensionCount() == 0) {
			
			throw new ArgumentException("Comparer with 0 dimensions is not allowed");
		}
		
		this._dimensionalComparer = dimensionalComparer;
		this._itemCount = 0;
		this._root = null;
	}

	public IDimensionalComparer<TValue> GetDimensionalComparer() {
		
		return this._dimensionalComparer;
	}
	
	
	public bool Insert(TValue valueToInsert) {
		ArgumentNullException.ThrowIfNull(valueToInsert);
		
		// is tree empty? Create node as root
		if (this._root is null) {
			this._root = new KdTreeNode(valueToInsert, null);
			
			this._itemCount++;
			return true;
		}
		
		// initialize variables
		uint layer = 0;
		KdTreeNode current = this._root;


		// traverse tree
		while (true) {
			uint dimension = layer % this._dimensionalComparer.GetDimensionCount();
			
			// -1 => new value is larger than current node - goes to right subtree
			// +1 => new value is smaller than current node - goes to left subtree
			//  0 => new value is possibly equal to current node - if not, goes to left subtree
			int cmpResult = this._dimensionalComparer.DimensionalCompare(dimension, current.Value, valueToInsert);
			
			// pick proper branch
			// +1 or 0 => left (larger) subtree
			// -1 => right (larger) subtree
			ref KdTreeNode? branch = ref (cmpResult > 0 ? ref current.Left : ref current.Right);

			// branch doesn't exist, we will insert into it
			if (branch is null) {
				KdTreeNode newNode = new KdTreeNode(valueToInsert, current);
				branch = newNode;
                
				this._itemCount++;
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

	public int Count() => this._itemCount;
	
	/// <summary>
	/// Returns values that fit into inclusive range between lower bound and upper bound.
	/// </summary>
	/// <param name="lowerBound">lower bound of range</param>
	/// <param name="upperBound">upper bound of range</param>
	/// <returns>IEnumerable containing selected values</returns>
	public IEnumerable<TValue> EnumerateRange(TValue lowerBound, TValue upperBound) {
		Stack<(KdTreeNode, uint)> pendingNodesWithLayer = new();
		if(this._root is not null) {
			pendingNodesWithLayer.Push((this._root, 0));
		}
		
		while(pendingNodesWithLayer.Count > 0) {
			var (currentNode, layer) = pendingNodesWithLayer.Pop();
			
			// check if current node is in range
			if(this._dimensionalComparer.InclusiveBetween(currentNode.Value, lowerBound, upperBound)) {
				yield return currentNode.Value;
			}
			
			uint dimension = layer % this._dimensionalComparer.GetDimensionCount();
			
			// check left subtree
			if(currentNode.Left is not null && this._dimensionalComparer.DimensionalCompare(dimension, currentNode.Value, lowerBound) >= 0) {
				pendingNodesWithLayer.Push((currentNode.Left, layer + 1));
			}
			
			// check right subtree
			if(currentNode.Right is not null && this._dimensionalComparer.DimensionalCompare(dimension, currentNode.Value, upperBound) < 0) {
				pendingNodesWithLayer.Push((currentNode.Right, layer + 1));
			}
		}
		
	}

	public IEnumerable<TValue> EnumerateAll() {
		KdTreeNode? currentNode = this._root;
		
		// find minimum
		while(currentNode?.Left is not null) {
			currentNode = currentNode.Left;
		}
		
		// use inorder to travel entire tree
		while(currentNode is not null) {
			yield return currentNode.Value;
			currentNode = currentNode.InOrderNext();
		}
	}


	/// <summary>
	/// Loads values from CSV file and inserts them into the tree.
	/// </summary>
	/// <param name="fileName">Path to the CSV file</param>
	/// <param name="fromCsvString">Function to convert CSV line into TValue</param>
	public void LoadCsv(string fileName, Func<string, TValue> fromCsvString) {
		using (var reader = new StreamReader(fileName)) {
			
			string? line;
			while((line = reader.ReadLine()) is not null) {
				this.Insert(fromCsvString(line));
			}
		}
	}

	/// <summary>
	/// Saves all values from the tree to a CSV file.
	/// </summary>
	/// <param name="fileName">Path to the CSV file</param>
	/// <param name="toCsvString">Function to convert TValue into CSV line</param>
	public void DumbCsv(string fileName, Func<TValue, string> toCsvString) {
		if(this._root is null) {
			return;
		}
		
		Queue<KdTreeNode> pendingNodes = new();
		pendingNodes.Enqueue(this._root);
		
		using (var writer = new StreamWriter(fileName)) {
			
			while(pendingNodes.Count > 0) {
				var currentNode = pendingNodes.Dequeue();
				
				writer.WriteLine(toCsvString(currentNode.Value));
				
				if(currentNode.Left is not null) {
					pendingNodes.Enqueue(currentNode.Left);
				}
				
				if(currentNode.Right is not null) {
					pendingNodes.Enqueue(currentNode.Right);
				}
			}
			
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
			
			uint dimension = layer % this._dimensionalComparer.GetDimensionCount();

			// in left subtree, the node must be smaller or equal as maximum [ comp(node, max) = -1 or 0 ]
			// in right subtree, the node must be larger than minimum [ comp(node, min) = +1 ] 
			
			if (minNodes[dimension] is not null && this._dimensionalComparer.DimensionalCompare(dimension, node.Value, minNodes[dimension]!.Value) < 0) {
				return false;
			}

			if(maxNodes[dimension] is not null && this._dimensionalComparer.DimensionalCompare(dimension, node.Value, maxNodes[dimension]!.Value) >= 0) {
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

		KdTreeNode?[] maxNodes = new KdTreeNode?[this._dimensionalComparer.GetDimensionCount()];
		KdTreeNode?[] minNodes = new KdTreeNode?[this._dimensionalComparer.GetDimensionCount()];
		
		for(int index = 0; index < this._dimensionalComparer.GetDimensionCount(); index++) {
			maxNodes[index] = null;
			minNodes[index] = null;
		}
		

		return CheckNode(this._root, 0, minNodes, maxNodes);
	}

}