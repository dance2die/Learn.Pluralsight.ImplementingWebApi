namespace CountingKs.Models
{
	public class TokenRequestModel
	{
		/// <summary>
		/// Developer's API key
		/// </summary>
		public string ApiKey { get; set; }
		/// <summary>
		/// Contains information to verify who they are
		/// </summary>
		public string Signature { get; set; }
	}
}