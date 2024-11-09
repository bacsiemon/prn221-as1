
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Repositories.Repos.Interfaces;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Runtime.ConstrainedExecution;
using DataAccess;

namespace Repositories.Repos
{
	public class MemberRepository : IMemberRepository
	{

		private readonly string _fileName = "member.json";
		private IGenericJsonTool<Member> _tool;
		private List<Member> _members;

		public MemberRepository()
		{

			if (_tool == null) _tool = new GenericJsonTool<Member>();
			//json
			if (!File.Exists(_fileName) || new FileInfo(_fileName).Length == 0)
			{
				SeedData();
			}
			else
			{
				_members = _tool.Read(_fileName);
				if (_members == null || _members.Count == 0)
					SeedData();
			}
			
		}

		public Member? Get(string email, string password)
		{
			//return _context.Members.FirstOrDefault(m => m.Email.Equals(email) && m.Password.Equals(password));
			_members = _tool.Read(_fileName);
			return _members.FirstOrDefault(m => m.Email.Equals(email) && m.Password.Equals(password));	
		}

		public Member? Get(string email)
		{
			_members = _tool.Read(_fileName);
			return _members.FirstOrDefault(m => m.Email.Equals(email));
		}

		public Member? Get(int memberId)
		{
			_members = _tool.Read(_fileName);
			return _members.FirstOrDefault(m => m.MemberId == memberId);
		}

		public List<Member> GetAll()
		{
			_members = _tool.Read(_fileName);
			return _members;
		}

		public bool Delete(int memberId)
		{
			try
			{
				Member existingMember = _members.FirstOrDefault(m => m.MemberId == memberId);
				if (existingMember == null) return false;
				_members.Remove(existingMember);

				File.Delete(_fileName);
				_tool.Write(_fileName, _members);
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
				GetAll();

				member.MemberId = GenerateId();
				_members.Add(member);
				_tool.Write(_fileName, _members);
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
				Member existing = _members.FirstOrDefault(m => m.MemberId == member.MemberId);
				if (existing == null) return false;

				existing.Email = member.Email;
				existing.CompanyName = member.CompanyName;
				existing.City = member.City;
				existing.Country = member.Country;
				existing.Password = member.Password;
				existing.IsAdmin = member.IsAdmin;

				File.Delete(_fileName);
				_tool.Write(_fileName, _members);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		private int GenerateId()
		{
			int max = 0;
			foreach (Member member in _members)
			{
				if (member.MemberId > max)
					max = member.MemberId;
			}
			return max + 1;
		}

		private void SeedData()
		{
			_members  = new List<Member>()
			{
				new Member
				{
					MemberId = 1,
					Email = "john@gmail.com",
					CompanyName = "FPT",
					City = "Ho Chi Minh",
					Country = "Viet Nam",
					Password = "john123",
					IsAdmin = true,
				},

				new Member
				{
					MemberId = 2,
					Email = "anna@gmail.com",
					CompanyName = "FPT",
					City = "Ho Chi Minh",
					Country = "Viet Nam",
					Password = "anna123",
					IsAdmin = false,
				}
			};
			_tool.Write(_fileName, _members);
		}
	}
}
