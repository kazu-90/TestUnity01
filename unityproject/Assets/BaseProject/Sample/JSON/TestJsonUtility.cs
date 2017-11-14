using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TestJsonUtility : MonoBehaviour {
	/*
	{
	 	"id":100,
	 	"name":"テストアイテム",
	 	"description":"説明だよ",
	 	"color":{  
	 		"id":1,
	 		"name":"black"
	 	},
	 	"list":[{
	 		"id":1
	 	},
	 	{
	 		"id":2
	 	}
	 	]
	}
	*/

	[Serializable]
	public class Item5 {
		public int id;
		public string name;
		public string description = "デフォルト値";
		public int dummy;
		public ItemColor color;
		public List<ItemList> list;
	}

	[Serializable]
	public class ItemColor {
		public string id;
		public string name;
	}

	[Serializable]
	public class ItemList {
		public string id;
	}


	string itemJson = "{ \"id\": 100, \"name\": \"テストアイテム\", \"description\": \"説明だよ\", \"color\": { \"id\" : 1, \"name\": \"black\" }, \"list\": [{ \"id\" : 1}, { \"id\" : 2}]  }";





	// Use this for initialization
	void Start () {
		/*{
			Item5 item5 = JsonUtility.FromJson<Item5>(itemJson);

			Debug.Log("itemJson " + itemJson);
			Debug.Log("item5 id " + item5.id);
			Debug.Log("item5 name " + item5.name);
			Debug.Log("item5 dummy " + item5.dummy);
			Debug.Log("item5 color " + item5.color);
			if(item5.color != null){
				Debug.Log("color id : " + item5.color.id);
				Debug.Log("color name : " + item5.color.name);
			}
			Debug.Log("item5 list " + item5.list);
			if(item5.color != null) {
				Debug.Log("list[0] id" + item5.list[0].id);
			}
			Debug.Log("item5 description " + item5.description);


			// Json文字列 -> List<T>
			List<ItemList> enemies = JsonUtility.FromJson<Serialization<ItemList>>(itemJson).ToList();
			Debug.Log("item5 list " + enemies);
		}*/




		var obj= Resources.Load("Text/jsontext") as TextAsset;
		string loadjson = obj.text;
		Debug.Log("rload : " + loadjson);

		Data_Common<Data_Login> dataCommon = JsonUtility.FromJson<Data_Common<Data_Login>>(loadjson);
		dataCommon.DrawLog();
		dataCommon.result.DrawLog();



		//object data = JsonUtility.FromJson<object>(loadjson);
		//Debug.Log(data.ToString());


		/*
		string json = "{\"code\":\"0\",\"maintenance\":0,\"serverTime\":\"2017/10/23 14:40:11\",\"downloadFlg\":\"0\",\"description\":\"説明だよ\",\"result\":{\"userID\":\"400000352\",\"password\":\"あいうえお\",\"accessToken\":\"\"}}";

		Data_Common dataCommon2 = JsonUtility.FromJson<Data_Common>(json);
		dataCommon2.DrawLog();
		*/



		var orgTest = new TestClass();
		orgTest.Log();

		// Json化
		string jsonStr = JsonUtility.ToJson(orgTest);
		Debug.Log("jsonStr:" + jsonStr);

		// Jsonから復元
		var newTest = JsonUtility.FromJson<TestClass>(jsonStr);
		newTest.Log();
	}




	[Serializable]
	public class Data_Common<T> {
		public int code;
		public string serverTime;
		public int downloadFlg;
		public string description;
		//public object result;
		public List<string> strList;
		public T result;


		public void DrawLog(){
			Debug.Log("Data_C code : " + code);
			Debug.Log("Data_C serverTime : " + serverTime);
			Debug.Log("Data_C downloadFlg : " + downloadFlg);
			Debug.Log("Data_C description : " + description);
			Debug.Log("Data_C result : " + result);
			Debug.Log("Data_C strList : " + strList.Count);
		}
	}

	[Serializable]
	public class Data_Login {
		public int userID;
		public string password;
		public string accessToken;
		[SerializeField] public List<int> ids;
		[SerializeField] public List<int> ids2;
		[SerializeField]public List<string> strList;

		public void DrawLog() {
			Debug.Log("Data_L userID : " + userID);
			Debug.Log("Data_L password : " + password);
			Debug.Log("Data_L ids : " + ids.Count);
			Debug.Log("Data_L ids2 : " + ids2.Count);
			Debug.Log("Data_L strList : " + strList.Count);
		}


		public List<int> ToList() { return ids; }

		/*public Data_Login(List<int> target) {
			this.target = target;
		}*/
	}




	public class TestClass : ISerializationCallbackReceiver {
		public string str;
		public int num;
		public List<string> strList;
		public TestClass2 test2;
		public Dictionary<int, string> dic;

		// Dictionaryシリアライズ化用List
		public List<int> dicKeys;
		public List<string> dicVals;

		public TestClass() {
			str = "テスト";
			num = 10;
			strList = new List<string> { "陸", "海", "空" };
			test2 = new TestClass2();
			dic = new Dictionary<int, string>();
			dic.Add(1, "dic1");
			dic.Add(2, "dic2");
			dic.Add(3, "dic3");

			dicKeys = new List<int>();
			dicVals = new List<string>();
		}

		public void Log() {
			string logStr = "str:" + str + " num:" + num;
			for(int i = 0; i < strList.Count; i++) {
				logStr += " list" + i + ":" + strList[i];
			}
			logStr += " test2.str2:" + test2.str2 + " test2.num2" + test2.num2;
			foreach(var kvp in dic) {
				logStr += " " + kvp.Key + ":" + kvp.Value;
			}
			Debug.Log(logStr);
		}

		/// <summary>
		/// シリアライズ前イベント
		/// </summary>
		public void OnBeforeSerialize() {
			// DictionaryをListとして保存
			dicKeys.Clear();
			dicVals.Clear();
			foreach(var kvp in dic) {
				dicKeys.Add(kvp.Key);
				dicVals.Add(kvp.Value);
			}
		}

		/// <summary>
		/// デシリアライズ後イベント
		/// </summary>
		public void OnAfterDeserialize() {
			// ListからDictionaryへ復元
			dic = new Dictionary<int, string>();
			for(int i = 0; i < Math.Min(dicKeys.Count, dicVals.Count); i++) {
				dic.Add(dicKeys[i], dicVals[i]);
			}
		}
	}

	/// <summary>
	/// Json化したいクラスに含まれるクラス
	/// </summary>
	[Serializable]
	public class TestClass2 {
		public string str2;
		public int num2;

		public TestClass2() {
			str2 = "テスト2";
			num2 = 20;
		}
	}
}
