using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Newtonsoft.Json;
using System.Xml.Serialization;
using SS31.Common.Config;

namespace SS31.Common
{
	// The language to use for serialization.
	public enum SerializeAs
	{
		Json,
		Yaml,
		Xml
	}

	// Serializes objects using Json, XML, or YAML, depending on the passed flag. This was a temporary solution and
	// will probably be changed in the future to use only Yaml, which is what we are using for the settings file.
	// Any other things that need to be processed, like item declarations, will probably be managed by a different
	// class in the future.
	public static class ObjectSerializer
	{
		#region File Functions
		public static T DeserializeFile<T>(string path, SerializeAs sa, out bool success, T onFail = default(T))
		{
			try
			{
				using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					using (TextReader reader = new StreamReader(stream))
					{
						string content = reader.ReadToEnd();
						success = true;
						return DeserializeObject<T>(content, sa);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogError("Could not deserialize the object using " + sa + " from the file \"" + path + "\".");
				Logger.LogException(ex);
				success = false;
				return onFail;
			}
		}
		public static void SerializeToFile<T>(T obj, string path, SerializeAs sa, out bool success)
		{
			try
			{
				using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					using (TextWriter writer = new StreamWriter(stream))
					{
						switch (sa)
						{
						case SerializeAs.Json:
							string s = SerializeJsonObject<T>(obj);
							writer.Write(s.ToCharArray());
							writer.Flush();
							break;
						case SerializeAs.Xml:
							XmlSerializer xserial = new XmlSerializer(typeof(T));
							xserial.Serialize(writer, obj);
							writer.Flush();
							break;
						case SerializeAs.Yaml:
							Serializer serial = new Serializer();
							serial.Serialize(writer, obj);
							writer.Flush();
							break;
						}
					}
				}

				success = true;
			}
			catch (Exception ex)
			{
				Logger.LogError("Could not serialize the object using " + sa + " to the file \"" + path + "\".");
				Logger.LogException(ex);
				success = false;
			}
		}
		#endregion

		#region Serial Functions
		public static T DeserializeObject<T>(string s, SerializeAs sa)
		{
			switch(sa)
			{
			case SerializeAs.Json:
				return DeserializeJsonObject<T>(s);
			case SerializeAs.Xml:
				return DeserializeXmlObject<T>(s);
			case SerializeAs.Yaml:
				return DeserializeYamlObject<T>(s);
			default:
				throw new Exception("Could not find type to deserialize.");
			}
		}
		public static string SerializeObject<T>(T obj, SerializeAs sa)
		{
			switch (sa)
			{
			case SerializeAs.Json:
				return SerializeJsonObject<T>(obj);
			case SerializeAs.Xml:
				return SerializeXmlObject<T>(obj);
			case SerializeAs.Yaml:
				return SerializeYamlObject<T>(obj);
			default:
				throw new Exception("Could not find type to serialize.");
			}
		}
		public static string SerializeConfigObject<T>(T obj) where T : IConfiguration
		{
			switch (obj.GetSerializeType())
			{
			case SerializeAs.Json:
				return SerializeJsonObject<T>(obj);
			case SerializeAs.Xml:
				return SerializeXmlObject<T>(obj);
			case SerializeAs.Yaml:
				return SerializeYamlObject<T>(obj);
			default:
				throw new Exception("Could not find type to serialize.");
			}
		}

		public static T DeserializeYamlObject<T>(string input)
		{
			using (StringReader reader = new StringReader(input))
			{
				Deserializer serial = new Deserializer();
				T t = serial.Deserialize<T>(reader);
				return t;
			}
		}
		public static string SerializeYamlObject<T>(T obj)
		{
			using (StringWriter writer = new StringWriter())
			{
				Serializer serial = new Serializer();
				serial.Serialize(writer, obj);
				return writer.ToString();
			}
		}

		public static T DeserializeJsonObject<T>(string input)
		{
			return (T)JsonConvert.DeserializeObject<T>(input);
		}
		public static string SerializeJsonObject<T>(T obj)
		{
			return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
		}

		public static T DeserializeXmlObject<T>(string input)
		{
			using (StringReader reader = new StringReader(input))
			{
				XmlSerializer serial = new XmlSerializer(typeof(T));
				T t = (T)serial.Deserialize(reader);
				return t;
			}
		}
		public static string SerializeXmlObject<T>(T obj)
		{
			using (StringWriter writer = new StringWriter())
			{
				XmlSerializer serial = new XmlSerializer(typeof(T));
				serial.Serialize(writer, obj);
				writer.Flush();
				return writer.ToString();
			}
		}
		#endregion
	}
}