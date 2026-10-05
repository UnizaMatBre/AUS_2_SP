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
	
}