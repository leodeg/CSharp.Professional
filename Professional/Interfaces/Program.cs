using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    class Program
    {
        static void Main (string[] args)
        {

        }
    }

    internal interface IPoint
    {
        void DoSomething ();
    }

    internal class Point : IPoint
    {
        public virtual void DoSomething ()
        {
            throw new NotImplementedException ();
        }

        //void IPoint.DoSomething ()
        //{
        //    throw new NotImplementedException ();
        //}
    }

    internal class DerivedFromPoint : Point
    {
        public override void DoSomething ()
        {
            base.DoSomething ();
        }
    }
}
