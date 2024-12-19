using UnityEngine;

public class CubeInteractable : MonoBehaviour, IInteractable {
    
    public void Interact(GameObject interactorObject) {
        
        Debug.Log("You interacted with me: " + gameObject.name);
    }

    public string ObtainInteractionText() {

        return ("My name is: " + gameObject.name);
    }

    public Transform ObtainTransform() {
        
        return (transform);
    }
}
