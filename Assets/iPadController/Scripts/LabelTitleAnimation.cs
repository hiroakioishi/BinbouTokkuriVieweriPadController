using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

// [ExecuteAlways]
public class LabelTitleAnimation : MonoBehaviour
{
    [SerializeField]
    GameObject _tmpPrefab = default;

    [SerializeField]
    string _titleText = "";

    string _prevTitleText = "";

    List<GameObject> _tmpList = new List<GameObject>();

    public Vector3 CenterPosition = new Vector3(0, 0, 0);
    public float TextMargin = 36;
    public float TextSize = 16;
    public float NoiseFrequency = 1.0f;
    public Vector3 NoiseAmplitude = Vector3.one;
    public float NoiseSpeed = 1.0f;

    float _noiseTimer = 0.0f;

    void Update()
    {
        if (_titleText != _prevTitleText)
        {
            if (_tmpList.Count > 0 && _tmpList != null)
            {
                for (var i = _tmpList.Count - 1; i >= 0; i--)
                {
                    if (Application.isEditor)
                        GameObject.DestroyImmediate(_tmpList[i]);
                    else
                        GameObject.Destroy(_tmpList[i]);
                    _tmpList[i] = null;
                }
                _tmpList.Clear();
                _tmpList = null;
            }

            _tmpList = new List<GameObject>();

            for (int i = 0; i < _titleText.Length; i++)
            {
                GameObject go = (GameObject)Instantiate(_tmpPrefab);
                go.name = "TitleLabel_" + _titleText[i];
                go.transform.parent = transform;
                go.GetComponent<TextMeshProUGUI>().text = _titleText[i].ToString();
                go.transform.localPosition = CenterPosition + new Vector3(TextMargin * i, 0, 0);
                _tmpList.Add(go);
            }
            _prevTitleText = _titleText;
        }

        if (_tmpList.Count > 0 && _tmpList != null)
        {
            for (var i = 0; i < _tmpList.Count; i++)
            {
                _tmpList[i].transform.localPosition =
                    CenterPosition +
                    new Vector3(TextMargin * i, 0, 0) + 
                    new Vector3(
                        NoiseAmplitude.x * noise.snoise(float2(NoiseFrequency * i * 1.3523f, _noiseTimer + i * 0.424f)),
                        NoiseAmplitude.y * noise.snoise(float2(NoiseFrequency * i * 2.2224f, _noiseTimer + i * 0.523f)),
                        NoiseAmplitude.z * noise.snoise(float2(NoiseFrequency * i * 3.4234f, _noiseTimer + i * 0.432f))
                    );
                _tmpList[i].GetComponent<TextMeshProUGUI>().fontSize = TextSize;
            }
        }

        _noiseTimer += Time.deltaTime * NoiseSpeed;
    }
}
