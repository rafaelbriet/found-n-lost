using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsCanvas;
    [SerializeField]
    private GameObject howToPlayCanvas;

    private void Awake()
    {
        creditsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ToggleCreditsCanvas()
    {
        creditsCanvas.SetActive(!creditsCanvas.activeInHierarchy);
    }

    public void ToggleHowToPlayCanvas()
    {
        howToPlayCanvas.SetActive(!howToPlayCanvas.activeInHierarchy);
    }
}
