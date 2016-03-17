namespace DataModel
{
    public class MovieBase
    {
        private bool Equals(MovieBase other)
        {
            return string.Equals(MovieName, other.MovieName) && string.Equals(Episode, other.Episode) &&
                   Year == other.Year;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (MovieName != null ? MovieName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Episode != null ? Episode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Year.GetHashCode();
                return hashCode;
            }
        }

        public string MovieName { get; set; }
        public string Episode { get; set; }
        public int? Year { get; set; }

        public override bool Equals(object obj)
        {
            var a = obj as MovieBase;
            return a != null && Equals(a);
        }

        public static bool operator ==(MovieBase left, MovieBase right)
        {
            if (Equals(left, null))
                return Equals(right, null);
            if (Equals(right, null))
                return false;
            return string.Equals(left.MovieName, right.MovieName) &&
                   string.Equals(left.Episode, right.Episode) && left.Year == right.Year;
        }

        public static bool operator !=(MovieBase left, MovieBase right)
        {
            return !(left == right);
        }

        public void Copy(MovieBase movieBase)
        {
            MovieName = movieBase.MovieName;
            Episode = movieBase.Episode;
            Year = movieBase.Year;
        }
    }
}