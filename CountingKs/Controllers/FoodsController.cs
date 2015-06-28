using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Data.Entities;
using CountingKs.Models;

namespace CountingKs.Controllers
{
	public class FoodsController : ApiController
	{
		private readonly ICountingKsRepository _repo;
		private readonly ModelFactory _modelFactory;

		public FoodsController(ICountingKsRepository repo)
		{
			_repo = repo;
			_modelFactory = new ModelFactory();
		}

		public IEnumerable<FoodModel> Get(bool includeMeasures = true)
		{
			var query = includeMeasures ? _repo.GetAllFoodsWithMeasures() : _repo.GetAllFoods();

			var results = query.OrderBy(food => food.Description)
				.Take(25)
				.ToList()
				.Select(food => _modelFactory.Create(food));

			return results;
		}

		public FoodModel Get(int id)
		{
			return _modelFactory.Create(_repo.GetFood(id));
		}
	}
}