using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CountingKs.Data;

namespace CountingKs.Controllers
{
	public class FoodsController : ApiController
	{
		public object Get()
		{
			var repo = new CountingKsRepository(new CountingKsContext());
			var result = repo.GetAllFoods()
				.OrderBy(food => food.Description)
				.Take(25)
				.ToList();

			return result;
		}
	}
}