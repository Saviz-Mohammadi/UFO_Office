using System;
using UnityEngine;

public class Table : MonoBehaviour, IPlaceableLocation
{
    [SerializeField] private Transform _container;
    
    public bool IsOccupied { get; set; }
    
    public Transform GetTransform() {
        return (_container);
    }

    public bool AcceptsType(IGrabbable grabbable) {
        if (grabbable is Item) {
            return (true);   
        }

        return (false);
    }
}
