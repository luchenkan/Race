/*
 * @网络管理
 * @by lck
 * @2018/05/24修改
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//网络管理
public class NetMgr
{
    public static Connection srvConn = new Connection();

    public static void Update()
    {
        srvConn.Update();
    }

    //心跳
    public static ProtocolBase GetHeatBeatProtocol()
    {
        //具体的发送内容根据服务端设定改动
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("HeatBeat");
        return protocol;
    }
}