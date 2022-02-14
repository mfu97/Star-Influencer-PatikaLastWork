using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Instance
    private static UIManager instance;
    public static UIManager Instance => instance ??= FindObjectOfType<UIManager>();
    #endregion

    #region Panels
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject barCanvas;
    #endregion

    [SerializeField] private List<GameObject> finishTexts;
    [SerializeField] private List<GameObject> stars;

    [SerializeField] private Image fillingBar;
    public void TapToStart()
    {
        startPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }
    public void StartGame()
    {
        tutorialPanel.SetActive(false);
        hud.SetActive(true);
    }
    public void OnLevelEnd()
    {
        hud.SetActive(false);
        winPanel.SetActive(true);
    }
    public void NextLevel()
    {
        winPanel.SetActive(false);
        hud.SetActive(true);

        // Hide all FinishTexts
        foreach (var finishText in finishTexts)
        {
            finishText.SetActive(false);
        }
        // Hide all Stars
        foreach (var star in stars)
        {
            star.SetActive(false);
        }
    }
    public void SetFinishText(int index)
    {
        // Open FinishText.
        finishTexts[index].SetActive(true);
    }
    public void SetFinishStars(int index) // 0 1 2 
    {
        // Open Stars
        if (index > 0)
        {
            for (int i = 0; i < index + 1; i++)
            {
                stars[i].SetActive(true);
            }
        }
    }
    public void SetTypeBar(int i)
    {
        float f = i * 0.01f;
        fillingBar.fillAmount = f;
    }
    public void ResetTypeBar()
    {
        fillingBar.fillAmount = 0;
    }
    public void SetBarActice(bool b)
    {
        barCanvas.SetActive(b);
    }
}
