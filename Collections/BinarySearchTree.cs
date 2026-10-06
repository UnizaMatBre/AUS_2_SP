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
		
		public void ReplaceChildren(Node oldChild, Node? newChild) {
			void AssignInto(ref Node? childBranch) {
				childBranch = newChild;

				if (newChild is not null) {
					newChild.Parent = this;
				}
			}

			if (this.Left == oldChild) {
				AssignInto(ref this.Left);
				return;
			}

			if (this.Right == oldChild) {
				AssignInto(ref this.Right);
				return;
			}
            
			throw new InvalidOperationException($"Node doesn't have child {oldChild}");
		}
		
		
		public Node? InOrderNext() {
			Node? current = null;
            
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
			Node? parent = current.Parent;
            
			// travel through ancestors until we find one that has us as left child
			while (parent != null && current != parent.Left) {
				current = parent;
				parent = parent.Parent;
			}

			return parent;
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