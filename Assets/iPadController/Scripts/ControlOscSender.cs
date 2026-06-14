using UnityEngine;
using OscJack;
//using UnityEditor.PackageManager;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class ControlOscSender : MonoBehaviour
{
    public string IPAddress = "127.0.0.1";
    public int    OscPort = 6666;

    OscClient _oscClient;

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
    public 徳利タイプ     TokkuriType   = 徳利タイプ.すべて;
    public モデル表示形式 ModelViewMode = モデル表示形式.通常3Dモデル;
    public 並べ方         Alignment     = 並べ方.指定の位置で表示;
    public 表示モード     ViewMode      = 表示モード.素面;

    [Header("Toggle References")]
    [Header("TokkuriType")]
    [SerializeField]
    Toggle ToggleTokkuriType_All;
    [SerializeField]
    Toggle ToggleTokkuriType_TakadaMinou;
    [SerializeField]
    Toggle ToggleTokkuriType_TanbaTachikui;
    [SerializeField]
    Toggle ToggleTokkuriType_Arita;

    [Header("ModelViewMode")]
    [SerializeField]
    Toggle ToggleModelViewMode_Default;
    [SerializeField]
    Toggle ToggleModelViewMode_UnwrapMap;

    [Header("AlignmentMode")]
    [SerializeField]
    Toggle ToggleAlignmentMode_Default;
    [SerializeField]
    Toggle ToggleAlignmentMode_Random;

    [Header("ViewMode")]
    [SerializeField]
    Toggle ToggleViewMode_Shirafu;
    [SerializeField]
    Toggle ToggleViewMode_Meitei;



    private void Start()
    {
        _oscClient = new OscClient(IPAddress, OscPort);

    }

    public void TogglePressed()
    {
        if (ToggleTokkuriType_All != null)
            if (ToggleTokkuriType_All.isOn)
                TokkuriType = 徳利タイプ.すべて;
        if (ToggleTokkuriType_TakadaMinou != null)
            if (ToggleTokkuriType_TakadaMinou.isOn)
                TokkuriType = 徳利タイプ.高田美濃焼;
        if (ToggleTokkuriType_TanbaTachikui != null)
            if (ToggleTokkuriType_TanbaTachikui.isOn)
                TokkuriType = 徳利タイプ.丹波立杭焼;
        if (ToggleTokkuriType_Arita != null)
            if (ToggleTokkuriType_Arita.isOn)
                TokkuriType = 徳利タイプ.有田焼系;

        if (ToggleModelViewMode_Default != null)
            if (ToggleModelViewMode_Default.isOn)
                ModelViewMode = モデル表示形式.通常3Dモデル;
        if (ToggleModelViewMode_UnwrapMap != null)
            if (ToggleModelViewMode_UnwrapMap.isOn)
                ModelViewMode = モデル表示形式.円筒展開平面;

        if (ToggleAlignmentMode_Default != null)
            if (ToggleAlignmentMode_Default.isOn)
                Alignment = 並べ方.指定の位置で表示;
        if (ToggleAlignmentMode_Random != null)
            if (ToggleAlignmentMode_Random.isOn)
                Alignment = 並べ方.ランダムな並びで表示;


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
                    // やらないほうがよいかも？
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
                    // 変更必要
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
                    // 変更必要
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
                    // 変更必要
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
                    // 変更必要
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
                    // 変更必要
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
                    // 変更必要
                    TransformUnwrapMapOnlyImariAritaOnAutoPackedGridPosition();
                }
            }
        }
    }

    public void TogglePressedViewMode()
    {
        if (ToggleViewMode_Shirafu != null)
            if (ToggleViewMode_Shirafu.isOn)
                ViewMode = 表示モード.素面;
        if (ToggleViewMode_Meitei != null)
            if (ToggleViewMode_Meitei.isOn)
                ViewMode = 表示モード.酩酊;

        if (ViewMode == 表示モード.素面)
        {
            _oscClient.Send("/EnableEmission", 1);
        }
        else if (ViewMode == 表示モード.酩酊)
        {
            _oscClient.Send("/DisableEmission", 1);
        }
    }


    public void TransformDefault()
    {
        _oscClient.Send("/TransformDefault", 1);
    }

    public void TransformUnwrapMap()
    {
        _oscClient.Send("/TransformUnwrapMap", 1);
    }

    public void TransformDefaultOnSpecificIndexPosition()
    {
        _oscClient.Send("/TransformDefaultOnSpecificIndexPosition", 1);
    }

    public void TransformUnwrapMapOnSpecificIndexPosition()
    {
        _oscClient.Send("/TransformUnwrapMapOnSpecificIndexPosition", 1);
    }

    public void TransformDefaultOnRandomPosition()
    {
        _oscClient.Send("/TransformDefaultOnRandomPosition", 1);
    }

    public void TransformUnwrapMapOnRandomPosition()
    {
        _oscClient.Send("/TransformUnwrapMapOnRandomPosition", 1);
    }

    public void TransformDefaultOnlyMinouOnAutoPackedGridPosition()
    {
        _oscClient.Send("/TransformDefaultOnlyMinouOnAutoPackedGridPosition", 1);
    }

    public void TransformDefaultOnlyTanbaTachikuiyakiOnAutoPackedGridPosition()
    {
        _oscClient.Send("/TransformDefaultOnlyTanbaTachikuiyakiOnAutoPackedGridPosition", 1);
    }

    public void TransformDefaultOnlyImariAritaOnAutoPackedGridPosition()
    {
        _oscClient.Send("/TransformDefaultOnlyImariAritaOnAutoPackedGridPosition", 1);
    }

    public void TransformUnwrapMapOnlyMinouyakiOnAutoPackedGridPosition()
    {
        _oscClient.Send("/TransformUnwrapMapOnlyMinouyakiOnAutoPackedGridPosition", 1);
    }

    public void TransformUnwrapMapOnlyTanbaTachikuiyakiOnAutoPackedGridPosition()
    {
        _oscClient.Send("/TransformUnwrapMapOnlyTanbaTachikuiyakiOnAutoPackedGridPosition", 1);
    }

    public void TransformUnwrapMapOnlyImariAritaOnAutoPackedGridPosition()
    {
        _oscClient.Send("/TransformUnwrapMapOnlyImariAritaOnAutoPackedGridPosition", 1);
    }

    /*
    public void EnableEmission()
    {
        _oscClient.Send("/EnableEmission", 1);
    }

    public void DisableEmission()
    {
        _oscClient.Send("/DisableEmission", 1);
    }
    */
}
