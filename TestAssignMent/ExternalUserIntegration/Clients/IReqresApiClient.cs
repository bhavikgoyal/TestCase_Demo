using ExternalUserIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUserIntegration.Clients
{
	public interface IReqresApiClient
	{
		Task<ReqresUserResponse.UserData> GetUserByIdAsync(int userId);
		Task<List<ReqresUserResponse.UserData>> GetAllUsersAsync();
	}
}
