using System.Collections.Generic;

namespace JsonLib
{
	public interface ISerializable
	{
		void Serialize(Dictionary<string, object> dict);
	}
}