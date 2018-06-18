/*
 *  @排行榜界面
 *  @by lck
 *  @2018/05/16
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPanel : PanelBase {

	private Button closeBtn;            // 关闭按钮

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "LeaderboardPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        // 按钮赋值
        closeBtn = skinTrans.Find("closeBtn").GetComponent<Button>();
        // 按钮绑定事件
        closeBtn.onClick.AddListener(OnCloseClink);
    }
    #endregion

    public void OnCloseClink()
    {
        // 关闭当前的界面Close();
        PanelMgr.instance.ClosePanel ("LeaderboardPanel");
    }
}