using System;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using OscJack;

public class ControlOscSender : MonoBehaviour
{
    [Header("OSC Settings")]
    public string IPAddress = "127.0.0.1";
    public int OscPort = 6666;

    private OscClient _oscClient;
    private bool _isQuitting = false;

    public enum 徳利タイプ
    {
        すべて,
        高田美濃焼,
        丹波立杭焼,
        有田焼系
    }

    public enum モデル表示形式
    {
        通常3Dモデル,
        円筒展開平面
    }

    public enum 並べ方
    {
        指定の位置で表示,
        ランダムな並びで表示
    }

    public enum 表示モード
    {
        素面,
        酩酊
    }

    [Header("フラグ")]
    public 徳利タイプ TokkuriType = 徳利タイプ.すべて;
    public モデル表示形式 ModelViewMode = モデル表示形式.通常3Dモデル;
    public 並べ方 Alignment = 並べ方.指定の位置で表示;
    public 表示モード ViewMode = 表示モード.素面;

    [Header("Toggle References")]

    [Header("TokkuriType")]
    [SerializeField] private Toggle ToggleTokkuriType_All;
    [SerializeField] private Toggle ToggleTokkuriType_TakadaMinou;
    [SerializeField] private Toggle ToggleTokkuriType_TanbaTachikui;
    [SerializeField] private Toggle ToggleTokkuriType_Arita;

    [Header("ModelViewMode")]
    [SerializeField] private Toggle ToggleModelViewMode_Default;
    [SerializeField] private Toggle ToggleModelViewMode_UnwrapMap;

    [Header("AlignmentMode")]
    [SerializeField] private Toggle ToggleAlignmentMode_Default;
    [SerializeField] private Toggle ToggleAlignmentMode_Random;

    [Header("ViewMode")]
    [SerializeField] private Toggle ToggleViewMode_Shirafu;
    [SerializeField] private Toggle ToggleViewMode_Meitei;

    private void Awake()
    {
        CreateOscClient();
    }

    private void OnDestroy()
    {
        DisposeOscClient();
    }

    private void OnApplicationQuit()
    {
        _isQuitting = true;
        DisposeOscClient();
    }

    private void CreateOscClient()
    {
        DisposeOscClient();

        try
        {
            _oscClient = new OscClient(IPAddress, OscPort);
        }
        catch (Exception e)
        {
            Debug.LogError($"OscClient の生成に失敗しました: {e.Message}");
            _oscClient = null;
        }
    }

