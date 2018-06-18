/*
 * @2018/06/01
 * @by lck
 * @自动寻路
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
   
    public Transform Cube_mid;  
    public Transform Cube_fin;
    public Transform Cube_end;

    //private int index;
    //public Transform[] target;
    private NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        /*if (nav.remainingDistance <= 0.5)
        {
            ++index;
            nav.SetDestination(target[index].position);
        }
         */
        // Nav的初始默认位置是this的位置
        if(Walk.instance.is_Start && nav.destination == this.transform.position)
            nav.SetDestination(Cube_mid.position);
    }

    // 碰撞检测
    public void OnTriggerEnter(Collider otherCollider)
    {
        // 如果到达目标点
        // Debug.Log("item " + otherCollider);
        if (otherCollider.tag == "Cube_mid")
        {
            nav.SetDestination(Cube_fin.position);
        }
        // 这个地方为什么不用else？是因为会碰撞到其他的Collider。
        else if (otherCollider.tag == "Cube_fin")
        {
            nav.SetDestination(Cube_end.position);
        }
    }
    
}