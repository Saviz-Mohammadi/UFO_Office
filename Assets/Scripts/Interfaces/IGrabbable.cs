using UnityEngine;

public interface IGrabbable {
    
    public void Grab(Transform gameObject);
    public void Drop(Transform gameObject);
}
