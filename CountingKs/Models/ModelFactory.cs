using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using CountingKs.Data.Entities;

namespace CountingKs.Models
{
	public class ModelFactory
	{
		private readonly UrlHelper _urlHelper;

		public ModelFactory(HttpRequestMessage request)
		{
			_urlHelper = new UrlHelper(request);
		}

		public FoodModel Create(Food food)
		{
			return new FoodModel
				{
					Url = _urlHelper.Link("Food", new {foodid = food.Id}),
					Description = food.Description,
					Measures = food.Measures.Select(measure => Create(measure))
				};
		}

		public MeasureModel Create(Measure measure)
		{
			return new MeasureModel
				{
					Url = _urlHelper.Link("Measures", new {foodid = measure.Food.Id, id = measure.Id}),
					Description = measure.Description,
					Calories = Math.Round(measure.Calories)
				};
		}
	}
}