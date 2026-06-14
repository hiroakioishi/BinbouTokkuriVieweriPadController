using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInputCooldown : MonoBehaviour
{
    [Header("Cooldown")]
    [SerializeField] private float cooldownSeconds = 1.0f;

    [Header("Targets")]
    [SerializeField] private Selectable[] targets;

    private bool isCoolingDown = false;

    private void Reset()
    {
        targets = GetComponentsInChildren<Selectable>(true);
    }

    private void Awake()
    {
        if (targets == null || targets.Length == 0)
        {
            targets = GetComponentsInChildren<Selectable>(true);
        }

        RegisterEvents();
    }

    private void RegisterEvents()
    {
        foreach (Selectable target in targets)
        {
            if (target is Button button)
            {
                button.onClick.AddListener(BeginCooldown);
            }
            else if (target is Toggle toggle)
            {
                toggle.onValueChanged.AddListener(isOn =>
                {
                    // ToggleGroup使用時はOFFになる側でも呼ばれるので、
                    // ONになった時だけクールダウンする
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

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isCoolingDown = true;

        SetInteractable(false);

        yield return new WaitForSecondsRealtime(cooldownSeconds);

        SetInteractable(true);

        isCoolingDown = false;
    }

    private void SetInteractable(bool interactable)
    {
        foreach (Selectable target in targets)
        {
            if (target != null)
            {
                target.interactable = interactable;
            }
        }
    }
}