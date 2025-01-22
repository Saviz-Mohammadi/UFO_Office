using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GrabController : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("The selected 'Transform' placed in this field will be used as origin detection point.")]
    [SerializeField] private Transform _transform = null;
    
    [SerializeField] private Transform _grabPoint = null;
    
    // Used to store the most recent grabbable object:
    private IGrabbable _foundGrabbable = null;
    private IGrabbable _grabbedGrabbable = null;
    private IContainer container;


    [Header("Fields (Customizable)")]
    [Tooltip("Controls the 'Enabled' state of the Gizmos.")]
    [SerializeField] private bool _gizmosIsEnabled = true;
    
    [Tooltip("Controls the color of the drawn Gizmos.")]
    [SerializeField] private Color _gizmosColor = Color.green;
    
    [Space(10)]
    [Tooltip("Controls the range within which the player can detect and interact with an interactable object. (Also used for drawing the Gizmos)")]
    [SerializeField] private float _interactionRange = 5.0f;

    
    
    [Space(10)]
    // Event is fired when this script discovers an interactable object: 
    public UnityEvent<GrabController> OnInteractableDiscovered;
    
    // Event is fired when the last interactable object was either replaced with a new one or just lost:
    public UnityEvent<GrabController> OnInteractableDiscarded;
    
    
    
    private void Update() {
        
        // Continuously scan the environment for new interactable objects:
        _foundGrabbable = ObtainGrabbableObject();
        
        // Invoke Unity events and allow other components to know what happened:
        if (_foundGrabbable != null) {
            
            OnInteractableDiscovered?.Invoke(this);
        }

        else {
            OnInteractableDiscarded?.Invoke(this);
        }
    }
    
    private void OnDrawGizmos() {

        if (!_gizmosIsEnabled) {
            return;
        }
        
        Gizmos.color = _gizmosColor;
        
        Gizmos.DrawRay(_transform.position, _transform.forward * _interactionRange);
    }
    
    // Called on 'Grabbed' Unity-Event of 'Player Input' component:
    public void OnGrabbed(InputAction.CallbackContext context) {

        // Todo: The functionality works, but for some reason if there is no object with in range the context throws error. Maybe this has something to do with input sysetm.
        
        if (context.performed) {

            if (_grabbedGrabbable == null) {
                _grabbedGrabbable = ObtainGrabbableObject();
                _grabbedGrabbable.Grab(_grabPoint);
            }

            else {
                _grabbedGrabbable.Drop(transform);
                _grabbedGrabbable = null;
                container = null;
            }
        }
    }
    
    public IGrabbable GetInteractable() {
     
        return (_foundGrabbable);
    }
    
    private IGrabbable ObtainGrabbableObject() {
        
        bool success = Physics.Raycast(_transform.position, _transform.forward, out RaycastHit hit, _interactionRange);

        if (!success) {
            return (null);
        }
        
        hit.collider.TryGetComponent(out IGrabbable interactable);
            
        if (interactable == null) {
            return (null);
        }
            
        return (interactable);
    }
    
    private IContainer ObtainContainerObject() {
        
        bool success = Physics.Raycast(_transform.position, _transform.forward, out RaycastHit hit, _interactionRange);

        if (!success) {
            return (null);
        }
        
        hit.collider.TryGetComponent(out IContainer interactable);
            
        if (interactable == null) {
            return (null);
        }
            
        return (interactable);
    }
}
