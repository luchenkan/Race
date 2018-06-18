/*
 * 	@2018/04/20
 *  @by lck
 *  @赛车的终点判断
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class Final : MonoBehaviour
{
	// Dictory记录当前的位置
	Dictionary<string,bool> dict = new Dictionary<string,bool>();
	// 终点判定
	private bool isFinal = false;
    // 终点时间
    public GameObject time;
	public string finalTime;
	
	void Start ()
	{
		dict.Clear ();
        dict.Add("plane1", false);
        dict.Add("plane2", false);
        dict.Add("plane3", false);
        dict.Add("plane4", false);
        dict.Add("plane5", false);
        time = GameObject.Find("time_txt");
	}

	void Update()
	{
		// NextScene ();
		// IsFinal ();
	}

	// 这个是在物理update里面进行的FixedUpdate
	public void OnTriggerEnter(Collider other)
	{
		// 检测输出碰撞的物体
		Debug.Log ("Item collected : " + other);
		// 检测撞到的是否为终点标识物
		if(other.tag == "plane1" || other.tag == "plane2" || other.tag == "plane3" || other.tag == "plane4" || other.tag == "plane5")
		{
			if (dict [other.tag])
			{
				dict [other.tag] = false;
			}
			else
			{
				dict [other.tag] = true;
			}
		}
		IsFinal ();
	}

	// 判定是否到了终点
	public void IsFinal()
	{
		int count = 0;
		foreach(bool value in dict.Values)
		{
			if (value == false)
			{
				isFinal = false;		// 没有到终点
			} 
			else
			{
				++count;
			}
		}
		// 到达了终点
		if (count == 5)
		{
			isFinal = true;
			// Invoke("NextScene",2.0f); 
            // 记录当前的时间
		    finalTime = time.GetComponent<Text>().text;
            Debug.Log("到达了终点" + finalTime);
            PanelMgr.instance.OpenPanel<InterludeTitle>("");
			//ConnStart();  
            // 向服务端发送插入时间请求
            Send();

		}
		Debug.Log ("终点值：" + count);
	}
	public void NextScene()
	{
		SceneManager.LoadScene ("Race_2");
	}
	
	// 发送时间数据
    public void Send()
    {
        // 构建协议
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("InsertLeaderBoard");
        protocol.AddString(finalTime);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnInsertLeaderBoardBack);
    }

    // 回调函数
    private void OnInsertLeaderBoardBack(ProtocolBase ProtocolBytes)
    {
        ProtocolBytes proto = (ProtocolBytes) ProtocolBytes;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            Debug.Log("插入成功");
        }
        else
        {
            Debug.Log("插入失败");
        }
    }

	/*
	private IEnumerator NextScene()
	{
		Debug.Log ("进入协程");
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene ("Race_2");
	}
	*/

	// 打开数据库
    /*
	public void ConnStart()
	{
		// 数据库开启
		string connStr = "SslMode = none;server=127.0.0.1;port=3306;User Id=root;password=3347689; Database=msgboard;";
        sqlConn = new MySqlConnection(connStr);
		try
		{
			sqlConn.Open();
			Debug.Log("[数据库]已连接");
            // 这里直接进行数据的插入
            HandleMsg(finalTime);
		}
		catch (Exception e)
		{
			Console.WriteLine("[数据库]连接失败"+ e.Message + "\n");
			throw;
		}
	}

	// 处理数据库(这里暂时只有一个默认的插入功能)
	public void HandleMsg(string time)
	{
		string cmdStr = string.Format("insert into time(Time) values('{0}')",time);
        MySqlCommand cmd = new MySqlCommand(cmdStr, sqlConn);
		try
		{
			cmd.ExecuteNonQuery();
			Debug.Log("数据已插入");
		}
		catch(Exception e)
		{
			Console.WriteLine("[数据库]插入失败" + e.Message);
		}
	}*/
}