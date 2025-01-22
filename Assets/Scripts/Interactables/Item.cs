using UnityEngine;

public class Item : MonoBehaviour, IGrabbable
{
    [SerializeField] private Rigidbody rb;
    private Transform target = null;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null) {
            rb.MovePosition(target.position);
        }
    }

    public void Grab(Transform t) {
        
        rb.isKinematic = true;
        target = t;
    }

    // Rename this to be placed
    public void Drop(Transform t) {
        
        target = null;
        rb.MovePosition(t.position);
        rb.isKinematic = false;
    }
}
