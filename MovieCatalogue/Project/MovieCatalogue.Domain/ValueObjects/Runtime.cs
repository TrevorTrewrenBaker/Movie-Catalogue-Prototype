namespace MovieCatalogue.Domain.ValueObjects
{
    public readonly struct Runtime
    {
        public int Minutes { get; }

        public Runtime(int minutes)
        {
            if (minutes < 0)
                throw new ArgumentOutOfRangeException(nameof(minutes), "Runtime cannot be negative.");

            Minutes = minutes;
        }

        public string ToDisplayString() => $"{Minutes / 60}h {Minutes % 60}m";
    }
}
