using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class ToggleOutlineView : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Outline outline;

    private void Reset()
    {
        toggle = GetComponent<Toggle>();
        outline = GetComponentInChildren<Outline>();
    }

    private void Awake()
    {
        if (toggle == null)
        {
            toggle = GetComponent<Toggle>();
        }

        toggle.onValueChanged.AddListener(UpdateView);
        UpdateView(toggle.isOn);
    }

    private void OnDestroy()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(UpdateView);
        }
    }

    private void UpdateView(bool isOn)
    {
        if (outline != null)
        {
            outline.enabled = isOn;
        }
    }
}