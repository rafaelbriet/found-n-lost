using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsCanvas;

    private void Awake()
    {
        creditsCanvas.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ToggleCreditsCanvas()
    {
        creditsCanvas.SetActive(!creditsCanvas.activeInHierarchy);
    }
}
