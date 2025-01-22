using UnityEngine;

public class Table : MonoBehaviour, IContainer
{
    [SerializeField] private Transform _container;
    
    public Transform GetTransform() {
        return _container;
    }
}
