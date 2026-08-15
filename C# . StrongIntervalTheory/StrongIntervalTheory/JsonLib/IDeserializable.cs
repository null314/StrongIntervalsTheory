using System.Collections.Generic;

namespace JsonLib
{
	public interface IDeserializable
	{
		void Deserialize(Dictionary<string, object> dict);
	}
}