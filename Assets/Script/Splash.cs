
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 스플래시 씬
/// </summary>
public class Splash : MonoBehaviour
{
    public float time = 2.0f;
    private void Start()
    {
        Invoke("MainScene", time);
    }

    private void MainScene()
    {
        SceneManager.LoadScene(1);
    }
}
