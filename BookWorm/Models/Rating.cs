namespace BookWorm.Models
{
    public class Rating
    {
        public int Id { get; private set; }
        public int Score { get; set; }
        public required ApplicationUser RatedBy { get; set; }

        public Rating(int score)
        {
            if (score is > 5 or < 1) throw new ArgumentException("score should be between 1 and 5");
            Score = score;
        }
    }
}
