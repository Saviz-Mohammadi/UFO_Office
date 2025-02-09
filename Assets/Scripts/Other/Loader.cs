using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public static class Loader {

    public static async void LoadScene(string sceneName, ProgressBar progressBar) {
        var targetScene = SceneManager.LoadSceneAsync(sceneName);

        progressBar.value = 0.0f;
        
        // Stop scene from activating immediately:
        targetScene.allowSceneActivation = false;

        do {
            progressBar.value = targetScene.progress;
            
        } while (targetScene.progress < 0.9f);
        
        // Enable scene activation:
        targetScene.allowSceneActivation = true;
    }

    public static void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
