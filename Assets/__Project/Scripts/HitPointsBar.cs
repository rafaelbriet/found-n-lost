using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HitPointsBar : MonoBehaviour
{
    [SerializeField]
    private Character character;
    [SerializeField]
    private Image image;
    [SerializeField]
    private float fadeInDuration = 0.2f;
    [SerializeField]
    private float fadeOutDuration = 0.2f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        image.fillAmount = 1;
    }

    private void OnEnable()
    {
        character.Damaged += OnCharacterDamaged;
        character.Died += OnCharacterDied;
    }

    private void OnDisable()
    {
        character.Damaged -= OnCharacterDamaged;
        character.Died -= OnCharacterDied;
    }

    private void OnCharacterDied(object sender, System.EventArgs e)
    {
        StartCoroutine(FadeOut(fadeInDuration));
    }

    private void OnCharacterDamaged(object sender, System.EventArgs e)
    {
        if (canvasGroup.alpha == 0)
        {
            StartCoroutine(FadeIn(fadeInDuration));
        }


        image.fillAmount = (float)character.CurrentHitPoints / character.MaxHitPoints;
    }

    private IEnumerator FadeIn(float duration)
    {
        float timePassed = 0;
        float timeStartedLerp = Time.time;

        while (timePassed <= duration)
        {
            float timeSinceStartedLerp = Time.time - timeStartedLerp;
            float percentComplete = timeSinceStartedLerp / duration;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, percentComplete);

            timePassed += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator FadeOut(float duration)
    {
        float timePassed = 0;
        float timeStartedLerp = Time.time;

        while (timePassed <= duration)
        {
            float timeSinceStartedLerp = Time.time - timeStartedLerp;
            float percentComplete = timeSinceStartedLerp / duration;

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, percentComplete);

            timePassed += Time.deltaTime;

            yield return null;
        }
    }
}
