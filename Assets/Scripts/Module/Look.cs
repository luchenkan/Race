/*
 * 	@赛车的镜头跟随
 *  @by lck
 *  @2018/3/14
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour {
	public enum RotationAxes   // 旋转类型的常量
	{
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}
	public RotationAxes axes = RotationAxes.MouseXAndY;       // 为了在面板中方便调试
	public float followSpeed = 20.0f;		   // 镜头跟随速度
	public float sensitivityHor  = 9.0f;	   // 镜头的旋转速度（垂直和水平）
	public float sensitivityVert = 9.0f;
	public float minVert = -45.0f;             // 水平的旋转角度范围
	public float maxVert = 45.0f;
	public float maxDistance = 22.0f;		   // 镜头移动范围
	public float minDistance = 5.0f;
	public float zoomSpeed = 2f;			   // 镜头缩放速度
	public float distance = 60.0f;			   // 初始镜头距离

	private float _rotationX = 0;              // 初始默认旋转起点

	// 初始化
	void Start () 
	{
		// 都以在上面初始化完毕
	}
	
	// 每帧更新
	void Update () 
	{
		if (axes == RotationAxes.MouseX)    // 分表表示镜头的三种旋转情况
		{
			transform.Rotate (0, Input.GetAxis ("Mouse X") * sensitivityHor, 0);
		}
		else if (axes == RotationAxes.MouseY)		// 关键是在控制上下的时候，左右不能改变
		{
			_rotationX -= Input.GetAxis ("Mouse Y") * sensitivityVert;
			_rotationX = Mathf.Clamp (_rotationX, minVert, maxVert);   // 限制角度

			float rotationY = transform.localEulerAngles.y;

			transform.localEulerAngles = new Vector3 (_rotationX, rotationY, 0);
		}
		else if (axes == RotationAxes.MouseXAndY)
		{
			_rotationX -= Input.GetAxis ("Mouse Y") * sensitivityVert;
			_rotationX = Mathf.Clamp (_rotationX, minVert, maxVert);   // 限制角度

			float delta = Input.GetAxis ("Mouse X") * sensitivityHor;
			float rotationY = transform.localEulerAngles.y + delta;

			transform.localEulerAngles = new Vector3 (_rotationX, rotationY, 0);
		}
        
        // 镜头拉近效果，后来取消掉了。
        Zoom();
	}

	//  调整镜头距离
	void Zoom()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0)   		// 向下滚动
		{
			if (distance > minDistance)
			{
				distance -= zoomSpeed;
			    this.GetComponent<Camera>().fieldOfView = distance;
			}
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0)	// 向上滚动
		{
			if (distance < maxDistance)
			{
				distance += zoomSpeed;
                this.GetComponent<Camera>().fieldOfView = distance;
			}
		}
	}
		
}