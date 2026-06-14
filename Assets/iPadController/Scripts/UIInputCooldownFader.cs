using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInputCooldownFader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Selectable[] targets;

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
        targets = GetComponentsInChildren<Selectable>(true);
    }

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (targets == null || targets.Length == 0)
        {
            targets = GetComponentsInChildren<Selectable>(true);
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = enabledAlpha;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        RegisterEvents();
    }

    private void RegisterEvents()
    {
        foreach (Selectable target in targets)
        {
            if (target == null)
            {
                continue;
            }

            if (target is Button button)
            {
                button.onClick.AddListener(BeginCooldown);
            }
            else if (target is Toggle toggle)
            {
                toggle.onValueChanged.AddListener(isOn =>
                {
                    // ToggleGroup使用時はOFF側でも呼ばれるので、ONになった時だけ実行
                    if (isOn)
                    {
                        BeginCooldown();
                    }
                });
            }
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
        SetInteractable(false);

        // UIを薄暗くする
        yield return FadeAlpha(canvasGroup.alpha, disabledAlpha, fadeOutSeconds);

        // 操作不可のまま待つ
        yield return new WaitForSecondsRealtime(disabledSeconds);

        // UIを戻す
        yield return FadeAlpha(canvasGroup.alpha, enabledAlpha, fadeInSeconds);

        // 入力を復帰
        SetInteractable(true);

        isCoolingDown = false;
        routine = null;
    }

    private void SetInteractable(bool interactable)
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = interactable;
            canvasGroup.blocksRaycasts = interactable;
        }

        foreach (Selectable target in targets)
        {
            if (target != null)
            {
                target.interactable = interactable;
            }
        }
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        if (canvasGroup == null)
        {
            yield break;
        }

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
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            canvasGroup.alpha = Mathf.Lerp(from, to, easedT);

            yield return null;
        }

        canvasGroup.alpha = to;
    }
}