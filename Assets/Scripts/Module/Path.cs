using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Path : MonoBehaviour {

	/// <summary>
	///  五个寻路点：（-160，,312，-82），（-615,312，-103），（-821,312,282），（-370,312,277)，（-116,312,32）
	/// 这个方法证明不行，失败了，赛车跑道的锅，有点急转，又有比较复杂的地方。
	/// 这个方法实际上不行。误差，物体的设置比较麻烦，而且准确度不高。
	/// </summary>

	// 所有路点
	private Vector3[] waypoints;
	// 当前的路点索引
	private int index = -1;
	// 当前的路点
	public Vector3 waypoint;
	// 是否循环
	public bool loop = false;
	// 是否完成
	public bool isFinish = false;
	// 到达误差
	public float deviation = 1;

	// 是否到达目的地
	public bool IsReach(Transform trans)
	{
		Vector3 pos = trans.position;
		float distance = Vector3.Distance (waypoint, pos);
		return distance < deviation;
	}

	// 下一个路点
	public void NextWayPoint() 
	{
		if (index < 0)
			return;
		// 处于最后第二个点
		if (index < waypoints.Length - 1)
		{
			++index;
		} else      		// 到达了终点
		{
			if (loop)		// 如果是循环的
				index = 0;
			else
				isFinish = true;
		}
		waypoint = waypoints [index];
	}

	// 控制场景标识物生成路点
	public void InitByObj(GameObject obj,bool isLoop = false)
	{
		int length = obj.transform.childCount;		// 这一招可以得到子物体数量
		if (length == 0)
		{
			waypoints = null;
			index = -1;
			Debug.LogWarning ("Path.InitByObj length = 0");
			return;
		}	
		waypoints = new Vector3[length];
		// 遍历物体的位置，给路点赋值
		for(var i = 0; i < length; ++i)
		{
			Transform trans = obj.transform.GetChild (i);
			waypoints [i] = trans.position;
		}
		// 重置开始参数
		index = 0;	// 当前路点也可以不为0（默认为0)
		waypoint = waypoints [index];
		this.loop = isLoop;
		isFinish = false;
	}

	// 还是用用Unity自带的寻路Navigation吧
	public void InitByMeshPath(Vector3 pos,Vector3 targetPos)
	{
		// 重置
		waypoints = null;
		index = -1;
		// 计算路径
		NavMeshPath navPath = new NavMeshPath ();
		bool hasFoundPath = NavMesh.CalculatePath (pos, targetPos, NavMesh.AllAreas, navPath);		// 计算路径
		if (!hasFoundPath)
			return;
		// 生成路径
		int length = navPath.corners.Length;			// 填充自身数据结构
		waypoints = new Vector3[length];
		for(int i = 0; i < length; ++i)
		{
			waypoints [i] = navPath.corners [i];
		}
		index = 0;
		waypoint = waypoints [index];
		isFinish = false;
	}

	// 调试路径
	/*
	public void DrawWaypoints()
	{
		if (waypoints == null)
			return;
		int length = waypoints.Length;
		for(int i = 0;i < length; ++i)
		{
			if(i == index)
			{
				Gizmos.DrawSphere (waypoints [i], 1);
			}
			else
			{
				Gizmos.DrawCube (waypoints [i], Vector3.one);
			}
		}
	}*/
}