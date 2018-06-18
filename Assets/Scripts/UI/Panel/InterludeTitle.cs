/*
 *  @过场面板
 *  @by lck
 *  @2018/04/25
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InterludeTitle: PanelBase {

	private Button continueBtn;		// 继续游戏按钮
	private Button restartBtn;		// 重新开始游戏按钮
	private Button exitBtn;			// 退出游戏按钮
	private GameObject playerCar;	// 赛车
	private string nowScene;		// 当前活跃场景名称

	void Start()
	{
		nowScene = SceneManager.GetActiveScene ().name;
		playerCar = GameObject.Find ("PlayerCar");
		playerCar.SetActive (false);
	}

	#region 生命周期
	public override void Init(params object[] args)
	{
		base.Init (args);
		skinPath = "InterludeTitle";
		layer = PanelLayer.Panel;
	}

	public override void OnShowing()
	{
		base.OnShowing ();
		Transform skinTrans = skin.transform;
		// 按钮赋值
		continueBtn = skinTrans.Find ("ContinueBtn").GetComponent<Button>();
		restartBtn = skinTrans.Find ("RestartBtn").GetComponent<Button>();
		exitBtn = skinTrans.Find ("ExitBtn").GetComponent<Button>();
		// 按钮绑定事件
		continueBtn.onClick.AddListener (OnContinueClink);
		restartBtn.onClick.AddListener (OnRestartClink);
		exitBtn.onClick.AddListener (OnExitClink);
	}
	#endregion

	public void OnRestartClink()
	{
		// 重新开始
		SceneManager.LoadScene (nowScene);
		PanelMgr.instance.ClosePanel ("InterludeTitle");
        playerCar.SetActive(true);
        
        // 如果这里重新开始，需要做一些准备。
        // 1.把那个路点全部还原。2.关闭面板后，显示赛车。是不是应该DeletePlayer  
	}

	public void OnContinueClink()
	{
		// 这个继续关卡按照逻辑应该是以数字为基准
        // 当前的完成程度，这个地方实际上不用显示，因为下一关没有完成。
		// SceneManager.LoadScene ("Race_2");
		PanelMgr.instance.ClosePanel ("InterludeTitle");
        playerCar.SetActive(true);
	}

	public void OnExitClink()
	{
		// 退出游戏
		Application.Quit ();
	}
}