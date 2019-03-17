namespace Iterator.Pattern
{
    internal class ConcreteAggregate : Aggregate
    {
        private readonly System.Collections.ArrayList m_Elements;
        private ConcreteIterator m_Iterator;

        public ConcreteAggregate ()
        {
            m_Elements = new System.Collections.ArrayList ();
        }

        public override object this[int index]
        {
            get { return m_Elements[index]; }
            set { m_Elements.Add (value); }
        }
        public int Count
        {
            get { return m_Elements.Count; }
        }

        public override Iterator CreateIterator ()
        {
            m_Iterator = new ConcreteIterator (this);
            return m_Iterator;
        }

        public void Clear ()
        {
            m_Elements.Clear ();
        }
    }
}
