using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Button _newGame;
    // Start is called before the first frame update
    void Start()
    {
        _newGame.onClick.AddListener(StartNewGame);
    }
    private void StartNewGame()
    {
        ScenesManager.Instance.LoadNewGame();
    }
}