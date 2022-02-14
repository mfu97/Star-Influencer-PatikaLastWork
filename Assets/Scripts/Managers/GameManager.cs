using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance => instance ??= FindObjectOfType<GameManager>();
    #endregion

    // Level List
    [SerializeField] private LevelList _levelList;

    #region Variables
    private bool gameStarted;
    public bool gameReset;
    #endregion

    private void Start()
    {
        // Set Level List
        LevelManager.SetLevelManager(_levelList);
    }
    public void TapToStart()
    {
        UIManager.Instance.TapToStart();
    }
    public void StartGame()
    {
        gameStarted = true;
        UIManager.Instance.StartGame();
        Player.Instance.AnimationChange();
    }
    public void OnLevelEnd()
    {
        gameStarted = false;
        UIManager.Instance.OnLevelEnd();
    }
    public void NextGame()
    {
        gameStarted = true;
        UIManager.Instance.SetBarActice(true);
        LevelManager.NextLevel();
        UIManager.Instance.NextLevel();
        ResetGame();
        Player.Instance.AnimationChange();
    }
    public bool IsGameStarted()
    {
        return gameStarted;
    }

    private void ResetGame() 
    {
        PlayerCollisionController.Instance.GameReset();
        CameraManager.Instance.ResetCameraPosition();
        Player.Instance.AnimationReset();
        UIManager.Instance.ResetTypeBar();
    }
}
