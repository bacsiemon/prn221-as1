using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DAOs
{
	public class MemberDAO
	{
		public int MemberId { get; set; }
		public string Email { get; set; } = null!;
		public string CompanyName { get; set; } = null!;
		public string City { get; set; } = null!;
		public string Country { get; set; } = null!;

	}
}
