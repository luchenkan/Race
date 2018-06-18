using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginPanel : PanelBase
{
    private InputField idInput;
    private InputField pwInput;
    private Button loginBtn;
    private Button regBtn;

    #region 生命周期
    // 初始化
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "LoginPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        // 找到对应的按钮组件
        idInput = skinTrans.Find("IDInput").GetComponent<InputField>();
        pwInput = skinTrans.Find("PWInput").GetComponent<InputField>();
        loginBtn = skinTrans.Find("LoginBtn").GetComponent<Button>();
        regBtn = skinTrans.Find("RegBtn").GetComponent<Button>();

        // 绑定对应的按钮事件
        loginBtn.onClick.AddListener(OnLoginClick);
        regBtn.onClick.AddListener(OnRegClick);
    }
    #endregion

    // 登录
    public void OnLoginClick()
    {
        // 用户名密码为空
        if (idInput.text == "" || pwInput.text == "") // 前端校验，但是不知道是哪一个为空
        {
            Debug.Log("用户名密码不能为空");
        }

        if (NetMgr.srvConn.status != Connection.Status.Connected)           // 如果已经登录，会发生挤下去
        {
            string host = "127.0.0.1";
            int port = 1234;
            NetMgr.srvConn.proto = new ProtocolBytes();
            NetMgr.srvConn.Connect(host, port);
        }
        // 发送
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("Login");
        protocol.AddString(idInput.text);
        protocol.AddString(pwInput.text);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnLoginBack);         // 这里发送了消息，时间线跳转到Connection线。OnLoginBack是回调函数。如果服务器返回给你的是true，则调用回调函数。
    }

    public void OnLoginBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes) protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            Debug.Log("登录成功");
            // 开始游戏
            Walk.instance.StartGame(idInput.text);    // 实际上这里不对。。。应该是PanelMgr.instance.
            // PanelMgr.instance.OpenPanel<TitlePanel>("");
            Close();
        }
        else
        {
            Debug.Log("登录失败！");
            PanelMgr.instance.OpenPanel<TipPanel>("");
        }
    }

    // 注册
    public void OnRegClick()
    {
        PanelMgr.instance.OpenPanel<RegPanel>("");
        Close();
    }
    
}