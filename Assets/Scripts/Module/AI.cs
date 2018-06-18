/*
 *  @移动状态机的实现
 *  @by lck
 *  @2018/04/23
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

	// 备用
	public enum state
	{
		
	}

	// 控制的AI车
	public Move car;
	// 路径
	private Path path = new Path ();

	// 初始化路径
	void InitWayPoint()
	{
		GameObject obj = GameObject.Find ("Path_marker");
		// 人工寻路方法，先使用测试。
        /*
		if (obj)
		    path.InitByObj (obj);
		*/ 

		// 寻路插件寻址，更加的合理且使用unity相关技术。
		/*
		if(obj && obj.transform.GetChild(0) != null)
		{
			Vector3 targetPos = obj.transform.GetChild (0).position;
			path.InitByMeshPath (transform.position, targetPos);
		}
		*/
	}

	// 初始化
	void Start()
	{
		InitWayPoint ();
	}
		
	// 运动处理
	void Update()
	{
		// 这里可以调用状态的判断，暂时没有想到用的地方
        /*
		MoveStart ();
		if(path.IsReach(transform))
		{
			path.NextWayPoint ();
		}
        */
	}

	// 移动开始
	public void MoveStart()
	{
		// 这部分移动到Move类部分
	}

	// 获取转向角
	public float GetSteering()
	{
		if (car == null)
			return 0;

		Vector3 itp = transform.InverseTransformPoint (path.waypoint);		// 路点相对于车的距离
		if (itp.x > path.deviation / 5)
			return car.maxSteeringAngle;		// 右转
		else if (itp.x < -path.deviation / 5)
			return -car.maxSteeringAngle;		// 左转
		else
			return 0;		// 直行
	}

	// 获取马力值
	public float GetMotor()
	{
		if (car == null)
			return 0;

		Vector3 itp = transform.InverseTransformPoint (path.waypoint);
		float x = itp.x;
		float z = itp.z;
		float r = 10;

		if (z < 0 && Mathf.Abs (x) < -z && Mathf.Abs (x) < r)
			return -car.maxMotorTorque;
		else
			return car.maxMotorTorque;
	}

	// 获取刹车
	public float GetBrakeTorque()
	{
		if (path.isFinish)
			return car.maxMotorTorque;
		else
			return 0;
	}
		
}