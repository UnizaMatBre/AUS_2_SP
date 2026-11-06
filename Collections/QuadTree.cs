namespace Collections;

public interface IQuadTreeValueStorage<TValue> {
    public int Count();
    
    public bool Insert(TValue value);

    public bool Remove(TValue value);
    
    public IEnumerable<TValue> EnumerateAll();
}


public class QuadTree<TValue, TValueStorage> where TValueStorage : IQuadTreeValueStorage<TValue>, new() {
	
    private static readonly Quadrant[] TRAVERSAL_ORDER = [
        Quadrant.NORTH_WEST,
        Quadrant.NORTH_EAST,
        Quadrant.SOUTH_EAST,
        Quadrant.SOUTH_WEST
    ];
    
    private class QuadTreeNode {
		private readonly uint _depth;
		
		
		public readonly RectangularBoundary<TValue, TValue> Boundary;
		public QuadTreeNode? NorthWest;
		public QuadTreeNode? NorthEast;
		public QuadTreeNode? SouthEast;
		public QuadTreeNode? SouthWest;

		private bool _isLeaf;
		
		private readonly Func<TValueStorage> _storageFactory;
		private TValueStorage? _items;
		
		public QuadTreeNode(RectangularBoundary<TValue, TValue> myBoundary, uint depth) {
			
			this.Boundary = myBoundary;
			this._depth = depth;
			this._isLeaf = true;
			this._items = new TValueStorage();
		}
		
		 
		
		
		public bool IsLeaf() {
			return this._isLeaf;
		}

		public bool Insert(TValue value, uint maxDepth) {
			if (this.IsLeaf() == false) {
				return false;
			}

			if (this._items!.Count() == 0 || this._depth >= maxDepth || this.Boundary.CanSplit() == false) {
				this._items.Insert(value);
				return true;
			}
			

			return false;
		}

		public void Split() {
			if (this.IsLeaf() == false || this.Boundary.CanSplit() == false) {
				throw new InvalidOperationException("Only leaf can be split.");
			}

			QuadTreeNode MakeChild(Quadrant quadrant) {
				var childrenBoundary = this.Boundary.SubBoundary(quadrant);
				
				return new QuadTreeNode(childrenBoundary, this._depth + 1);
			}
			
			this.NorthWest = MakeChild(Quadrant.NORTH_WEST);
			this.NorthEast = MakeChild(Quadrant.NORTH_EAST);
			this.SouthEast = MakeChild(Quadrant.SOUTH_EAST);
			this.SouthWest = MakeChild(Quadrant.SOUTH_WEST);


			foreach (var oneItem in this._items!.EnumerateAll()) {
				var itemsQuadrant = this.Boundary.LocateQuadrantPosition(oneItem, oneItem);
				
				this.GetChildAt(itemsQuadrant)!._items!.Insert(oneItem);
			}

			this._items = default(TValueStorage);
			this._isLeaf = false;
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
				this.Boundary.LocateQuadrantPosition(value, value)
			);
		}

		public TValueStorage GetItems() {

			return this._items!;
		}
			

		public RectangularBoundary<TValue, TValue> GetBoundary() {

			return this.Boundary;
		}

