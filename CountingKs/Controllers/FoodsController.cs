using System.Collections.Generic;
using System.Linq;
using CountingKs.Data;
using CountingKs.Models;

namespace CountingKs.Controllers
{
	public class FoodsController : BaseApiController
	{
		public FoodsController(ICountingKsRepository repo) : base(repo)
		{
		}

		public IEnumerable<FoodModel> Get(bool includeMeasures = true)
		{
			var query = includeMeasures ? TheRepository.GetAllFoodsWithMeasures() : TheRepository.GetAllFoods();
			var results = query.OrderBy(food => food.Description)
				.Take(25)
				.ToList()
				.Select(food => TheModelFactory.Create(food));

			return results;
		}

		public FoodModel Get(int foodid)
		{
			return TheModelFactory.Create(TheRepository.GetFood(foodid));
		}
	}
}