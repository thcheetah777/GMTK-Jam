using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

        public enum Scene
    {
        StartScene,
        GameScene,
        DeathScene
    }
    public void LoadNewGame()
    {
        SceneManager.LoadScene(Scene.GameScene.ToString());
    }
    public void LoadNextScene()
    {
     //   SceneManager.LoadScene(SceneManager.GetActiveScene().builtIndex + 1);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Scene.StartScene.ToString());
    }
}
