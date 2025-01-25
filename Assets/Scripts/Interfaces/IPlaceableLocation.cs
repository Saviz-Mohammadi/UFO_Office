using UnityEngine;

// Warning: One potential flaw that I have found with my code is this: What if an object is placed within the trigger by accident such as: being drooped, or physically pushed into the domain despite not being of compatible type. Maybe it is a good idea to do a check on 'Trigger-Enter' to see if type is compatbile before setting isccoupdie.

public interface IPlaceableLocation {
    /// <summary>
    /// A property that determines if the container is currently containing an item. Note that it is best to let this property be set via a trigger in collider.
    /// </summary>
    public bool IsOccupied { get; set; }

    public Transform GetContainerTransform();
    
    bool AcceptsType(IGrabbable grabbable);
}
