﻿using ExternalUserIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUserIntegration.Services
{
	public interface IExternalUserService
	{
		Task<User> GetUserByIdAsync(int userId);
		Task<IEnumerable<User>> GetAllUsersAsync();
	}
}
