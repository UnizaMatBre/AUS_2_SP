namespace Collections;

public interface ISpatial {

	(int x, int y) GetPosition();
}



public class QuadTree<TValue> : IBinarySearchTree<TValue> where TValue : ISpatial {
	private class QuadTreeNode {
		
		
	}


	public bool Insert(TValue valueToInsert) {
		
		throw new NotImplementedException();
	}

	public bool TryGetValue(TValue valueToFind, out TValue? output) {
		
		throw new NotImplementedException();
	}
}