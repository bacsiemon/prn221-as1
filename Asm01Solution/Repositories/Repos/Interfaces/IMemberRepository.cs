
using DataAccess.Models;

namespace Repositories.Repos.Interfaces
{
    public interface IMemberRepository
    {
		Member? Get(string email, string password);
		Member? Get(string email);
		Member? Get(int memberId);
		List<Member> GetAll();

		bool Delete(int memberId);
		bool Add(Member member);

		bool Update(Member member);
	}
}