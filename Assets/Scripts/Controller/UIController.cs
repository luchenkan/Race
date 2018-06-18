/*
 * @游戏界面UI管理
 * @by lck
 * @2018/05/18
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
   
	[SerializeField] private Text Time;			    // 时间数字
	[SerializeField] private Button setting;	    // 设置按钮
	[SerializeField] private Button rank;		    // 排行榜按钮
	[SerializeField] private SettingPopup settingPopup;			// 设置窗口
    [SerializeField] private Button talkButton;     // 留言按钮
    [SerializeField] private Button turnButton;     // 翻转按钮
    private GameObject playerCar;	// 赛车

    // 存储数据
    public static List<string> list = new List<string>();
	void Start () 
	{
		settingPopup.Close ();	                    //	初始设置窗口不弹出
        rank.onClick.AddListener(OnLeaderBoardClick);
	}
	
    // 排行榜绑定事件
    public void OnLeaderBoardClick()
    {
        // 发送协议
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("LeaderBoard");
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnLeaderBoardBack);
    }

    // 设置按钮绑定事件
	public void OnOpenSetting()
	{
		Debug.Log("open Setting Button");
		settingPopup.Open ();
	}

    // 留言板按钮绑定事件
    public void OnOpenTalk()
    {
        Debug.Log("open Talk Button");
        // PanelMgr.instance.OpenPanel<Connect>("");
    }

    // 调整翻转的赛车为正常位
    public void OnOpenTurn()
    {
        playerCar = GameObject.Find("PlayerCar");
        if (playerCar == null)
            return;
        // 先获得当前的y的转向角
        float y = playerCar.GetComponent<Transform>().localEulerAngles.y;
        playerCar.GetComponent<Transform>().rotation = Quaternion.Euler(0, y, 0); 
    }

    // 排行榜回调函数
    public void OnLeaderBoardBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes) protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        string data = null;
        while (data != "")
        {
            data = proto.GetString(start, ref start);
            list.Add(data);
        }
        if (ret == 0)
        {
            // 检测一下传回的数据
            foreach (var VARIABLE in list)
            {
                Debug.Log(VARIABLE);
            }
            PanelMgr.instance.OpenPanel<LeaderboardPanel>("");
        }
        else
        {
            Debug.Log("查询失败");
        }
    }
}