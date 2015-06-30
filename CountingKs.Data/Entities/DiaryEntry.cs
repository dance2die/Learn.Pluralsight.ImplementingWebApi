namespace CountingKs.Data.Entities
{
	public class DiaryEntry
	{
		public int Id { get; set; }
		public Food FoodItem { get; set; }
		public Measure Measure { get; set; }
		public double Quantity { get; set; }

		public virtual Diary Diary { get; set; }
	}
}
