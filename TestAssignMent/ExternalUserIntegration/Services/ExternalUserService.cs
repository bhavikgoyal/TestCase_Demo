using ExternalUserIntegration.Clients;
using ExternalUserIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUserIntegration.Services
{
	public class ExternalUserService : IExternalUserService
	{
		private readonly IReqresApiClient _apiClient;

		public ExternalUserService(IReqresApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		public async Task<User> GetUserByIdAsync(int userId)
		{
			var userData = await _apiClient.GetUserByIdAsync(userId);
			return MapUser(userData);
		}

		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			var allUserData = _apiClient.GetAllUsersAsync().Result;
			return allUserData.Select(MapUser);
		}

		private static User MapUser(ReqresUserResponse.UserData data)
		{
			return new User
			{
				Id = data.Id,
				Email = data.Email,
				FirstName = data.First_Name,
				LastName = data.Last_Name
			};
		}
	}
}
