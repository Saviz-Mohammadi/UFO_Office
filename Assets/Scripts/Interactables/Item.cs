using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Item : MonoBehaviour, IGrabbable {
    [Header("Dependencies")]
    [Tooltip("The 'Rigidbody' component of the item. Used for physical manipulations.")]
    [SerializeField] private Rigidbody _rigidbody;
    
    [Tooltip("The 'Renderer' component of the item. Used for rendering a ghost replicate.")]
    [SerializeField] private Renderer _renderer;
    
    [Tooltip("The 'GameObject' component of the item. Used for displaying a ghost replicate.")]
    [SerializeField] private GameObject _gameObject;

    [SerializeField] private Collider _collider;
    
    public bool IsGrabbed { get; set; }
    
    public IPlaceableLocation placeableLocationParent { get; set; }

    public Transform GetTransform() {
        return (this.transform);
    }

    public Rigidbody GetRigidbody() {
        return (this._rigidbody);
    }

    public Collider GetCollider() {
        return (_collider);
    }

    public Renderer GetRenderer() {
        return (this._renderer);
    }

    public GameObject GetGhost() {
        return (this._gameObject);
    }
}
