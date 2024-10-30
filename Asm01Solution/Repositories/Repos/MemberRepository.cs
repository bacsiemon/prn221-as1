
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Repositories.Repos.Interfaces;

namespace Repositories.Repos
{
	public class MemberRepository : IMemberRepository
	{
		private Assignment1Context _context;

		public MemberRepository()
		{
			if (this._context == null) _context = new();
		}

		public Member? Get(string email, string password)
		{
			return _context.Members.FirstOrDefault(m => m.Email.Equals(email) && m.Password.Equals(password));
		}

		public Member? Get(string email)
		{
			return _context.Members.FirstOrDefault(m => m.Email.Equals(email));
		}

		public List<Member> GetAll()
		{
			return _context.Members.ToList();
		}

		public bool Delete(int memberId)
		{
			try
			{
				Member existingMember = _context.Members.FirstOrDefault(m => m.MemberId == memberId);
				if (existingMember == null) return false;
				
				_context.Members.Remove(existingMember);
				_context.SaveChanges();
				return true;
			}

			catch (Exception ex)
			{
				return false;
			}

		}

		public bool Add(Member member)
		{
			try
			{
				_context.Members.Add(member);
				_context.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool Update(Member member)
		{
			try
			{
				Member existing = _context.Members.FirstOrDefault(m => m.MemberId == member.MemberId);
				if (existing == null) return false;


				existing.Email = member.Email;
				existing.CompanyName = member.CompanyName;
				existing.City = member.City;
				existing.Country = member.Country;
				existing.Password = member.Password;
				existing.IsAdmin = member.IsAdmin;


				_context.Members.Update(existing);
				_context.SaveChanges(true);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
