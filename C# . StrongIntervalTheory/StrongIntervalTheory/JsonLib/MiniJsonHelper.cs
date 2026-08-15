using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace JsonLib
{
	public static class MiniJsonHelper
	{
		#region Serialize 

		public static string SerializeString(string obj)
		{
			return obj;
		}

		public static string SerializeObject(object obj) 
		{
			return Json.Serialize(obj);
		}

		public static string Serialize<T, Y>(T obj, Func<T, Y> func) 
		{
			return Json.Serialize(func(obj));
		}

		public static string Serialize<T>(this T obj) where T : ISerializable
		{
			var dict = new Dictionary<string, object>();
			obj.Serialize(dict);
			return Json.Serialize(dict);
		}

		public static void AddField<T, Y>(this Dictionary<string, object> dict, string key, T value, Func<T, Y> serialize)
		{
			dict.Add(key, serialize(value));
		}

		public static void Add<T, Y>(this Dictionary<string, object> dict, string key, T value, Func<T, Y> serialize)
		{
			dict.Add(key, serialize(value));
		}

		public static Func<IEnumerable<T>, Y[]> SerializeArray<T, Y>(Func<T, Y> serialize)
		{
			return list =>
			{
				if (list == null)
					return null;

				return list.Select(serialize).ToArray();
			};
		}

		public static Func<IEnumerable<T>, List<Y>> SerializeList<T, Y>(Func<T, Y> serialize)
		{
			return list => list.Select(serialize).ToList();
		}

		public static Func<Dictionary<K1, V1>, Dictionary<K2, V2>> SerializeDictionary<K1, K2, V1, V2>(Func<K1, K2> serializeKey, Func<V1, V2> serializeValue)
		{
			return list => list.ToDictionary(d => serializeKey(d.Key), d => serializeValue(d.Value));
		}

		public static T SerializeTrivial<T>(T obj)
		{
			return obj;
		}

		public static int SerializeTrivialInt(int obj)
		{
			return obj;
		}

		public static bool SerializeTrivialBool(bool obj)
		{
			return obj;
		}

		public static Dictionary<string, object> SerializeType<T>(T obj) where T : ISerializable
		{
			if (obj == null)
				return null;

			var dict = new Dictionary<string, object>();
			obj.Serialize(dict);
			return dict;
		}

		#endregion 

		#region Deserialize

		public static T Deserialize<T>(this string json)
			where T: IDeserializable, new ()
		{
			var t = new T();
			t.Deserialize(Json.Deserialize(json) as Dictionary<string, object>);
			return t;
		}

		public static T Deserialize<T>(this string json, Func<object, T> convert)
		{
			return convert(Json.Deserialize(json));
		}


		public static void Deserialize<T>(this Dictionary<string, object> dict, ref T variable, string param, Func<object, T> convert)
		{
			try
			{
				if (dict.ContainsKey(param) == false)
					variable = default(T);
				else
					variable = convert(dict[param]);
			}
			catch (Exception e)
			{
				throw new Exception(string.Format("Can not deserialize field '{0}'\n{1}", param, e));
			}
		}

		public static void Deserialize<T>(this Dictionary<string, object> dict, string param, Func<object, T> convert, Action<T> func)
		{
			if (dict.ContainsKey(param) != false)
				func(convert(dict[param]));
		}

		public static T Deserialize<T>(this Dictionary<string, object> dict, string param, Func<object, T> convert)
		{
			if (dict.ContainsKey(param) == false)
				return default(T);
			else
				return convert(dict[param]);
		}

		public static void Deserialize<T>(this Dictionary<string, object> dict, ref T variable, string param, Func<object, T> convert, T defaultValue)
		{
			if (dict.ContainsKey(param) == false)
				variable = defaultValue;
			else
				variable = convert(dict[param]);
		}

		public static object ConvertObject(object obj)
		{
			return obj;
		}

		public static int ConvertInt(object obj)
		{
			if (obj is string)
			{
				try
				{
					return int.Parse(obj as string, CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					throw new Exception("Can not parse to int: " + obj as string);
				}
			}
			else if (obj is int)
				return (int) obj;
			else if (obj is long)
				return (int) (long) obj;
			else
				throw new Exception("Convert to int error. Object type is " + obj.GetType().Name);
		}

		public static long ConvertLong(object obj)
		{
			if (obj is string)
				return long.Parse(obj as string, CultureInfo.InvariantCulture);
			else
				return (long)obj;
		}

		public static float ConvertFloat(object obj)
		{
			if (obj is long)
				return (float) (long) obj;
			else
				return (float)(double)obj;
		}

		public static double ConvertDouble(object obj)
		{
			if (obj is long)
				return (double) (long) obj;
			else
				return (double)obj;
		}

		public static Func<object, T> ConvertEnum<T>(Func<int, T> convert)
		{
			return obj => convert(ConvertInt(obj));
		}

		public static string ConvertString(object obj)
		{
			return (string)obj;
		}

		public static bool ConvertBool(object obj)
		{
			if (obj is long)
				return (long)obj != 0;

			if (obj is int)
				return (int)obj != 0;

			return (bool)obj;
		}

		public static Func<object, T[]> ConvertArray<T>(Func<object, T> convert)
		{
			return obj => (obj as IEnumerable<object>).Select(o => convert(o)).ToArray();
		}

		public static Func<object, List<T>> ConvertList<T>(Func<object, T> convert)
		{
			return obj =>
			{
				if (obj == null)
					return null;

				if (obj is IEnumerable<T>)
					return (obj as IEnumerable<T>).ToList();
				else
					return (obj as IEnumerable<object>).Select(o => convert(o)).ToList();
			};
		}

		public static Func<object, Dictionary<K, V>> ConvertDictionary<K, V>(Func<object, K> convertKey, Func<object, V> convertValue)
		{
			return obj =>
			{
				if (obj is Dictionary<string, object>)
					return (obj as Dictionary<string, object>).ToDictionary(d => convertKey(d.Key), d => convertValue(d.Value));

				if(obj is Dictionary<object, object>)
					return (obj as Dictionary<object, object>).ToDictionary(d => convertKey(d.Key), d => convertValue(d.Value));

				if (obj is Dictionary<K, V>)
					return obj as Dictionary<K, V>;

				throw new Exception("Object has type: " + obj.GetType().Name);
			};
		}

		public static Func<object, Dictionary<int, V>> ConvertDictionary<V>(Func<object, V> convertValue)
		{
			return obj =>
			{
				var dict = new Dictionary<int, V>();
				foreach (var pair in (obj as Dictionary<string, object>))
				{
					dict.Add(ConvertInt(pair.Key), convertValue(pair.Value));
				}
				return dict;
			};
		}

		public static T ConvertType<T>(object obj) where T : IDeserializable, new()
		{
			if (obj == null)
				return default(T);

			if (obj is T)
				return (T)obj;
			else
			{
				var result = new T();
				result.Deserialize(obj as Dictionary<string, object>);
				return result;
			}
		}

		#endregion
	}
}