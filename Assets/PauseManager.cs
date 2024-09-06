using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class PauseManager : MonoBehaviour
{
    [SerializeField] InputActionAsset pauseAction;
    [SerializeField] UnityEvent onPause, onContinue;

    private void OnEnable()
    {
        pauseAction.Enable();
        pauseAction.FindActionMap("Player").FindAction("Pause").performed += PausePressed;
        GameManager.SetGameState(GameState.PLAYING);
    }
    private void PausePressed(InputAction.CallbackContext context)
    {
        if (GameManager.GetGameState() == GameState.PAUSED)
        {
            onContinue.Invoke();
            GameManager.SetGameState(GameState.PLAYING);
        }
        else if(GameManager.GetGameState() == GameState.PLAYING)
        {
            onPause.Invoke();
            GameManager.SetGameState(GameState.PAUSED);
        }
    }
}
