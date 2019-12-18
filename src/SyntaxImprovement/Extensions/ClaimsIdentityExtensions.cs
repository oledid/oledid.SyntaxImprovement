using System.Security.Claims;

namespace oledid.SyntaxImprovement.Extensions
{
	public static class ClaimsIdentityExtensions
	{
		/// <summary>
		/// Adds or updates a claim
		/// </summary>
		public static void SetClaim(this ClaimsIdentity identity, string claimType, string value)
		{
			if (identity.HasClaim(c => c.Type == claimType))
			{
				identity.RemoveClaim(identity.FindFirst(c => c.Type == claimType));
			}
			identity.AddClaim(new Claim(claimType, value));
		}

		/// <summary>
		/// Adds or removes a claim
		/// </summary>
		public static void SetClaim(this ClaimsIdentity identity, string claimType, bool value)
		{
			if (identity.HasClaim(c => c.Type == claimType))
			{
				identity.RemoveClaim(identity.FindFirst(c => c.Type == claimType));
			}
			if (value)
			{
				identity.AddClaim(new Claim(claimType, value.ToString()));
			}
		}
	}
}
