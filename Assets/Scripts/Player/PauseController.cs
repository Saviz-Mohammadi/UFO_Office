using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour {
    
    // Invoked by 'Player Input' component:
    public void OnPaused(InputAction.CallbackContext context) {
        if (context.performed) {
            GameManager.Instance.TogglePause();
        }
    }
}
