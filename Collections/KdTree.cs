namespace Collections;

public interface IDimensionalComparable<in T> {

	public int DimensionalCompareTo(uint dimension, T other);
}

public class KdTree<TValue> where TValue : IDimensionalComparable<TValue> {

	private class KdTreeNode {
		private TValue Value;
		
		private KdTreeNode? Parent;
		private KdTreeNode? Left;
		private KdTreeNode? Right;

		public KdTreeNode(TValue value, KdTreeNode? parent) {
			this.Value = value;
			this.Parent = parent;
		}
	}
}