/*
 * @变化的天空
 * @by lck
 * @2018/05/17修改
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour {

	[SerializeField] private Material sky;			// 天空盒
	[SerializeField] private Light sun;				// 灯光

	private float _fullIntensity;
	private float _cloudValue = 0.0f;

	void Start ()
	{
        // 获取灯光强度值
		_fullIntensity = sun.intensity;
        // 重置天空状态，Blend代表混合值。
        sky.SetFloat("_Blend", 0);

	}
	
	void Update () 
	{
        // 不能无限制的暗下去
        if (_cloudValue <= 0.75)
		    SetOvercast (_cloudValue);		   // 持续过渡天气，每帧增加一定滑动条值
		    _cloudValue += .0005f;             // release的时候可以打开
	}

	// 同时调整天空的Blend值和灯光强度
	private void SetOvercast(float value)
	{
		sky.SetFloat ("_Blend", value);
		sun.intensity = _fullIntensity - (_fullIntensity * value);
	}
}