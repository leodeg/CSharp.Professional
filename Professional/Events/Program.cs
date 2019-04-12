using System;
using System.Collections.Generic;

namespace Events
{
    internal class Program
    {
        private static void Main (string[] args)
        {

        }
    }

    public static class EventArgExtensions
    {
        public static void Raise<TEventArgs> (this TEventArgs e, object sender, ref EventHandler<TEventArgs> eventHandler)
        {
            System.Threading.Volatile.Read (ref eventHandler)?.Invoke (sender, e);
        }
    }

    #region Mail

    internal class MailEventArgs : EventArgs
    {
        public MailEventArgs (string from, string to, string subject)
        {
            From = from;
            To = to;
            Subject = subject;
        }

        public string From { get; }
        public string To { get; }
        public string Subject { get; }
    }

    internal class MailManager
    {
        public event EventHandler<MailEventArgs> MailHandler;

        protected virtual void OnNewMail (MailEventArgs e)
        {
            e.Raise (this, ref MailHandler);
        }

        public void NewMail (string from, string to, string subject)
        {
            MailEventArgs e = new MailEventArgs (from, to, subject);
            OnNewMail (e);
        }
    }

    internal sealed class Fax
    {
        public Fax (MailManager manager)
        {
            manager.MailHandler += FaxMsg;
        }

        public void Unregister (MailManager manager)
        {
            manager.MailHandler -= FaxMsg;
        }

        private void FaxMsg (object sender, MailEventArgs args)
        {
            Console.WriteLine ("Fax mail message: ");
            Console.WriteLine ("From = {0}, To = {1}, Subject = {2}", args.From, args.To, args.Subject);
        }
    }

    #endregion

    #region EventSet

    public sealed class EventKey { }

    public sealed class EventSet
    {
        private readonly Dictionary<EventKey, Delegate> events = new Dictionary<EventKey, Delegate> ();

        public void Add (EventKey eventKey, Delegate handler)
        {
            System.Threading.Monitor.Enter (events);

            Delegate temp;
            events.TryGetValue (eventKey, out temp);
            events[eventKey] = Delegate.Combine (temp, handler);

            System.Threading.Monitor.Exit (events);
        }

        public void Remove (EventKey eventKey, Delegate handler)
        {
            System.Threading.Monitor.Enter (events);

            Delegate temp;
            if (events.TryGetValue(eventKey, out temp))
            {
                temp = Delegate.Remove (temp, handler);
                if (temp != null)
                {
                    events[eventKey] = temp;
                }
                else
                {
                    events.Remove (eventKey);
                }
            }

            System.Threading.Monitor.Exit (events);
        }

        public void Raise (EventKey eventKey, object sender, EventArgs e)
        {
            Delegate temp;
            System.Threading.Monitor.Enter (events);
            events.TryGetValue (eventKey, out temp);
            System.Threading.Monitor.Exit (events);

            if (temp != null)
            {
                temp.DynamicInvoke(new object[] { sender, e});
            }
        }
    }

    #endregion
}
