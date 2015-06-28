using System;
using System.Linq;
using CountingKs.Data.Entities;

namespace CountingKs.Models
{
	public class ModelFactory
	{
		public FoodModel Create(Food food)
		{
			return new FoodModel
				{
				Description = food.Description,
				Measures = food.Measures.Select(measure => Create(measure))
				};
		}

		public MeasureModel Create(Measure measure)
		{
			return new MeasureModel
				{
					Description = measure.Description,
					Calories = Math.Round(measure.Calories)
				};
		}
	}
}