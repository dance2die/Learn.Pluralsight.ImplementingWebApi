using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Data.Entities;

namespace CountingKs.Controllers
{
	public class FoodsController : ApiController
	{
		public IEnumerable<Food> Get()
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