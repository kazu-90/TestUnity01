using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
	[SerializeField]private BulletMove m_BulletObj;
	[SerializeField] Transform m_TargetTransform;

	float createIntervalTime = 1.5f;
	float progressTime;

	private void Awake() {
		progressTime = 0.0f;
	}


	private void Update() {
		//if(/*Input.anyKeyDown*/Input.anyKey){
		//	CreateBullet();
		//}

		progressTime -= Time.deltaTime;
		if(progressTime <= 0.0f){
			progressTime = createIntervalTime;

			for(int cnt = 0; cnt < 10; ++cnt){
				CreateBullet();
			}
		}

	}


	private void CreateBullet(){
		BulletMove bulletMove = Instantiate<BulletMove>(m_BulletObj);
		bulletMove.Init(m_TargetTransform);
	}
}