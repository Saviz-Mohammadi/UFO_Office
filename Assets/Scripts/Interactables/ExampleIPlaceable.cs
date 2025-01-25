using System;
using UnityEngine;

public class ExampleIPlaceable : MonoBehaviour, IPlaceableLocation
{
    [SerializeField] private Transform _container;

    public bool IsOccupied { get; set; }

    public Transform GetContainerTransform() {
        return (_container);
    }

    public bool AcceptsType(IGrabbable grabbable) {
        if (grabbable is Item) {
            return (true);   
        }

        return (false);
    }
}
