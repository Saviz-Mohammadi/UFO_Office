using UnityEngine;

public interface IPlaceableLocation
{
    bool IsOccupied { get; set; }

    public Transform GetTransform();
    
    bool AcceptsType(IGrabbable grabbable);
}
