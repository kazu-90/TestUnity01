/*!
 * データ取得クラス
 * 
 * @file	DataGetter.cs
 * @author	k-fujiya
 * @date	2017/11/14
 */
using System.ComponentModel;
using System.Collections.Generic;

public static class DataGetterSample {
	
	private static Dictionary<string, TypeConverter>	converterDict = new Dictionary<string, TypeConverter>() {
		{ typeof(byte).Name,    new ByteConverter()    },
		{ typeof(sbyte).Name,   new SByteConverter()   },
		{ typeof(int).Name,     new Int32Converter()   },
		{ typeof(uint).Name,    new UInt32Converter()  },
		{ typeof(short).Name,   new Int16Converter()   },
		{ typeof(ushort).Name,  new UInt16Converter()  },
		{ typeof(long).Name,    new Int64Converter()   },
		{ typeof(ulong).Name,   new UInt64Converter()  },
		{ typeof(float).Name,   new SingleConverter()  },
		{ typeof(double).Name,  new DoubleConverter()  },
		{ typeof(char).Name,    new CharConverter()    },
		{ typeof(bool).Name,    new BooleanConverter() },
		{ typeof(decimal).Name, new DecimalConverter() },
		{ typeof(System.DateTime).Name, new DateTimeConverter() },
	};

	// データ取得
	/// <summary>
	/// データ取得の使い方
	/// 
	/// ☆int 型の場合
	/// int temp = DataGetterSample.GetDictData<int>(data, "garden_data");
	/// ☆List<int> 型の場合
	/// List<int> tempList = DataGetterSample.GetDictData< List<int> >(data, "garden_data");
	/// 
	/// 第一引数: 取得元の Dictionary<string, object>
	/// 第二引数: 取得したいキー値
	/// 
	/// 指定されたキーを元に Dictionary<string, object> からデータを取得します。
	/// 指定されたキーが Dictionary に存在しない場合、
	/// エクセプション("does not contains key")を飛ばします。
	///	指定されたキーにより取得したデータが指定された型でキャスト出来なかった場合、
	///	エクセプション("invalid cast exception")を飛ばします。
	/// 
	/// </summary>

	public static T GetDictData<T> (Dictionary<string, object> dict, string key, T def = default(T)) {
		if (!dict.ContainsKey(key)) {
			return def;
		}

		var val  = GetDictDataToObj(dict, key);
		var type = typeof(T);
		if (type.IsGenericType) {
			// ジェネリック型なら、そのまま返す
			return (T)val;
		} else
		if (type.IsClass) {
			// クラス型ならそのまま返す
			return (T)val;
		}
		
		object ret = def;

		if (null == val) {
		} else if (type == typeof(string)) {
			// 文字列ならToStringして返す
			ret = val.ToString();
		} else if (type == typeof(UnityEngine.Color)) {
			UnityEngine.Color col = UnityEngine.Color.white;
			if (!UnityEngine.ColorUtility.TryParseHtmlString(val.ToString(), out col)) {
				ret = def;
			}
			else {
				ret = col;
			}
		} else if (converterDict.ContainsKey(type.Name)) {
			// コンバータを取り出して、パース
			ret = converterDict[type.Name].ConvertFromString(val.ToString());
		} else if (type.IsEnum) {
			// enum
			ret = System.Enum.Parse(type, val.ToString());
		} else {
			throw new System.SystemException("can't convert: from " + val.GetType().ToString() + " to " + typeof(T));
		}
		
		return (T)ret;
	}

	// Dictonaryからobject取得
	private static object GetDictDataToObj (Dictionary<string, object> dict, string key) {
		object retObj = null;
		if (dict.ContainsKey(key)) {
			retObj = dict[key];
		} else {
			throw new System.SystemException("does not contains key : " + key);
		}
		return retObj;
	}

	// パラムデータから指定のキーの情報を取り出す。キーがないときはdefを返す。
	public static T GetDicValue<T>(this Dictionary<string, object> _param, string _key, T _def) 
	{
		return DataGetterSample.GetDictData<T>(_param, _key, _def);
	}
}

