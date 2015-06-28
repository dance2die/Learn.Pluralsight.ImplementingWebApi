using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Models;
using CountingKs.Services;

namespace CountingKs.Controllers
{
	public class DiariesController : BaseApiController
	{
		private ICountingKsIdentityService _identityService;

		public DiariesController(ICountingKsRepository repo, ICountingKsIdentityService identityService)
			: base(repo)
		{
			_identityService = identityService;
		}

		public IEnumerable<DiaryModel> Get()
		{
			var userName = _identityService.CurrentUser;
			var results = TheRepository.GetDiaries(userName)
				.OrderByDescending(diary => diary.CurrentDate)
				.Take(10)
				.ToList()
				.Select(diary => TheModelFactory.Create(diary));
			return results;
		}

		public HttpResponseMessage Get(DateTime diaryId)
		{
			var userName = _identityService.CurrentUser;
			var diary = TheRepository.GetDiary(userName, diaryId);
			if (diary == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound);
			}
			return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(diary));
		}
	}
}
