using UnityEngine;

public interface IGrabbable {
    
    /// <summary>
    /// Required for preventing grabbing an object that is being held by other players or entities.
    /// </summary>
    public bool IsGrabbed { get; set; }
    
    /// <summary>
    /// This property helps in determining if item is currently placed inside of a placeable location or not and to change that.
    /// </summary>
    public IPlaceableLocation placeableLocationParent { get; set; }
    
    /// <summary>
    /// This method is used to obtain the 'Transform' of the object. Required for manipulating the location and parent-child relationship of the object for creating 'IGrabbable' behaviour.
    /// </summary>
    /// <returns>Transform</returns>
    public Transform GetTransform();
    
    /// <summary>
    /// This method is used to obtain the 'Rigidbody' of the object. Required for manipulating the physical behaviour of the 'IGrabbable' object.
    /// </summary>
    /// <returns>Rigidbody</returns>
    public Rigidbody GetRigidbody();
    
    public Collider GetCollider();
    
    /// <summary>
    /// This method is used to obtain the 'Renderer' of the object. Every 'IGrabbable' object is required to have and return a 'Renderer' for visualizing the ghost replicant of the object when attempting to place it somewhere.
    /// </summary>
    /// <returns>Renderer</returns>
    public Renderer GetRenderer();

    /// <summary>
    /// This method is used to obtain the ghost representation object of the grabbable. Usually, this is a wire frame to give the player a hint as to where the item will be placed.
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject GetGhost();
}
