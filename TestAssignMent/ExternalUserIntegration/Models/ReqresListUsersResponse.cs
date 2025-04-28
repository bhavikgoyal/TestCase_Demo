using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUserIntegration.Models
{
	public class ReqresListUsersResponse
	{
		public int Page { get; set; }
		public int Total_Pages { get; set; }
		public List<ReqresUserResponse.UserData> Data { get; set; }
	}
}
