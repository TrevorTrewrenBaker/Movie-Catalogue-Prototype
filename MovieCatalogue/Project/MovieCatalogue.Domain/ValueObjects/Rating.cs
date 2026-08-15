using MovieCatalogue.Domain.Enums;

namespace MovieCatalogue.Domain.ValueObjects
{
    public readonly struct Rating
    {
        private const double HighestRating = 10.0; 
        private const double ExcellentThreshold = 8.0;
        private const double GoodThreshold = 6.0;
        private const double AverageThreshold = 4.0;
        private const double MinRating = 0.0; 

        public double Value { get; }

        public Rating(double value)
        {
            if (value < MinRating || value > HighestRating)
                throw new ArgumentOutOfRangeException(nameof(value), $"Rating must be between {MinRating} and {HighestRating}");

            Value = value;
        }

        public RatingCategory Category
        {
            get
            {
                if (Value >= ExcellentThreshold) 
                    return RatingCategory.Excellent;

                if (Value >= GoodThreshold) 
                    return RatingCategory.Good;

                if (Value >= AverageThreshold) 
                    return RatingCategory.Average;

                return RatingCategory.Poor;
            }
        }

        public override string ToString() => $"{Value:F1}/10";
    }
}