		public uint GetDepth() {
			
			return this._depth;
		}
	}

	private readonly RectangularBoundary<TValue, TValue> _outerBoundary;
	private readonly uint _maxDepth;

	private QuadTreeNode? _root;

    
    
	public QuadTree(
		RectangularBoundary<TValue, TValue> outerBoundary,
		uint maxDepth = 20
	) {
		if (maxDepth == 0) {
			throw new ArgumentException("Maximum depth cannot be zero", nameof(maxDepth));
		}

		this._outerBoundary = outerBoundary;
		this._maxDepth = maxDepth;

		this._root = null;
	}
	
	
	public bool Insert(TValue valueToInsert) {
		ArgumentNullException.ThrowIfNull(valueToInsert, nameof(valueToInsert));
		
		if(this._outerBoundary.Contains(valueToInsert, valueToInsert) == false){
			return false;
		}

		if (this._root == null) {
			this._root = new QuadTreeNode(this._outerBoundary, 0);
		}
		
		QuadTreeNode? currentNode = this._root;
		while (currentNode.Insert(valueToInsert, this._maxDepth) == false) {
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
	
	
	public IEnumerable<TValue> EnumerateBoundary(RectangularBoundary<TValue, TValue> searchBoundary) {
		Stack<QuadTreeNode> pendingNodes = new();
		if (this._root is not null) {
			pendingNodes.Push(this._root);
		}

		while (pendingNodes.Count != 0) {
			var currentNode = pendingNodes.Pop();

			// skip nodes that don't intersect in any way
			if (currentNode.Boundary.Intersect(searchBoundary) == false) {
				continue;
			}

			if (currentNode.IsLeaf()) {
				// NOTE: Fix null
				foreach (var oneItem in currentNode.GetItems().EnumerateAll()) {
					if (searchBoundary.Contains(oneItem, oneItem)) {
						yield return oneItem;
					}
				}
			}
			else {
				foreach (var oneQuadrant in TRAVERSAL_ORDER) {
					pendingNodes.Push(currentNode.GetChildAt(oneQuadrant)!);	
				}
			}
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
		
		Queue<QuadTreeNode> pendingNodes = new();
		pendingNodes.Enqueue(this._root);
		
		using (var writer = new StreamWriter(fileName)) {
			
			while(pendingNodes.Count > 0) {
				var currentNode = pendingNodes.Dequeue();
				
				foreach (var oneValue in currentNode.GetItems().EnumerateAll()) {
						
				}
				
				
				foreach (var currentQuadrant in TRAVERSAL_ORDER) {
					var oneChild = currentNode.GetChildAt(currentQuadrant);
					
					if(oneChild is not null) {
						pendingNodes.Enqueue(oneChild);
					}
				}
			}
			
		}
	}
	
	public bool Debug_IsQuadTree() {

		HashSet<QuadTreeNode> visited = new();

		bool CheckNode(QuadTreeNode node, RectangularBoundary<TValue, TValue> expectedBoundary, uint expectedDepth) {
			// each node can be visited only once
			if (visited.Add(node) == false) {
				Console.WriteLine("Fail: node already visited");
				return false;
			}

			// node must have correct depth
			if (node.GetDepth() != expectedDepth) {
				Console.WriteLine("Fail: node has the wrong depth");
				return false;
			}

			// node cannot be deeper than maximum
			if (node.GetDepth() > this._maxDepth) {
				Console.WriteLine("Fail: node is deeper than maximum");
				return false;
			}

			// node must have expected boundary
			if (node.GetBoundary().HasSameBounds(expectedBoundary) == false) {
				Console.WriteLine("Fail: node has wrong boundary.");
				return false;
			}
			
			// count kids
			var childCount = 0;
			foreach (var quadrant in TRAVERSAL_ORDER) {
				if (node.GetChildAt(quadrant) is not null) {
					childCount++;
				}
			}

			// no children means it is leaf
			if (childCount == 0) {
				// if leaf is not at max depth, it cannot have more items than one
				if (node.GetDepth() < this._maxDepth && node.GetItems().Count() > 1) {
					Console.WriteLine("Fail: Leaf that is not on bottom can have at most 1 member");
					return false;
				}

				
				// all items must be inside of node's boundaries
				foreach (var item in node.GetItems().EnumerateAll()) {
					if (node.GetBoundary().Contains(item, item) == false) {
						Console.WriteLine("Fail: Item must be inside bounds of node");
						return false;
					}
				}

				return true;
			}

			// non-leaf node must have exactly 4 children
			if (childCount != TRAVERSAL_ORDER.Length) {
				Console.WriteLine("Fail: Node must have either 0 or 4 children");
				return false;
			}
			
			// check all children
			foreach (var quadrant in TRAVERSAL_ORDER) {
				var child = node.GetChildAt(quadrant);


				// check kid using recursion
				if (CheckNode(child!, node.GetBoundary().SubBoundary(quadrant), expectedDepth + 1) == false) {

					return false;
				}
			}

			return true;
		}

		if (this._root is null) {
			
			return true;
		}

		return CheckNode(this._root, this._outerBoundary, 0);
	}
	
}