using UnityEngine;
using System.Collections.Generic;

public static class LevelManager
{
    private static int _levelIndex;

    private static LevelList _levelList;
    private static Level _currentLevel;

    public static void SetLevelManager(LevelList list)
    {
        _levelList = list;
        _levelIndex = PlayerPrefs.GetInt("levelIndex", 0);

        LoadLevel();
    }
    public static void LoadLevel()
    {
        _currentLevel = GameObject.Instantiate(_levelList.levels[_levelIndex % _levelList.levels.Count]);
    }
    public static void NextLevel()
    {
        _currentLevel.DestroyLevel();
        _levelIndex++;
        PlayerPrefs.SetInt("levelIndex", _levelIndex);

        LoadLevel();
    }
    public static Level GetCurrentLevel()
    {
        return _currentLevel;
    }
    public static int GetCurrentLevelIndex()
    {
        return _levelIndex;
    }
    public static int GetCurrentLevelNumber()
    {
        return _levelIndex + 1;
    }
}
