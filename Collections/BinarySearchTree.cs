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

}