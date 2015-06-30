using System;
using System.Linq;
using System.Web.Http.Routing;
using CountingKs.Data;
using CountingKs.Models;

namespace CountingKs.Controllers
{
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
			var prevUrl = page > 0 ? helper.Link("Food", new {page = page - 1}) : "";
			var nextUrl = page < totalCount - 1 ? helper.Link("Food", new {page = page + 1}) : "";

			var results = baseQuery
				.Skip(PAGE_SIZE * page)
				.Take(PAGE_SIZE)
				.ToList()
				.Select(food => TheModelFactory.Create(food));

			return new
				{
					TotalCount = totalCount,
					TotalPage = totalPages,
					PrevPageUrl = prevUrl,
					NextPageUrl = nextUrl,
					Results = results
				};
		}

		public FoodModel Get(int foodid)
		{
			return TheModelFactory.Create(TheRepository.GetFood(foodid));
		}
	}
}