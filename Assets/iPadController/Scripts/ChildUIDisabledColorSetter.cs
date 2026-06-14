using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChildUIDisabledColorSetter : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private bool includeInactive = true;

    [Header("Disabled Color")]
    [SerializeField] private Color disabledColor = new Color(0.45f, 0.45f, 0.45f, 1.0f);

    [Header("Options")]
    [SerializeField] private bool applyOnStart = true;

    private void Start()
    {
        if (applyOnStart)
        {
            ApplyDisabledColor();
        }
    }

    [ContextMenu("Apply Disabled Color To Children")]
    public void ApplyDisabledColor()
    {
        Selectable[] selectables = GetComponentsInChildren<Selectable>(includeInactive);

        foreach (Selectable selectable in selectables)
        {
            if (selectable == null)
            {
                continue;
            }

            ColorBlock colors = selectable.colors;

            // Disabled Color �̂ݕύX
            colors.disabledColor = disabledColor;

            selectable.colors = colors;
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            foreach (Selectable selectable in selectables)
            {
                if (selectable != null)
                {
                    EditorUtility.SetDirty(selectable);
                }
            }
        }
#endif
    }
}