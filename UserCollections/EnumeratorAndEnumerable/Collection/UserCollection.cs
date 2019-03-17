using System;
using System.Collections;
using System.Collections.Generic;


namespace EnumeratorAndEnumerable.Collection
{
    class UserCollection : IEnumerable, IEnumerator, IDisposable
    {
        #region UserCollection Members

        private readonly Element[] m_Elements;
        private int m_Position;

        public UserCollection (int capacity)
        {
            m_Elements = new Element[capacity];
            m_Position = -1;
        }

        public Element this[int index]
        {
            get { return m_Elements[index]; }
            set { m_Elements[index] = value; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator ()
        {
            return this as IEnumerator;
        }

        #endregion

        #region IEnumerator Members

        public object Current
        {
            get { return m_Elements[m_Position]; }
        }

        public bool MoveNext ()
        {
            if (m_Position < m_Elements.Length - 1)
            {
                m_Position++;
                return true;
            }
            return false;
        }

        public void Reset ()
        {
            m_Position = -1;
        }

        #endregion

        #region IDisposable Members

        public void Dispose ()
        {
            ( (IEnumerator)this ).Reset ();
        }

        #endregion
    }
}
