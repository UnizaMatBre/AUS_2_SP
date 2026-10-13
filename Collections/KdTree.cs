namespace Collections;

public interface IDimensionalComparable<in T> {

	public int DimensionalCompareTo(uint dimension, T other);
}

public class KdTree<TValue> where TValue : IDimensionalComparable<TValue> {

	private class KdTreeNode {
		public TValue Value;
		
		public KdTreeNode? Parent;
		public KdTreeNode? Left;
		public KdTreeNode? Right;

		public KdTreeNode(TValue value, KdTreeNode? parent) {
			this.Value = value;
			this.Parent = parent;
		}
	}
	

	private KdTreeNode? _root;

	public KdTree() {
		this._root = null;
	}

	
	public bool Insert(TValue newValue) {
		// is tree empty? Create node as root
		if (this._root is null) {
			this._root = new KdTreeNode(newValue, null);
			
			return true;
		}

		// initialize variables
		uint layer = 0;
		KdTreeNode current = this._root;


		// traverse tree
		while (true) {
			int cmpResult = current.Value.DimensionalCompareTo(layer, newValue);
			
			// handle duplicate
			if (cmpResult == 0) {
				return false;
			}
            
			// pick proper branch
			ref KdTreeNode? branch = ref (cmpResult < 0 ? ref current.Left : ref current.Right);

			// branch doesn't exist, we will insert into it
			if (branch is null) {
				KdTreeNode newNode = new KdTreeNode(newValue, current);
				branch = newNode;
                
				return true;
			}
            
			// move to next level
			current = branch;
			layer++;
		}
	}

	public bool TryGetValue(TValue searchedValue, out TValue? output) {
		uint layer = 0;
		KdTreeNode? current = this._root;

		while (current != null) {

			int cmpCurrent = current.Value.DimensionalCompareTo(layer, searchedValue);

			if (cmpCurrent == 0) {
				output = current.Value;
				return true;
			}


			current = (cmpCurrent < 0) ? current.Left : current.Right;
			layer++;
		}
        
		output = default;
		return false;
	}
}