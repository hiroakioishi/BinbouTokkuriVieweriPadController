using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ControlOscSender : MonoBehaviour
{
    [Header("OSC Settings")]
    public string IPAddress = "127.0.0.1";
    public int OscPort = 6666;

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

    [Header("Send Option")]
    [SerializeField] private bool LogSendMessage = false;

    private void SendOsc(string address, int value = 1)
    {
        if (string.IsNullOrEmpty(address))
        {
            Debug.LogWarning("OSC Address が空です。");
            return;
        }

        if (!address.StartsWith("/"))
        {
            address = "/" + address;
        }

        try
        {
            byte[] packet = BuildOscMessage(address, value);

            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.Send(packet, packet.Length, IPAddress, OscPort);
            }

            if (LogSendMessage)
            {
                Debug.Log($"OSC Send: {address} {value} -> {IPAddress}:{OscPort}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"OSC送信に失敗しました。Address: {address}, Error: {e.Message}");
        }
    }

    private static byte[] BuildOscMessage(string address, int value)
    {
        List<byte> data = new List<byte>();

        AddPaddedString(data, address);
        AddPaddedString(data, ",i");
        AddInt32BigEndian(data, value);

        return data.ToArray();
    }

    private static void AddPaddedString(List<byte> data, string text)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(text);

        data.AddRange(bytes);
        data.Add(0);

        while (data.Count % 4 != 0)
        {
            data.Add(0);
        }
    }

    private static void AddInt32BigEndian(List<byte> data, int value)
    {
        data.Add((byte)((value >> 24) & 0xFF));
        data.Add((byte)((value >> 16) & 0xFF));
        data.Add((byte)((value >> 8) & 0xFF));
        data.Add((byte)(value & 0xFF));
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
                    TransformDefaultOnlyMinouOnSpecificIndexPosition();
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
                    TransformUnwrapMapOnlyMinouyakiOnSpecificIndexPosition();
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
                    TransformDefaultOnlyTanbaTachikuiyakiOnSpecificIndexPosition();
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
                    TransformUnwrapMapOnlyTanbaTachikuiyakiOnSpecificIndexPosition();
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
                    TransformDefaultOnlyImariAritaOnSpecificIndexPosition();
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
                    TransformUnwrapMapOnlyImariAritaOnSpecificIndexPosition();
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

    public void TransformDefaultOnlyMinouOnSpecificIndexPosition()
    {
        SendOsc("/TransformDefaultOnlyMinouOnSpecificIndexPosition");
    }

    public void TransformDefaultOnlyMinouOnAutoPackedGridPosition()
    {
        SendOsc("/TransformDefaultOnlyMinouOnAutoPackedGridPosition");
    }

    public void TransformDefaultOnlyTanbaTachikuiyakiOnSpecificIndexPosition()
    {
        SendOsc("/TransformDefaultOnlyTanbaTachikuiyakiOnSpecificIndexPosition");
    }

    public void TransformDefaultOnlyTanbaTachikuiyakiOnAutoPackedGridPosition()
    {
        SendOsc("/TransformDefaultOnlyTanbaTachikuiyakiOnAutoPackedGridPosition");
    }

    public void TransformDefaultOnlyImariAritaOnSpecificIndexPosition()
    {
        SendOsc("/TransformDefaultOnlyImariAritaOnSpecificIndexPosition");
    }

    public void TransformDefaultOnlyImariAritaOnAutoPackedGridPosition()
    {
        SendOsc("/TransformDefaultOnlyImariAritaOnAutoPackedGridPosition");
    }

    public void TransformUnwrapMapOnlyMinouyakiOnSpecificIndexPosition()
    {
        SendOsc("/TransformUnwrapMapOnlyMinouyakiOnSpecificIndexPosition");
    }

    public void TransformUnwrapMapOnlyMinouyakiOnAutoPackedGridPosition()
    {
        SendOsc("/TransformUnwrapMapOnlyMinouyakiOnAutoPackedGridPosition");
    }

    public void TransformUnwrapMapOnlyTanbaTachikuiyakiOnSpecificIndexPosition()
    {
        SendOsc("/TransformUnwrapMapOnlyTanbaTachikuiyakiOnSpecificIndexPosition");
    }

    public void TransformUnwrapMapOnlyTanbaTachikuiyakiOnAutoPackedGridPosition()
    {
        SendOsc("/TransformUnwrapMapOnlyTanbaTachikuiyakiOnAutoPackedGridPosition");
    }

    public void TransformUnwrapMapOnlyImariAritaOnSpecificIndexPosition()
    {
        SendOsc("/TransformUnwrapMapOnlyImariAritaOnSpecificIndexPosition");
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
}