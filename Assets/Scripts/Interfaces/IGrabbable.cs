using UnityEngine;

public interface IGrabbable {
    
    /// <summary>
    /// Required for preventing grabbing an object that is being held by other players or entities.
    /// </summary>
    public bool IsGrabbed { get; set; }
    
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
    
    /// <summary>
    /// This method is used to obtain the 'Renderer' of the object. Every 'IGrabbable' object is required to have and return a 'Renderer' for visualizing the ghost replicant of the object when attempting to place it somewhere.
    /// </summary>
    /// <returns>Renderer</returns>
    public Renderer GetRenderer();

    public GameObject GetGhost();
}
