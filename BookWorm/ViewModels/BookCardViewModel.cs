namespace BookWorm.ViewModels
{
    public class BookCardViewModel
    {
        private double _rating;
        public string? CoverImagePath { get; set; }
        public string Title { get; set; }

        public string Rating
        {
            get => _rating.ToString("F1");
            set => _rating = double.Parse(value);
        }
    }
}
