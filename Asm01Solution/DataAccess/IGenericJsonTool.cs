
namespace DataAccess
{
	public interface IGenericJsonTool<T> where T : class
	{
		List<T>? Read(string filename);
		bool Write(string filename, List<T> t);
	}
}