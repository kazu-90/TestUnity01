using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour {

	[SerializeField] Transform m_TargetTransform;

	Vector3 m_Velocity;		// 速度（物体がどれだけ移動したか）
	Vector3 m_Position;		// 位置
	Vector3 m_Acceleration;	// 加速度
	Transform m_Transfrom;

	private void Awake() {
		m_Transfrom = transform;
		m_Position = m_Transfrom.position;

		m_Velocity = new Vector3(5.0f, 0.0f, 0.0f);
	}


	public void Init(Transform target) {
		m_TargetTransform = target;
	}


	// Update is called once per frame
	void Update () {
		m_Acceleration = Vector3.zero;

		//m_Acceleration = new Vector3(0.0f, -9.8f, 0.0f);	// 重力

		bool success = HitAtTheFixedTime();

		if(success) {
			// 基本の式
			m_Velocity += m_Acceleration * Time.deltaTime;
			m_Position += m_Velocity * Time.deltaTime;
			m_Transfrom.position = m_Position;
		}
	}


	//ある物体が初速度v0から t秒後に速度v に達した時の加速度と移動距離
	// d:移動距離　v0:速度　t:時間　a:加速度
	// d = (v0 * t) + (1/2 * a * (t*t))
	//
	// 加速度を出すように変換
	// a = (2 * (d - v0 * t)) / (t*t)

	//決められた時間に着弾
	private float m_PeriodTime = 1.0f;
	bool HitAtTheFixedTime(){
		if(m_TargetTransform == null){
			return false;
		}

		var diff = m_TargetTransform.position - m_Position;
		Debug.Log(diff);
		m_Acceleration = (2.0f * (diff - m_Velocity * m_PeriodTime)) / (m_PeriodTime * m_PeriodTime);

		m_PeriodTime -= Time.deltaTime;
		if(m_PeriodTime < 0.0f){
			return false;
		}


		return true;
	}
}


