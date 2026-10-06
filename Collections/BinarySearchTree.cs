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
	
	
	public virtual bool Remove(TData data) {
		Node? current = this._root;

		// traverse tree
		while (current != null) {
			int cmpResult = this._comparer.Compare(data, current.Data);

			// we found data for removal
			if (cmpResult == 0) {
				break;
			}
            
			current = (cmpResult < 0) ? ref current.Left : ref current.Right;
		}
        
		// branch doesn't exist => data we want to remove doesn't exist
		if (current == null) {
			return false;
		}

		// handle 2 children
		if (current.Left != null && current.Right != null) {
			// get inorder successor
			Node successor = current.InOrderNext()!;
            
            
			// "swap" successor with node for removal
			successor.Parent!.ReplaceChildren(successor, successor.Right);
			current.Data = successor.Data;
			
                        
			return true;
		}
        
		// handle 1 or 0 children
		Node? child = current.Left ?? current.Right;

		if (current.Parent == null) {
			this._root = child;
			if (child != null) {
				child.Parent = null;
			}
		}
		else {
			current.Parent.ReplaceChildren(current, child);
		}
        
		return true;
	}
	
	
	public virtual bool TryGetValue(TData data, out TData? output) {
        
		Node? current = this._root;

		while (current != null) {
            
			int cmpCurrent = this._comparer.Compare(data, current.Data);

			if (cmpCurrent == 0) {
				output = current.Data;
				return true;
			}


			current = (cmpCurrent < 0) ? current.Left : current.Right;
		}
        
		output = default;
		return false;
	}
}