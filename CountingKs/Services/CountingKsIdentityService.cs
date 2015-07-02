using System.Threading;

namespace CountingKs.Services
{
	public class CountingKsIdentityService : ICountingKsIdentityService
	{
		public string CurrentUser
		{
			get
			{
				//return "shawnwildermuth";
				return Thread.CurrentPrincipal.Identity.Name;
			}
		}
	}
}