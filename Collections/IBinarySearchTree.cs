namespace Collections;

public interface IBinarySearchTree<TValue> {

    bool Insert(TValue valueToInsert);

    bool TryGetValue(TValue valueToFind, out TValue? output);
    
    
    // bool Remove(TValue valueToRemove);

}