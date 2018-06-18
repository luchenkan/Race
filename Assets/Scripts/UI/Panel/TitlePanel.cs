/*
 * 	@标题面板
 *  @by lck
 *  @2018/04/10
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitlePanel : PanelBase
{
	private Button startBtn;
	private Button infoBtn;
	private GameObject playerCar;

	#region 生命周期
	public override void Init(params object[] args)
	{
		base.Init(args);
		skinPath = "TitlePanel";
		layer = PanelLayer.Panel;
	}

	public override void OnShowing()
	{
        // 找到赛车并隐藏
        playerCar = GameObject.Find("PlayerCar");
        playerCar.SetActive(false);

		base.OnShowing();
		Transform skinTrans = skin.transform;
		startBtn = skinTrans.Find("StartButton").GetComponent<Button>();

		startBtn.onClick.AddListener(OnStartClick);
	}
	#endregion

	public void OnStartClick()
	{
		playerCar.SetActive (true);
		Close();
	}

}