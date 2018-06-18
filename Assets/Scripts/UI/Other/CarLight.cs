/*
 *  @车身灯光控制
 *  @by lck
 *  @2018/04/15
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarLight : MonoBehaviour {

	// 车身左右车灯
	private GameObject rLight;
	private GameObject lLight;
	// 灯光按钮
	[SerializeField]private GameObject Lbtn;
	[SerializeField]private Sprite press;		// 两张按钮贴图
	[SerializeField]private Sprite release;
	private bool isLight = true;    // 车灯默认开启
    private GameObject playerCar;   // 赛车

	void Start () 
	{
		if(release != null)
			Lbtn.GetComponent<Image>().sprite = release;
	}

    void Update()
    {
        playerCar = GameObject.Find("PlayerCar");
        if (playerCar == null)
            return;
        else if (playerCar != null && rLight == null)
        {
            rLight = GameObject.Find("RSpotlight");
            lLight = GameObject.Find("LSpotlight");
        }
    }

	// 操作灯
	public void SetLight()
	{
		if(isLight)
		{
			isLight = false;
			rLight.SetActive (false);
			lLight.SetActive (false);
			if(release != null)
				Lbtn.GetComponent<Image>().sprite = release;
		}
		else
		{
			isLight = true;
			rLight.SetActive (true);
			lLight.SetActive (true);
			if (press != null)
				Lbtn.GetComponent<Image>().sprite = press;
		}
	}
		
}