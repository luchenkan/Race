/*
 *	@2018/04/15 
 *  @by lck
 *  @UI面板调用
 */
using UnityEngine;
using System.Collections;

public class Root : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        PanelMgr.instance.OpenPanel<LoginPanel>("");
    }

    void Update()
    {
        NetMgr.Update();
    }
}