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
		Debug.Log("rload : " + obj.text);


		Data_Common dataCommon = JsonUtility.FromJson<Data_Common>(itemJson);
		dataCommon.DrawLog();


		string json = "{\"code\":\"0\",\"maintenance\":0,\"serverTime\":\"2017/10/23 14:40:11\",\"downloadFlg\":\"0\",\"description\":\"説明だよ\",\"result\":{\"userID\":\"400000352\",\"password\":\"あいうえお\",\"accessToken\":\"\"}}";


		Data_Common dataCommon2 = JsonUtility.FromJson<Data_Common>(json);
		dataCommon2.DrawLog();

	}




	[Serializable]
	public class Data_Common {
		public int code;
		public string serverTime;
		public int downloadFlg;
		public string description;
		public object result;


		public void DrawLog(){
			Debug.Log("Data_C code : " + code);
			Debug.Log("Data_C serverTime : " + serverTime);
			Debug.Log("Data_C downloadFlg : " + downloadFlg);
			Debug.Log("Data_C description : " + description);
			Debug.Log("Data_C result : " + result);
		}
	}


}
