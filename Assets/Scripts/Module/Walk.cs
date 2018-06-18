/*
 * @维护消息队列
 * @by lck
 * @2015/05/10，25号修改
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

public class Walk : MonoBehaviour
{
    //预设
    public GameObject prefab;
    //players
    Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    //self
    string playerID = "";
    //上一次移动时间
    public float lastMoveTime;
    //单例
    public static Walk instance;
    // 是否一开始游戏
    public bool is_Start = false;
    void Start()
    {
        instance = this;
    }

    //添加玩家
    void AddPlayer(string id, Vector3 pos, int score)
    {
        GameObject player = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        player.name = "PlayerCar";
        player.transform.localEulerAngles = new Vector3(0,107,0);           // 这是正对跑道方向
        players.Add(id, player);
    }

    //删除玩家
    void DelPlayer(string id)
    {
        //已经初始化该玩家
        if (players.ContainsKey(id))
        {
            Destroy(players[id]);
            players.Remove(id);
        }
    }


    //更新信息
    public void UpdateInfo(string id, Vector3 pos, int score)
    {
        //已经初始化该玩家
        if (players.ContainsKey(id))
        {
            players[id].transform.position = pos;
        }
        //尚未初始化该玩家
        else
        {
            AddPlayer(id, pos, score);
        }
    }

    public void StartGame(string id)
    {
        // 表示已开始游戏，用于隔断一些动作。
        is_Start = true;
        playerID = id;
        //诞生玩家
        Vector3 pos = new Vector3(445, -25, 283);
        AddPlayer(playerID, pos, 0);
        //同步
        SendPos();
        //获取列表
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString("GetList");
        NetMgr.srvConn.Send(proto, GetList);
        NetMgr.srvConn.msgDist.AddListener("UpdateInfo", UpdateInfo);
        NetMgr.srvConn.msgDist.AddListener("PlayerLeave", PlayerLeave);
    }

    //发送位置
    void SendPos()
    {
        GameObject player = players[playerID];
        Vector3 pos = player.transform.position;
        //消息
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString("UpdateInfo");
        proto.AddFloat(pos.x);
        proto.AddFloat(pos.y);
        proto.AddFloat(pos.z);
        NetMgr.srvConn.Send(proto);
    }

    //更新列表
    public void GetList(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        //获取头部数值
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int count = proto.GetInt(start, ref start);
        //遍历
        for (int i = 0; i < count; i++)
        {
            string id = proto.GetString(start, ref start);
            float x = proto.GetFloat(start, ref start);
            float y = proto.GetFloat(start, ref start);
            float z = proto.GetFloat(start, ref start);
            int score = proto.GetInt(start, ref start);
            Vector3 pos = new Vector3(x, y, z);
            UpdateInfo(id, pos, score);
        }
    }

    //更新信息
    public void UpdateInfo(ProtocolBase protocol)
    {
        //获取数值
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        string id = proto.GetString(start, ref start);
        float x = proto.GetFloat(start, ref start);
        float y = proto.GetFloat(start, ref start);
        float z = proto.GetFloat(start, ref start);
        int score = proto.GetInt(start, ref start);
        Vector3 pos = new Vector3(x, y, z);
        UpdateInfo(id, pos, score);
    }

    //玩家离开
    public void PlayerLeave(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        //获取数值
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        string id = proto.GetString(start, ref start);
        DelPlayer(id);
    }

}