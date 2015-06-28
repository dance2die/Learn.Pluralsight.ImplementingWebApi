using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Data.Entities;

namespace CountingKs.Controllers
{
	public class FoodsController : ApiController
	{
		private ICountingKsRepository _repo;

		public FoodsController(ICountingKsRepository repo)
		{
			_repo = repo;
		}

		public IEnumerable<Food> Get()
		{
			var result = _repo.GetAllFoods()
				.OrderBy(food => food.Description)
				.Take(25)
				.ToList();

			return result;
		}
	}
}