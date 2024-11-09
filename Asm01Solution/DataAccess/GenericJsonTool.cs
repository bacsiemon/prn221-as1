using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess
{
	public class GenericJsonTool<T> : IGenericJsonTool<T> where T : class
	{

		public bool Write(string filename, List<T> t)
		{
				FileStream stream;

				if (!File.Exists(filename))
					stream = File.Create(filename);
				else
					stream = File.OpenWrite(filename);

				JsonSerializer.Serialize(stream, t);
				stream.Close();
				return true;
		}


		public List<T>? Read(string filename)
		{
			FileStream stream = null;
			try
			{		
				if (!File.Exists(filename) || new FileInfo(filename).Length == 0)
				{
					stream = File.Create(filename);
					stream.Close();
					return new List<T>();
				}
				stream = File.OpenRead(filename);
				var returnValue = JsonSerializer.Deserialize<List<T>>(stream);
				stream.Close();

				if (returnValue == null || returnValue.Count == 0)
					return new List<T>();
				return returnValue;
			}
			catch (Exception ex) 
			{
				stream.Close();
				File.Delete(filename);
				return new List<T>();
			}	
		}
	}
}
	

