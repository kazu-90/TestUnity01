using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	Vector3 m_Velocity;     // 速度（物体がどれだけ移動したか）
	Vector3 m_Position;     // 位置
	Vector3 m_Acceleration; // 加速度
	Transform m_Transfrom;
	float angle;

	private void Awake() {
		m_Transfrom = transform;
		m_Position = m_Transfrom.position;
		angle = 90.0f;

		m_Velocity = new Vector3(5.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		m_Acceleration = Vector3.zero;


		var radian = angle * Mathf.PI / 180.0f;
		m_Position.x = 6.0f * Mathf.Cos(radian);
		m_Transfrom.position = m_Position;
		angle += 30.0f * Time.deltaTime;
		if(angle >= 360.0f){
			angle = 0.0f;
		}
	}
}
