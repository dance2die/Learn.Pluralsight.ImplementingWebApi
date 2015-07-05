using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;
using CountingKs.Data;
using CountingKs.Filters;
using CountingKs.Models;

namespace CountingKs.Controllers
{
	[CountingKsAuthorize(false)]
	public class FoodsController : BaseApiController
	{
		public FoodsController(ICountingKsRepository repo)
			: base(repo)
		{
		}

		private const int PAGE_SIZE = 50;

		public object Get(bool includeMeasures = true, int page = 0)
		{
			var query = includeMeasures ? TheRepository.GetAllFoodsWithMeasures() : TheRepository.GetAllFoods();
			var baseQuery = query.OrderBy(food => food.Description);
			var totalCount = baseQuery.Count();
			var totalPages = Math.Ceiling((double)totalCount / PAGE_SIZE);

			var helper = new UrlHelper(Request);
			var links = new List<LinkModel>();
			if (page > 0)
			{
				links.Add(TheModelFactory.CreateLink(helper.Link("Food", new { page = page - 1 }), "prevPage"));
			}

			if (page < totalCount - 1)
			{
				links.Add(TheModelFactory.CreateLink(helper.Link("Food", new { page = page + 1 }), "nextPage"));
			}

			//var prevUrl = page > 0 ? helper.Link("Food", new {page = page - 1}) : "";
			//var nextUrl = page < totalCount - 1 ? helper.Link("Food", new {page = page + 1}) : "";

			var results = baseQuery
				.Skip(PAGE_SIZE * page)
				.Take(PAGE_SIZE)
				.ToList()
				.Select(food => TheModelFactory.Create(food));

			return new
				{
					TotalCount = totalCount,
					TotalPage = totalPages,
					//PrevPageUrl = prevUrl,
					//NextPageUrl = nextUrl,
					Links = links,
					Results = results
				};
		}

		public FoodModel Get(int foodid)
		{
			return TheModelFactory.Create(TheRepository.GetFood(foodid));
		}
	}
}