/*
 * @轮胎滚动方法
 * @by lck
 * @2018/05/18
 */
using UnityEngine;
using System.Collections;

public class WheelPosition : MonoBehaviour {
    // 轮胎碰撞器
    public WheelCollider WheelCollider;
    // 新移动位置
    private Vector3 newPos;


    void Update () 
	{
        // 存储碰撞信息
	    RaycastHit hit;
        // 分别对应:射线的发射点，射线的方向，RaycastHit,距离
	    if (Physics.Raycast(WheelCollider.transform.position, -WheelCollider.transform.up, out hit, WheelCollider.suspensionDistance + WheelCollider.radius))
		   
		    if (hit.collider.isTrigger)	
		    {               
                newPos =transform.position;
		    }	
		    else
		    {
		        newPos = hit.point + WheelCollider.transform.up * WheelCollider.radius;
		    }
    		
	    else
		    newPos = WheelCollider.transform.position - (WheelCollider.transform.up * WheelCollider.suspensionDistance);    		
	    
	    transform.position = newPos;
    }
}
