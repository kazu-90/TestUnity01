using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
	[SerializeField]private BulletMove m_BulletObj;
	[SerializeField] Transform m_TargetTransform;


	private void Update() {
		if(Input.anyKeyDown){
			BulletMove bulletMove = Instantiate<BulletMove>(m_BulletObj);
			bulletMove.Init(m_TargetTransform);
		}
	}

}
