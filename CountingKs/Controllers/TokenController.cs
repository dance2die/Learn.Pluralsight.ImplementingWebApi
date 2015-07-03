using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Data.Entities;
using CountingKs.Models;
using CountingKs.Services;

namespace CountingKs.Controllers
{
	public class TokenController : BaseApiController
	{
		private ICountingKsIdentityService _identityService;

		public TokenController(ICountingKsRepository repo, ICountingKsIdentityService identityService)
			: base(repo)
		{
			_identityService = identityService;
		}

		/// <summary>
		/// DEBUGGING ONLY: Get the signature of current user
		/// </summary>
		/// <returns></returns>
		public HttpResponseMessage Get()
		{
			try
			{
				ApiUser user = TheRepository.GetApiUsers().FirstOrDefault(u => u.Name == _identityService.CurrentUser);
				if (user != null)
				{
					var secret = user.Secret;
					// Simplistic implementation DO NOT USE
					byte[] key = Convert.FromBase64String(secret);
					HMACSHA256 provider = new HMACSHA256(key);
					// Compute Hash from API Key (NOT SECURE)
					byte[] hash = provider.ComputeHash(Encoding.UTF8.GetBytes(user.AppId));
					string signature = Convert.ToBase64String(hash);

					Request.CreateResponse(HttpStatusCode.Created, signature);
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}

			return Request.CreateResponse(HttpStatusCode.BadRequest);
		}

		public HttpResponseMessage Post([FromBody]TokenRequestModel model)
		{
			try
			{
				ApiUser user = TheRepository.GetApiUsers().FirstOrDefault(u => u.AppId == model.ApiKey);
				if (user != null)
				{
					var secret = user.Secret;
					// Simplistic implementation DO NOT USE
					byte[] key = Convert.FromBase64String(secret);
					HMACSHA256 provider = new HMACSHA256(key);
					// Compute Hash from API Key (NOT SECURE)
					byte[] hash = provider.ComputeHash(Encoding.UTF8.GetBytes(user.AppId));
					string signature = Convert.ToBase64String(hash);

					if (signature == model.Signature)
					{
						string rawTokenInfo = string.Concat(user.AppId + DateTime.UtcNow.ToString("d"));
						var rawTokenByte = Encoding.UTF8.GetBytes(rawTokenInfo);
						var token = provider.ComputeHash(rawTokenByte);
						var authToken = new AuthToken
							{
								Token = Convert.ToBase64String(token),
								Expiration = DateTime.UtcNow.AddDays(7),
								ApiUser = user
							};

						if (TheRepository.Insert(authToken) && TheRepository.SaveAll())
						{
							return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(authToken));
						}
					}
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}

			return Request.CreateResponse(HttpStatusCode.BadRequest);
		}
	}
}