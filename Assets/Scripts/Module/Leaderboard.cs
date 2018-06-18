/*
 * @排行榜功能
 * @by lck
 * @2018/05/09
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Leaderboard : MonoBehaviour {

	// 需要有一个设置好的prefabs，用作copy
	public GameObject obj;
	// 数据库
	//public MySqlConnection sqlConn;
	// 存储数据容器
	// List<object> list = new List<object>();
    private List<string> list = UIController.list;
    // 父类
    private GameObject parent;
    // 长度等参数
    private Vector2 contentSize;
    private Vector3 itemLocalPos;
    private float itemHeight;

	void Start()
	{
		/*
		 * 计算需要的数量。
		 * 这个数量来自数据库查询，不过上限10个。
		 */
        // 找到父节点
	    parent = GameObject.Find("Content");         
	    contentSize = parent.GetComponent<RectTransform>().sizeDelta;

	    itemHeight = obj.GetComponent<RectTransform>().rect.height;
	    itemLocalPos = obj.transform.localPosition;
        // 设置容器的高度
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, 10 * itemHeight);
		for(var i = 0; i < 10; ++i)
		{
            // 添加项
		    AddItem(i);
		}
	}

    public void AddItem(int num)
    {
        GameObject a = Instantiate(obj) as GameObject;
        a.transform.Find("Text").GetComponent<Text>().text = list[num].ToString();
        // 设置位置（与父节点相同）
        a.GetComponent<Transform>().SetParent(parent.GetComponent<Transform>(),false);
        a.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - (num + 1) * itemHeight - 40, 0);
    }

	// 打开数据库
	/*public void ConnStart()
	{
		// 数据库开启
		string connStr = "SslMode = none;server=127.0.0.1;port=3306;User Id=root;password=3347689; Database=msgboard;";
		sqlConn = new MySqlConnection (connStr);
		try
		{
			sqlConn.Open();
			Debug.Log("[数据库]已连接");
		}
		catch(Exception e)
		{
			Console.WriteLine ("[数据库]连接失败" + e.Message + '\n');
		}
	}

	// 查询数据库
	public void HandleMsg()
	{
		string str;	
		string cmdStr = string.Format ("select * from time order by Time LIMIT 10");
		MySqlCommand cmd = new MySqlCommand (cmdStr, sqlConn);
		try
		{
			MySqlDataReader dataReader = cmd.ExecuteReader();
			str = "";
		    int rank = 0;
			while(dataReader.Read())
			{
			    ++rank;
				list.Add(rank + ". " +dataReader["Time"]);
			}
			// 关闭读取
			dataReader.Close();
		}
		catch(Exception e)
		{
			Console.WriteLine ("[数据库]查询失败");
		}
	}*/

}