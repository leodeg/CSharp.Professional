namespace Iterator.Pattern
{
    internal abstract class Aggregate
    {
        public abstract Iterator CreateIterator ();
        public abstract object this[int index] { get; set; }
    }
}
