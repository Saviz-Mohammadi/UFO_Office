using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Color = UnityEngine.Color;

// NOTE: In this game, I am using free placement. If you want to create pre-determined placement, then you could create an interface type of 'IContainer' and check against it to see if item can be placed there. It can have things such as: 'CanContain()', 'IsEmpty()', etc.
// Here is a nice tutorial that is the most like to my implementation: https://www.youtube.com/watch?v=CYk1mEcvhqQ

public class GrabController : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("The selected 'Transform' placed in this field will be used as origin detection point.")]
    [SerializeField] private Transform _transform = null;
    
    [Tooltip("The selected 'Transform' placed in this field will be used as the location to place a grabbable object.")]
    [SerializeField] private Transform _grabTransform = null;
    
    [Header("Events")]
    [Space(10)]
    public UnityEvent<GrabController> OnFoundGrabbableChanged;
    
    [Header("Fields (Customizable)")]
    [Tooltip("Controls weather the player will use exact placement using 'IPlaceableLocation', or free placement.")]
    [SerializeField] private bool _useFreePlacement = true;
    
    [Tooltip("Controls the 'Enabled' state of the Gizmos.")]
    [SerializeField] private bool _gizmosIsEnabled = true;
    
    [Tooltip("Controls the color of the drawn Gizmos.")]
    [SerializeField] private Color _gizmosColor = Color.green;
    
    [Space(10)]
    [Tooltip("Controls the range within which the player can detect and interact with a grabbable object. (Also used for drawing the Gizmos)")]
    [SerializeField] private float _interactionRange = 5.0f;

    [Tooltip("Controls the range that the player will drop the item being held.")]
    [SerializeField] private float _dropDistance = 2.0f;

    [SerializeField] private LayerMask _maskLayer;
    
    private IGrabbable _foundGrabbable = null;
    
    private IGrabbable _grabbedGrabbable = null;
    
    private IPlaceableLocation _foundPlaceableLocation = null;
    
    private GameObject _ghostPrefab = null;
    
    private bool _isHolding = false;
    
    private bool _keyIsDownDrop = false;
    
    private bool _keyIsDownGrab = false;
    
    private bool _keyIsDownPlace = false;
    
    private void Update() {
        RaycastHit? hit = TryObtainHit();
        
        this._foundGrabbable = TryObtainGrabbable(hit);
        this._foundPlaceableLocation = TryObtainPlaceableLocation(hit);

        OnFoundGrabbableChanged?.Invoke(this);
        
        bool canGrab = this._foundGrabbable != null && !this._foundGrabbable.IsGrabbed && !this._isHolding && this._keyIsDownGrab;
            
        if (canGrab) {
            this.Grab(this._foundGrabbable);
        }
        
        bool canDrop = this._isHolding && this._keyIsDownDrop;

        if (canDrop) {
            this.Drop(this._grabbedGrabbable);
        }

        if (this._useFreePlacement) {
            bool canPlace = this._isHolding && this._keyIsDownPlace && IsSurfaceHorizontal(hit);
        
            if (canPlace) {
                this.Place(this._grabbedGrabbable, hit, Quaternion.identity);
            }
            
            bool canDisplayGhost = this._isHolding && IsSurfaceHorizontal(hit);
            
            if (canDisplayGhost) {
                this.DisplayGhost(this._grabbedGrabbable, hit, Quaternion.identity);
            }
        }

        else {
            bool canPlace = this._isHolding && this._keyIsDownPlace && _foundPlaceableLocation != null && !_foundPlaceableLocation.IsOccupied && _foundPlaceableLocation.AcceptsType(_grabbedGrabbable);
        
            if (canPlace) {
                this.Place(this._foundPlaceableLocation, this._grabbedGrabbable);
            }
            
            bool canDisplayGhost = this._isHolding && _foundPlaceableLocation != null && !_foundPlaceableLocation.IsOccupied && _foundPlaceableLocation.AcceptsType(_grabbedGrabbable);
            
            if (canDisplayGhost) {
                this.DisplayGhost(this._foundPlaceableLocation, this._grabbedGrabbable);
            }
        }
    }

    private void OnDrawGizmos() {
        if (!_gizmosIsEnabled) {
            return;
        }
        
        Gizmos.color = _gizmosColor;
        Gizmos.DrawRay(_transform.position, _transform.forward * _interactionRange);
    }
    
    // Invoked by 'PlayerInput':
    public void OnGrabbed(InputAction.CallbackContext context) {
        this._keyIsDownGrab = context.ReadValueAsButton();
    }
    
    // Invoked by 'PlayerInput':
    public void OnDroped(InputAction.CallbackContext context) {
        this._keyIsDownDrop = context.ReadValueAsButton();
    }

    // Invoked by 'PlayerInput':
    public void OnPlaced(InputAction.CallbackContext context) {
        this._keyIsDownPlace = context.ReadValueAsButton();
    }
    
    public IGrabbable GetInteractable() {
        return (_foundGrabbable);
    }
    
    private RaycastHit? TryObtainHit() {
        bool success = Physics.Raycast(this._transform.position, this._transform.forward, out RaycastHit hit, _interactionRange, _maskLayer);

        if (!success) {
            return (null);
        }

        return (hit);
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
    
    private IPlaceableLocation TryObtainPlaceableLocation(RaycastHit? hit) {
        if (!hit.HasValue) {
            return (null);
        }
        
        hit.Value.collider.TryGetComponent(out IPlaceableLocation placeableLocation);
            
        if (placeableLocation == null) {
            return (null);
        }

        return (placeableLocation);
    }
    
    private bool IsSurfaceHorizontal(RaycastHit? hit) {
        float maxSlopeAngle = 10f;

        if (!hit.HasValue) {
            return (false);
        }
        
        Vector3 surfaceNormal = hit.Value.normal;
        // Calculate the angle between the surface normal and Vector3.up
        float angle = Vector3.Angle(surfaceNormal, Vector3.up);
        return angle <= maxSlopeAngle;
    }
    
    private float CalculateBoundsOffset(Renderer renderer) {
        if (renderer != null) {
            Bounds bounds = renderer.bounds;
            return (bounds.extents.y); // Distance from pivot to bottom
        }
        
        return (0.0f);
    }
    
    private void Grab(IGrabbable grabbable) {
        //grabbable.GetRigidbody().isKinematic = true;
        grabbable.GetCollider().enabled = false;
        
        
        grabbable.GetTransform().SetParent(this._grabTransform);
        grabbable.GetTransform().localPosition = Vector3.zero;
        grabbable.GetTransform().localRotation = Quaternion.identity;
        
        grabbable.IsGrabbed = true;

        if (grabbable.placeableLocationParent != null) {
            grabbable.placeableLocationParent.IsOccupied = false;
        }
        
        grabbable.placeableLocationParent = null;
        
        this._grabbedGrabbable = grabbable;
        this._ghostPrefab = grabbable.GetGhost();
        this._ghostPrefab.SetActive(true);
        this._isHolding = true;
    }
    
    private void Drop(IGrabbable grabbable) {
        grabbable.GetTransform().SetParent(null);
        grabbable.GetRigidbody().MovePosition(this.transform.position + this.transform.forward * this._dropDistance);
        grabbable.GetTransform().localRotation = Quaternion.identity;
        //grabbable.GetRigidbody().isKinematic = false;
        grabbable.GetCollider().enabled = true;
        
        grabbable.IsGrabbed = false;
        grabbable.GetGhost().SetActive(false);
        this._grabbedGrabbable = null;
        this._ghostPrefab.SetActive(false);
        this._ghostPrefab = null;
        this._isHolding = false;
    }

    private void Place(IGrabbable grabbable, RaycastHit? hit, Quaternion rotation) {
        if (!hit.HasValue) {
            return;
        }
        
        grabbable.GetTransform().SetParent(null);
        
        // Maybe I am dumb or something, but for some reason this works and nothing else. I am not sure what the problme is. All the logic seems to work correctly, except for moving and positionng. Some really strange things happen when I attempt to move
        // My guess is that when moving the rigidbody and collider of the item somehow interact with the player, causing wierd stuff happen.
        grabbable.GetRigidbody().MovePosition(hit.Value.point + hit.Value.normal * CalculateBoundsOffset(grabbable.GetRenderer()));
        grabbable.GetTransform().localRotation = rotation;
        //grabbable.GetTransform().SetPositionAndRotation(, rotation);
        //grabbable.GetRigidbody().isKinematic = false;
        
        grabbable.GetCollider().enabled = true;
        //Physics.IgnoreCollision(grabbable.GetCollider(), this.GetComponent<Collider>(), true);

        //RestoreCollision(grabbable);
        
        grabbable.IsGrabbed = false;
        this._grabbedGrabbable = null;
        this._ghostPrefab.SetActive(false);
        this._ghostPrefab = null;
        this._isHolding = false;
    }
    
    private void Place(IPlaceableLocation placeableLocation, IGrabbable grabbable) {
        grabbable.GetTransform().SetParent(null);
        
        Transform containerTransform = placeableLocation.GetContainerTransform();
        
        grabbable.GetTransform().SetPositionAndRotation(containerTransform.position, containerTransform.rotation);
        grabbable.GetRigidbody().isKinematic = false;
        
        grabbable.IsGrabbed = false;
        grabbable.placeableLocationParent = placeableLocation;
        grabbable.placeableLocationParent.IsOccupied = true;
        this._grabbedGrabbable = null;
        this._ghostPrefab.SetActive(false);
        this._ghostPrefab = null;
        this._isHolding = false;
    }

    private void DisplayGhost(IGrabbable grabbable, RaycastHit? hit, Quaternion rotation) {
        if (!hit.HasValue) {
            return;
        }
        
        //grabbable.GetGhost().SetActive(true);
        _ghostPrefab.transform.SetPositionAndRotation(hit.Value.point + (hit.Value.normal * CalculateBoundsOffset(grabbable.GetRenderer())), rotation);
    }
    
    private void DisplayGhost(IPlaceableLocation placeableLocation, IGrabbable grabbable) {
        Transform containerTransform = placeableLocation.GetContainerTransform();
        
        //grabbable.GetGhost().SetActive(true);
        // The assumption here is that if a certain 'IContainer' can accept our type of item, then they should have a Transform that can fit and represent our ghost since the ghost is just a hint representation of our actual item.
        _ghostPrefab.transform.SetPositionAndRotation(containerTransform.position, containerTransform.rotation);
    }
}
