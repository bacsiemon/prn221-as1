
using DataAccess.Models;

namespace Repositories.Repos.Interfaces
{
    public interface IMemberRepository
    {
		Member? Get(string email, string password);
		List<Member> GetAll();

		bool Delete(int memberId);
		bool Add(Member member);

		bool Update(Member member);
	}
}