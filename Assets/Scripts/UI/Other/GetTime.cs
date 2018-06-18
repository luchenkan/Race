/*
 *  @游戏运行时间
 *  @by lck
 *  @2018/04/15
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GetTime : MonoBehaviour {

	// 赛车运行时间
	public int hour;
	public int minute;
	public int second;
	public int millisecond;

	// 当前时间已运行
	public float now_time;

	// UI组件
	public Text time_txt;

	// 初始化
	void Start () 
	{
		time_txt = GetComponent<Text> ();
	}
	
	// 更新
	void Update () 
	{
        // 游戏开始才更新
	    if (Walk.instance.is_Start)
	    {
	        now_time += Time.deltaTime;
	        hour = (int) now_time/3600;
	        minute = ((int) now_time - hour*3600)/60;
	        second = (int) now_time - hour*3600 - minute*60;
	        time_txt.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
	    }
	    else
	    {
	        // 不做处理
	    }
	}
}