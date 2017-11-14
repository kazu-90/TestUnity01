/*!
 * JSONデータを受け取って展開できるように処理するクラス
 * 
 * @file	DataBase.cs
 * @author	 fujiya
 * @date	2014/08/25
 * 
 * @author	 fujiya
 * @update	2015/01/23
 */
using System.Collections.Generic;

public class DataBase {

	private object						origin;
	private Dictionary<string, object>	hashTable;

	public object	data { get { return origin; } }


	public DataBase(object obj) {
		Init(obj);
	}
	
	public DataBase() {
	}

	public virtual void Init(object obj) {
		origin = obj;
		if (null == origin) {
			hashTable = null;
		} else {
			hashTable = origin as Dictionary<string, object>;
		}
	}

	protected bool FindHashKey(string key){
		return hashTable.ContainsKey(key) ? true : false;
	}

	public T getValue<T>(string name) {
		if (Contains(name)) {
			return DataGetterSample.GetDictData<T>(hashTable, name);
		} 
		//Debug.Log(name + " が見つかりませんでした。");
		return default(T);
	}

	//! 0, 1で入っているものを、boolで取得する用
	public bool getBool(string name) {
		var num = getValue<byte>(name);
		return System.Convert.ToBoolean(num);
	}

	public bool Contains(string name) {
		if (null == hashTable) {
			return false;
		}
		return hashTable.ContainsKey(name);
	}

	//! リスト作成ジェネリック関数
	public List<T> CreateList<T> (string name, System.Func<object, T> func = null) where T : DataBase, new() {
		var list = new List<T>();

		if (Contains(name)) {
			var objlist = getValue<List<object>>(name);

			if (objlist == null) {
				return null;
			}
			
			foreach (var obj in objlist) {
				T item = null;
				if(obj != null){
					if (null != func) {
						item = func(obj);
					} else {
						item = new T();
						item.Init(obj);
					}
				}
				list.Add(item);
			}
		}
		return list;
	}

	//! 文字列を表示
	public override string ToString () {
		return "";
		// MiniJSON 入れるまではコメントアウト
	//	return MiniJSON.Json.Serialize(hashTable);
	}

	//! カンマ区切りのコードを取得します
	public List<uint> GetCodes(string column) {
		var list = new List<uint>();

		string text = getValue<string>(column);
		if (!string.IsNullOrEmpty(text)) {
			var cds = text.Split(new char[]{','}, System.StringSplitOptions.RemoveEmptyEntries);
			foreach (var tmp in cds) {
				uint cd = 0;
				if (true == uint.TryParse(tmp, out cd)) {
					list.Add(cd);
				}
			}
		}

		return list;
	}

	public List<int> GetCodesInt(string column, int offset = 0)
	{
		var list = new List<int>();

		GetSplitCodes(column, (tmp) =>{
			int cd = 0;
			if (true == int.TryParse(tmp, out cd)){
				list.Add(cd + offset);
			}
		});
		return list;
	}

	public void GetSplitCodes(string column, System.Action<string> AddListCallback) {
		string[] cds = null;

		string text = getValue<string>(column);
		if (!string.IsNullOrEmpty(text)) {
			cds = text.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
		}
		if (cds != null && cds.Length != 0) {
			foreach (var tmp in cds) {
				AddListCallback (tmp);
			}
		}
	}

	public List<bool> GetCodesBool (string column) {
		var list = new List<bool> ();

		GetSplitCodes (column, (tmp) => {
			bool flg = tmp == "1" ? true : false;
			list.Add (flg);
		});
		return list;
	}

	//public List<T> GetCodesList<T>(string column) {
	//	var list = new List<T>();

	//	GetSplitCodes(column, (tmp) => {
	//		list.Add(ConvertData<T>(tmp));
	//	});
	//	return list;
	//}

	//T ConvertData<T>(object val)
	//{
	//	var type = typeof(T);
	//	if (type.IsGenericType) {
	//		// ジェネリック型なら、そのまま返す
	//		return (T)val;
	//	} else
	//	if (type.IsClass) {
	//		// クラス型ならそのまま返す
	//		return (T)val;
	//	}

	//	T def = default(T);
			
	//	return def;
	//}
}

