/*
 * 	@赛车的移动控制
 *  @by lck
 *  @2018/3/13
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour{   
	
	// 轮子类
	[System.Serializable]
	public class AxleInfo
	{
		public WheelCollider leftWheel;
		public WheelCollider rightWheel;
		public bool motor;		
		public bool steering;
	}
	// 轮轴
	public List<AxleInfo> axleinfos;
	// 马力/最大马力
	private float motor = 0;
	public float maxMotorTorque;
	// 制动/最大制动
	private float brakeTorque = 0;
	public float maxBrakeTorque = 300;
	// 转向角/最大转向角
	private float steering = 0;
	public float maxSteeringAngle;
    // 加一个档位控制（key,value是档位的对应的速度上限）
    // private Dictionary<int, int> Stalls = new Dictionary<int, int>();
    // private Dictionary<int,long> Stalls_motor = new Dictionary<int, long>(); 
    // private int stall;
	// 音效模块
	[SerializeField] private AudioSource _soundSource;	// 音源物体
	[SerializeField] private AudioClip _soundMotor;		// 引擎音效
	[SerializeField] private AudioClip _soundBrake;		// 刹车音效
	// 轮子
	private Transform wheels;   // 换一种实现方式
    public Transform flWheel;
    public Transform frWheel;
    public Transform blWheel;
    public Transform brWheel;
 
	// AI判断
	public bool isAI = false;
	private AI ai;
	// 汽车刚体
	private Rigidbody rb;
    // 汽车速度
    public float speed;
    // 汽车刹车痕迹
    public List<TrailRenderer> Trail;
    //Y轴上的重心
    public float centerOfMassY = 0;

	// 初始化数据
	void Start () 
	{
		// 判断是否为AI
		if(isAI)
		{
			Debug.Log ("我是AI！");
			ai = gameObject.AddComponent<AI> ();
			ai.car = this;
		}
		// 获取轮子
		wheels = transform.Find ("Wheels");
		// 获取刚体
		rb = this.transform.GetComponent<Rigidbody> ();
		// 获取声音
		_soundSource = gameObject.GetComponent<AudioSource> ();
		_soundSource.spatialBlend = 1;		// 空间声音
        // 初始不出现拖尾
	    SetTrailRender(false);
        //重置赛车刚体的重心
        /// GetComponent<Rigidbody>().centerOfMass = new Vector3(0, centerOfMassY, 0);   // 这个重心调整有问题，跟碰撞体有关系
	}

	// 不定帧更新
	void Update()
	{
		ComputerCtrl ();
		PlayerCtrl ();
		// 遍历车轴
		foreach(AxleInfo axleInfo in axleinfos)
		{
			// 转向
			if(axleInfo.steering)
			{
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			// 马力
			if(axleInfo.motor)
			{
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
			if(true)
			{
				axleInfo.leftWheel.brakeTorque = brakeTorque;
				axleInfo.rightWheel.brakeTorque = brakeTorque;
			}
			// 旋转
			/*if(axleInfo.rightWheel != null && axleInfo.leftWheel != null)
			{
                WheelRotation(axleInfo.leftWheel);     
                WheelRotation(axleInfo.rightWheel);
			}*/
		}
	    RotateWheels();
	    SteelWheels();
		motorSound ();
        // 在视图中查看速度
	    speed = GetSpeed();
	}
		
    // 刹车痕迹，这个痕迹实际上是有问题的。true表示开启，false表示关闭
    public void SetTrailRender(bool flag)
    {
        if(Trail.Count == 0)
            return;
        if (flag)
        {
            foreach (var trail in Trail)
            {
                trail.enabled = true;
            }
        }
        else
        {
            foreach (var trail in Trail)
            {
                trail.enabled = false;
            }
        }
    }

    // 下压力，防侧翻
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(0, 0, 1200.0f);
        // 如果速度大于30，实际上更加好的是有一个速度对应的函数
        if (speed >= 30)
         {
            float z_force = 2000.0f;
            GetComponent<Rigidbody>().AddForce(0,0,z_force);
            // Debug.Log("加下压力防侧翻");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            // 氮气加速
            GetComponent<Rigidbody>().AddForce(20000f, 0f, 0f);
        }
    }

	/// 汽车当前的速度
	public float GetSpeed()
	{
		var speed = rb.velocity.magnitude;
		return speed;
	}

    // 发送位置信息
    void Send()
    {
        // 消息
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString("UpdateInfo");
        proto.AddFloat(this.transform.position.x);
        proto.AddFloat(this.transform.position.y);
        proto.AddFloat(this.transform.position.z);
        NetMgr.srvConn.Send(proto);

    }

    void RotateWheels()
    {   
        //车轮的转角速度
        if (flWheel == null)
            return;
        flWheel.Rotate(axleinfos[0].leftWheel.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        frWheel.Rotate(axleinfos[0].rightWheel.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        blWheel.Rotate(axleinfos[1].leftWheel.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        brWheel.Rotate(axleinfos[1].rightWheel.rpm / 60 * 360 * Time.deltaTime, 0, 0);
    }

    void SteelWheels()
    {   
        //赛车两个前轮在转弯时的角度
        if (flWheel == null)
            return;
        flWheel.localEulerAngles = new Vector3(flWheel.localEulerAngles.x, axleinfos[0].leftWheel.steerAngle -
                                               flWheel.localEulerAngles.z, flWheel.localEulerAngles.z);
        frWheel.localEulerAngles = new Vector3(frWheel.localEulerAngles.x, axleinfos[0].rightWheel.steerAngle -
                                               frWheel.localEulerAngles.z, frWheel.localEulerAngles.z);
    }
    

	/// 轮子旋转，实际效果看不出来。这个旋转函数有问题
	public void WheelRotation(WheelCollider collider)
	{
		if (wheels == null)
			return;
		// 获取旋转信息
		Vector3 pos;
		Quaternion rotation;
		collider.GetWorldPose (out pos, out rotation);
		// 遍历旋转每个轮子
		foreach(Transform wheel in wheels)
		{
			wheel.rotation = rotation;
		}
	}

	// 玩家控制（主要是控制刹车，加速和转向）
	public void PlayerCtrl()
	{
		// 如果是AI，则不受玩家控制
		if (!isAI)
		{
            motor = maxMotorTorque * -Input.GetAxis("Vertical");
		    steering = maxSteeringAngle * Input.GetAxis ("Horizontal");
			brakeTorque = 0;
		}
	    if (motor != 0 || steering != 0)
	    {
            // 发送位置信息（后期可以加同步）
            // Send(); 这里有一个思路的问题，如果还需要将游戏发展下去。就考虑这个问题。
	    }
		foreach(AxleInfo axlenlnfo in axleinfos)
		{
			if(axlenlnfo.leftWheel.rpm > 5 && motor < 0)  // 前进时，按后退键。这里实际上有一个设计问题，就是我们一般玩的车是没有倒挡的。自动默认后退
			{
				brakeTorque = maxBrakeTorque;
				// brakeSound ();
                // 拖尾效果处理
                SetTrailRender(true);
			}
			else if(axlenlnfo.leftWheel.rpm < -5 && motor > 0)	// 后退时，按下上键
			{
				brakeTorque = maxBrakeTorque;
				// brakeSound ();   这个声音太刺耳了，减掉一个
                // 拖尾效果处理
                SetTrailRender(true);
			}
			else
			{
                SetTrailRender(false);
			}
			continue;
		}
        // 紧急暂停
	    if (Input.GetKey(KeyCode.LeftShift))
	    {
	        rb.velocity = new Vector3(0,0,0);
	    }

	}

	// 引擎音效
	public void motorSound()
	{
		if (!_soundSource)
			return;
		if(motor != 0 && !_soundSource.isPlaying)
		{
			_soundSource.loop = true;
			_soundSource.clip = _soundMotor;
			_soundSource.Play ();
		}
		else if(motor == 0)
		{
			_soundSource.Pause ();
		}
	}

	// 刹车音效
	public void brakeSound()
	{
		_soundSource.loop = true;
		_soundSource.clip = _soundBrake;
		_soundSource.Play ();
	}

	// 电脑控制
	public void ComputerCtrl()
	{
		// 移动
	    if (!isAI || !Walk.instance.is_Start)
	        return;

		steering = ai.GetSteering ();
		motor = ai.GetMotor ();
		brakeTorque = ai.GetBrakeTorque ();
	}

}