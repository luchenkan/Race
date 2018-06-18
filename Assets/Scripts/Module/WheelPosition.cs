/*
 * @��̥��������
 * @by lck
 * @2018/05/18
 */
using UnityEngine;
using System.Collections;

public class WheelPosition : MonoBehaviour {
    // ��̥��ײ��
    public WheelCollider WheelCollider;
    // ���ƶ�λ��
    private Vector3 newPos;


    void Update () 
	{
        // �洢��ײ��Ϣ
	    RaycastHit hit;
        // �ֱ��Ӧ:���ߵķ���㣬���ߵķ���RaycastHit,����
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
