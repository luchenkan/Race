using UnityEngine;
using System.Collections;
using System.Runtime.Remoting;
using UnityEngine.UI;

public class TipPanel : PanelBase
{
    // 填写的信息，Text后期可以扩展到一般的提示框。
    private Text text;
    private Button closeBtn;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "TipPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        closeBtn = skinTrans.Find("CloseBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(OnCloseClick);
    }
    #endregion

    public void OnCloseClick()
    {
        this.Close();
    }
}