    private void DisposeOscClient()
    {
        if (_oscClient == null)
            return;

        try
        {
            _oscClient.Dispose();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"OscClient の破棄時に例外が発生しました: {e.Message}");
        }
        finally
        {
            _oscClient = null;
        }
    }

    private void SendOsc(string address, int value = 1)
    {
        if (_isQuitting)
            return;

        if (_oscClient == null)
        {
            CreateOscClient();
        }

        if (_oscClient == null)
        {
            Debug.LogError($"OSC送信に失敗しました。OscClient が生成されていません。Address: {address}");
            return;
        }

        try
        {
            _oscClient.Send(address, value);
        }
        catch (SocketException e)
        {
            Debug.LogWarning($"OSC送信時に SocketException が発生しました。OscClient を再生成します。Address: {address}, Error: {e.Message}");

            CreateOscClient();

            if (_oscClient == null)
                return;

            try
            {
                _oscClient.Send(address, value);
            }
            catch (Exception retryException)
            {
                Debug.LogError($"OSC再送信に失敗しました。Address: {address}, Error: {retryException.Message}");
            }
        }
        catch (ObjectDisposedException e)
        {
            Debug.LogWarning($"OscClient がすでに破棄されていました。OscClient を再生成します。Address: {address}, Error: {e.Message}");

            CreateOscClient();

            if (_oscClient == null)
                return;

            try
            {
                _oscClient.Send(address, value);
            }
            catch (Exception retryException)
            {
                Debug.LogError($"OSC再送信に失敗しました。Address: {address}, Error: {retryException.Message}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"OSC送信に失敗しました。Address: {address}, Error: {e.Message}");
        }
    }

    public void ToggleEnableEmission()
    {
        EnableEmission();
    }
    public void ToggleDisableEmission()
    {
        DisableEmission();
    }

    public void TogglePressed()
    {
        UpdateTokkuriTypeFromToggle();
        UpdateModelViewModeFromToggle();
        UpdateAlignmentFromToggle();

        if (TokkuriType == 徳利タイプ.すべて)
        {
            if (ModelViewMode == モデル表示形式.通常3Dモデル)
            {
                if (Alignment == 並べ方.指定の位置で表示)
                {
                    TransformDefaultOnSpecificIndexPosition();
                }
                else if (Alignment == 並べ方.ランダムな並びで表示)
                {
                    TransformDefaultOnRandomPosition();
                }
            }
            else if (ModelViewMode == モデル表示形式.円筒展開平面)
            {
                if (Alignment == 並べ方.指定の位置で表示)
                {
                    TransformUnwrapMapOnSpecificIndexPosition();
                }
                else if (Alignment == 並べ方.ランダムな並びで表示)
                {
                    TransformUnwrapMapOnRandomPosition();
                }
            }
        }
        else if (TokkuriType == 徳利タイプ.高田美濃焼)
        {
            if (ModelViewMode == モデル表示形式.通常3Dモデル)
            {
                if (Alignment == 並べ方.指定の位置で表示)
                {
                    TransformDefaultOnlyMinouOnAutoPackedGridPosition();
                }
                else if (Alignment == 並べ方.ランダムな並びで表示)
                {
                    TransformDefaultOnlyMinouOnAutoPackedGridPosition();
                }
            }
            else if (ModelViewMode == モデル表示形式.円筒展開平面)
            {
                if (Alignment == 並べ方.指定の位置で表示)
                {
                    TransformUnwrapMapOnlyMinouyakiOnAutoPackedGridPosition();
                }
                else if (Alignment == 並べ方.ランダムな並びで表示)
                {
                    TransformUnwrapMapOnlyMinouyakiOnAutoPackedGridPosition();
                }
            }
        }
        else if (TokkuriType == 徳利タイプ.丹波立杭焼)
        {
            if (ModelViewMode == モデル表示形式.通常3Dモデル)
            {
                if (Alignment == 並べ方.指定の位置で表示)
                {
                    TransformDefaultOnlyTanbaTachikuiyakiOnAutoPackedGridPosition();
                }
                else if (Alignment == 並べ方.ランダムな並びで表示)
                {
                    TransformDefaultOnlyTanbaTachikuiyakiOnAutoPackedGridPosition();
                }
            }
            else if (ModelViewMode == モデル表示形式.円筒展開平面)
            {
                if (Alignment == 並べ方.指定の位置で表示)
                {
                    TransformUnwrapMapOnlyTanbaTachikuiyakiOnAutoPackedGridPosition();
                }
                else if (Alignment == 並べ方.ランダムな並びで表示)
                {
                    TransformUnwrapMapOnlyTanbaTachikuiyakiOnAutoPackedGridPosition();
                }
            }
        }
        else if (TokkuriType == 徳利タイプ.有田焼系)
        {
            if (ModelViewMode == モデル表示形式.通常3Dモデル)
            {
                if (Alignment == 並べ方.指定の位置で表示)
                {
                    TransformDefaultOnlyImariAritaOnAutoPackedGridPosition();
                }
                else if (Alignment == 並べ方.ランダムな並びで表示)
                {
                    TransformDefaultOnlyImariAritaOnAutoPackedGridPosition();
                }
            }
            else if (ModelViewMode == モデル表示形式.円筒展開平面)
            {
                if (Alignment == 並べ方.指定の位置で表示)
                {
                    TransformUnwrapMapOnlyImariAritaOnAutoPackedGridPosition();
                }
                else if (Alignment == 並べ方.ランダムな並びで表示)
                {
                    TransformUnwrapMapOnlyImariAritaOnAutoPackedGridPosition();
                }
            }
        }
    }

    public void TogglePressedViewMode()
    {
        UpdateViewModeFromToggle();

        if (ViewMode == 表示モード.素面)
        {
            SendOsc("/EnableEmission");
        }
        else if (ViewMode == 表示モード.酩酊)
        {
            SendOsc("/DisableEmission");
        }
    }

    private void UpdateTokkuriTypeFromToggle()
    {
        if (ToggleTokkuriType_All != null && ToggleTokkuriType_All.isOn)
        {
            TokkuriType = 徳利タイプ.すべて;
        }
        else if (ToggleTokkuriType_TakadaMinou != null && ToggleTokkuriType_TakadaMinou.isOn)
        {
            TokkuriType = 徳利タイプ.高田美濃焼;
        }
        else if (ToggleTokkuriType_TanbaTachikui != null && ToggleTokkuriType_TanbaTachikui.isOn)
        {
            TokkuriType = 徳利タイプ.丹波立杭焼;
        }
        else if (ToggleTokkuriType_Arita != null && ToggleTokkuriType_Arita.isOn)
        {
            TokkuriType = 徳利タイプ.有田焼系;
        }
    }

    private void UpdateModelViewModeFromToggle()
    {
        if (ToggleModelViewMode_Default != null && ToggleModelViewMode_Default.isOn)
        {
            ModelViewMode = モデル表示形式.通常3Dモデル;
        }
        else if (ToggleModelViewMode_UnwrapMap != null && ToggleModelViewMode_UnwrapMap.isOn)
        {
            ModelViewMode = モデル表示形式.円筒展開平面;
        }
    }

    private void UpdateAlignmentFromToggle()
    {
        if (ToggleAlignmentMode_Default != null && ToggleAlignmentMode_Default.isOn)
        {
            Alignment = 並べ方.指定の位置で表示;
        }
        else if (ToggleAlignmentMode_Random != null && ToggleAlignmentMode_Random.isOn)
        {
            Alignment = 並べ方.ランダムな並びで表示;
        }
    }

    private void UpdateViewModeFromToggle()
    {
        if (ToggleViewMode_Shirafu != null && ToggleViewMode_Shirafu.isOn)
        {
            ViewMode = 表示モード.素面;
        }
        else if (ToggleViewMode_Meitei != null && ToggleViewMode_Meitei.isOn)
        {
            ViewMode = 表示モード.酩酊;
        }
    }

    public void TransformDefault()
    {
        SendOsc("/TransformDefault");
    }

    public void TransformUnwrapMap()
    {
        SendOsc("/TransformUnwrapMap");
    }

    public void TransformDefaultOnSpecificIndexPosition()
    {
        SendOsc("/TransformDefaultOnSpecificIndexPosition");
    }

    public void TransformUnwrapMapOnSpecificIndexPosition()
    {
        SendOsc("/TransformUnwrapMapOnSpecificIndexPosition");
    }

    public void TransformDefaultOnRandomPosition()
    {
        SendOsc("/TransformDefaultOnRandomPosition");
    }

    public void TransformUnwrapMapOnRandomPosition()
    {
        SendOsc("/TransformUnwrapMapOnRandomPosition");
    }

    public void TransformDefaultOnlyMinouOnAutoPackedGridPosition()
    {
        SendOsc("/TransformDefaultOnlyMinouOnAutoPackedGridPosition");
    }

    public void TransformDefaultOnlyTanbaTachikuiyakiOnAutoPackedGridPosition()
    {
        SendOsc("/TransformDefaultOnlyTanbaTachikuiyakiOnAutoPackedGridPosition");
    }

    public void TransformDefaultOnlyImariAritaOnAutoPackedGridPosition()
    {
        SendOsc("/TransformDefaultOnlyImariAritaOnAutoPackedGridPosition");
    }

    public void TransformUnwrapMapOnlyMinouyakiOnAutoPackedGridPosition()
    {
        SendOsc("/TransformUnwrapMapOnlyMinouyakiOnAutoPackedGridPosition");
    }

    public void TransformUnwrapMapOnlyTanbaTachikuiyakiOnAutoPackedGridPosition()
    {
        SendOsc("/TransformUnwrapMapOnlyTanbaTachikuiyakiOnAutoPackedGridPosition");
    }

    public void TransformUnwrapMapOnlyImariAritaOnAutoPackedGridPosition()
    {
        SendOsc("/TransformUnwrapMapOnlyImariAritaOnAutoPackedGridPosition");
    }

    public void EnableEmission()
    {
        SendOsc("/EnableEmission");
    }

    public void DisableEmission()
    {
        SendOsc("/DisableEmission");
    }

    public void RecreateOscClient()
    {
        CreateOscClient();
    }
}