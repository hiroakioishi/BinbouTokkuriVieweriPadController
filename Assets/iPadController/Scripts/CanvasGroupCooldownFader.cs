using System.Collections;
using UnityEngine;

public class CanvasGroupCooldownFader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Cooldown")]
    [SerializeField] private float disabledSeconds = 1.0f;

    [Header("Fade")]
    [SerializeField] private float fadeOutSeconds = 0.15f;
    [SerializeField] private float fadeInSeconds = 0.25f;

    [Header("Alpha")]
    [SerializeField] private float enabledAlpha = 1.0f;
    [SerializeField] private float disabledAlpha = 0.45f;

    private bool isCoolingDown = false;
    private Coroutine routine;

    public bool IsCoolingDown => isCoolingDown;

    private void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = enabledAlpha;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void BeginCooldown()
    {
        if (isCoolingDown)
        {
            return;
        }

        if (routine != null)
        {
            StopCoroutine(routine);
        }

        routine = StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isCoolingDown = true;

        // 入力は即座に止める
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // 薄暗くフェードアウト
        yield return FadeAlpha(canvasGroup.alpha, disabledAlpha, fadeOutSeconds);

        // 操作不可のまま待つ
        yield return new WaitForSecondsRealtime(disabledSeconds);

        // 明るくフェードイン
        yield return FadeAlpha(canvasGroup.alpha, enabledAlpha, fadeInSeconds);

        // 入力復帰
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        isCoolingDown = false;
        routine = null;
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            canvasGroup.alpha = to;
            yield break;
        }

        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);

            // 少し滑らかな変化
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            canvasGroup.alpha = Mathf.Lerp(from, to, easedT);

            yield return null;
        }

        canvasGroup.alpha = to;
    }
}