namespace Iterator.Pattern
{
    internal class ConcreteIterator : Iterator
    {
        private readonly ConcreteAggregate m_Aggregate;
        private int m_CurrentIndex;

        public ConcreteIterator (ConcreteAggregate aggregate)
        {
            m_Aggregate = aggregate;
            m_CurrentIndex = 0;
        }

        public override object First ()
        {
            return m_Aggregate[0];
        }

        public override object Next ()
        {
            object element = null;
            if (m_CurrentIndex < m_Aggregate.Count - 1)
            {
                element = m_Aggregate[++m_CurrentIndex];
            }
            return element;
        }

        public override bool IsDone ()
        {
            return m_CurrentIndex >= m_Aggregate.Count - 1;
        }

        public override object CurrentItem ()
        {
            return m_Aggregate[m_CurrentIndex];
        }
    }
}
