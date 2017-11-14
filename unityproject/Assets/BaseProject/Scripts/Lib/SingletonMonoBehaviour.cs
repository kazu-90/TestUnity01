/*!
 * シングルトンので動くコンポーネント用
 * 
 * @file	SingletonMonoBehaviour.cs
 * @author	k-fujiya
 * @date	2017/7/14
 */
using UnityEngine;
using System.Collections;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T	s_Instance = null;

	public static T Instance { get {
		if (s_Instance == null) {
			s_Instance = (T)FindObjectOfType(typeof(T));
			if (s_Instance == null) {
				Debug.LogError(typeof(T) + " is nothing");
			}
		}
		return s_Instance;
	} }

	public static bool isInstance { get {
		if (null != s_Instance) {
			return true;
		}
		return false;
	} }

	protected void Awake () {
		CheckInstance();
	}

	protected virtual void OnDestroy () {
		if (this == Instance) {
			s_Instance = null;
		}
	}

	protected bool CheckInstance () {
		if (this == Instance) {
			return true;
		}
		Destroy(this.gameObject);
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	// オブジェクト生成 .
	// @param  original - オリジナルオブジェクト .
	// @param  position - 初期座標 .
	// @param  rotation - 初期回転値 .
	// @return Object   - 生成したオブジェクト .
	//---------------------------------------------------------------------------------------------------
	public static Object WrapToInstantiate ( Object original, Vector3 position, Quaternion rotation ) {
		return Instantiate( original, position, rotation );
	}

	//---------------------------------------------------------------------------------------------------
	// オブジェクト生成 .
	// @param  original - オリジナルオブジェクト .
	// @return Object   - 生成したオブジェクト .
	//---------------------------------------------------------------------------------------------------
	public static Object WrapToInstantiate ( Object original ) {
		return Instantiate( original );
	}

}
