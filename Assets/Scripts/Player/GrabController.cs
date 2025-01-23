using System.Drawing;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Color = UnityEngine.Color;

// Note: From my perspective, we are as the controller and player responsible for grabbing and dropping objects and the IGrabble should only enale use to get some details and access to the fields and abilities.
// That is why I decided to have the logic and performances in heer and have the interfaces just act as finders.

public class GrabController : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("The selected 'Transform' placed in this field will be used as origin detection point.")]
    [SerializeField] private Transform _transform = null;
    
    [SerializeField] private Transform _grabPoint = null;
    
    // Used to store the most recent grabbable object:
    private IGrabbable _foundGrabbable = null;
    private IGrabbable _grabbedGrabbable = null;
    private IPlaceableLocation _placeableLocation;
    
    private bool _keyIsDownDrop = false;
    private bool _keyIsDownGrab = false;
    private bool _keyIsDownPlace = false;
    
    private bool _holdGrabbable = false;
    private bool _grabbableFound = false;


    [Header("Fields (Customizable)")]
    [Tooltip("Controls the 'Enabled' state of the Gizmos.")]
    [SerializeField] private bool _gizmosIsEnabled = true;
    
    [Tooltip("Controls the color of the drawn Gizmos.")]
    [SerializeField] private Color _gizmosColor = Color.green;
    
    [Space(10)]
    [Tooltip("Controls the range within which the player can detect and interact with an interactable object. (Also used for drawing the Gizmos)")]
    [SerializeField] private float _interactionRange = 5.0f;

    
    [SerializeField] private float _dropRange = 2.0f;
    
    
    [Space(10)]
    // Event is fired when this script discovers an interactable object: 
    public UnityEvent<GrabController> OnInteractableDiscovered;
    
    // Event is fired when the last interactable object was either replaced with a new one or just lost:
    public UnityEvent<GrabController> OnInteractableDiscarded;
    
    
    
    // In this game that I am making I like to place items at predeterimend positions using the 'IContainer' type. But, if you want to dynamically enabel placing objects on surfaces you can use hit.position which returns the position of the hitted raycast location and place it there.
    private void Update() {
        RaycastHit? hit = TryObtainHit();
        
        _foundGrabbable = TryObtainGrabbable(hit);

        if (_foundGrabbable != null) {
            
            _grabbableFound = true;
            OnInteractableDiscovered?.Invoke(this);
        }

        else {
            _grabbableFound = false;
            OnInteractableDiscarded?.Invoke(this);
        }
        
        bool canGrab = _grabbableFound && !(_foundGrabbable.IsGrabbed) && (!_holdGrabbable) && _keyIsDownGrab;
            
        if (canGrab) {
            Grab(_foundGrabbable);
        }
        
        bool canDrop = _holdGrabbable && _keyIsDownDrop;

        if (canDrop) {
            Drop(_grabbedGrabbable);
        }
        
        bool canPlace = _holdGrabbable && _keyIsDownPlace && IsSurfaceHorizontal(hit.Value.normal);
        
        if (canPlace) {
            this.Place(_grabbedGrabbable, hit, Quaternion.identity);
        }
        
        // Add code for displaying ghost here:
    }
    
    
    
    // Invoked by 'PlayerInput':
    public void OnGrabbed(InputAction.CallbackContext context) {
        this._keyIsDownGrab = context.ReadValueAsButton();
    }
    
    public void OnDrop(InputAction.CallbackContext context) {
        this._keyIsDownDrop = context.ReadValueAsButton();
    }

    // Invoked by 'PlayerInput':
    public void OnPlace(InputAction.CallbackContext context) {
        this._keyIsDownPlace = context.ReadValueAsButton();
    }
    
    public IGrabbable GetInteractable() {
        return (_foundGrabbable);
    }
    
    private IGrabbable TryObtainGrabbable(RaycastHit? hit) {
        if (!hit.HasValue) {
            return (null);
        }
        
        hit.Value.collider.TryGetComponent(out IGrabbable grabbable);
            
        if (grabbable == null) {
            return (null);
        }
            
        return (grabbable);
    }

    private RaycastHit? TryObtainHit() {
        bool success = Physics.Raycast(this._transform.position, this._transform.forward, out RaycastHit hit, _interactionRange);

        if (!success) {
            return (null);
        }

        return (hit);
    }
    
    private bool IsSurfaceHorizontal(Vector3 surfaceNormal) {
        float maxSlopeAngle = 10f;
        
        // Calculate the angle between the surface normal and Vector3.up
        float angle = Vector3.Angle(surfaceNormal, Vector3.up);
        return angle <= maxSlopeAngle;
    }
    
    // If you do not want to calculate and account for bounds offset, then the best way is to just make sure that the pivot of your grabbable objects is at the bottom.
    private float CalculateBoundsOffset(Renderer renderer)
    {
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            
            return (bounds.extents.y); // Distance from pivot to bottom
        }
        
        return (0.0f);
    }
    
    private void Grab(IGrabbable grabbable) {
        grabbable.GetRigidbody().isKinematic = true;
        
        grabbable.GetTransform().SetParent(this.transform);
        grabbable.GetTransform().localPosition = Vector3.zero;
        grabbable.GetTransform().localRotation = Quaternion.identity;
        
        grabbable.IsGrabbed = true;
        this._grabbedGrabbable = grabbable;
        this._holdGrabbable = true;
    }
    
    private void Drop(IGrabbable grabbable) {
        grabbable.GetTransform().SetParent(null);
        grabbable.GetRigidbody().MovePosition(this.transform.position + this.transform.forward * this._dropRange);
        grabbable.GetTransform().localRotation = Quaternion.identity;
        grabbable.GetRigidbody().isKinematic = false;
        
        grabbable.IsGrabbed = false;
        this._grabbedGrabbable = null;
        this._holdGrabbable = false;
    }

    private void Place(IGrabbable grabbable, RaycastHit? hit, Quaternion rotation) {
        if (!hit.HasValue) {
            return;
        }
        
        grabbable.GetTransform().SetParent(null);
        grabbable.GetTransform().SetPositionAndRotation(hit.Value.point + (hit.Value.normal * CalculateBoundsOffset(grabbable.GetRenderer())), rotation);
        grabbable.GetRigidbody().isKinematic = true;
        
        grabbable.IsGrabbed = false;
        this._grabbedGrabbable = null;
        this._holdGrabbable = false;
        
        // If you are the type of person that wants everything to be cleanedup and tidy, then you can also make the ghost child object here be reset to parent coordinates. but, that is pretty much a waste of resources since we won't even care in the firsst place.
    }
    
    private void OnDrawGizmos() {
        if (!_gizmosIsEnabled) {
            return;
        }
        
        Gizmos.color = _gizmosColor;
        Gizmos.DrawRay(_transform.position, _transform.forward * _interactionRange);
    }
}
