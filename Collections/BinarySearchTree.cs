using System.Net.Http.Headers;

namespace Collections;

public class BinarySearchTree<TData> where TData : IComparable<TData> {

	protected class Node {
		public TData Data;

		public Node? Left;
		public Node? Right;
		public Node? Parent;
		
		public Node(TData data, Node? parent) {
			this.Data = data;
			this.Parent = parent;
		}
	}
	
	
	private IComparer<TData> _comparer;
	private Node? _root;
	
	public BinarySearchTree() {
		// TODO: What about just calling other constructor?
		this._comparer = Comparer<TData>.Default;
	}
	
	public BinarySearchTree(IComparer<TData> comparer) {
		
		this._comparer = comparer;
	}
	
	protected virtual Node CreateNode(TData data, Node? parent) {
		
		return new Node(data, parent);
	}


	public virtual bool Add(TData data) {
		// no root node? Make it
		if (this._root is null) {
			this._root = this.CreateNode(data, null);
			
			return true;
		}
		
		
		// initialize variables
		Node current = this._root;


		// traverse tree
		while (true) {
			int cmpResult = this._comparer.Compare(data, current.Data);

			// handle duplicate
			if (cmpResult == 0) {
				return false;
			}
            
			// pick proper branch
			ref Node? branch = ref (cmpResult < 0 ? ref current.Left : ref current.Right);

			// branch doesn't exist, we will insert into it
			if (branch is null) {
				Node newNode = this.CreateNode(data, current);
				branch = newNode;
                
				return true;
			}
            
			// move to next level
			current = branch;
		}
	}
}