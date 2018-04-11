﻿using Tgstation.Server.Host.Models;

namespace Tgstation.Server.Host.Security
{
	/// <summary>
	/// Contains various cryptographic functions
	/// </summary>
	interface ICryptographySuite
	{
		/// <summary>
		/// Sets a <see cref="User.PasswordHash"/> for a given <paramref name="user"/>
		/// </summary>
		/// <param name="user">The <see cref="User"/> whos <see cref="User.PasswordHash"/> is to be set</param>
		/// <param name="newPassword">The new password for the <see cref="User"/></param>
		void SetUserPassword(User user, string newPassword);
	}
}
