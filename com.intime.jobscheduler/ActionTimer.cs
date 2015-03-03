using System;
using System.Diagnostics;

namespace com.intime.jobscheduler
{
    public class ActionTimer
    {
        public static ActionAnalytics Perform(Action action)
        {
            return Perform(1,action);
        }

        public static ActionAnalytics Perform(int repeatCount, Action action)
        {
            if (repeatCount == 0)
            {
                return new ActionAnalytics(TimeSpan.Zero);
            }

            var watch = new Stopwatch();
            watch.Start();
            for (var time = 0; time < repeatCount; time++)
            {
                action();
            }
            watch.Stop();
            return new ActionAnalytics(watch.Elapsed);
        }
    }

    public class ActionAnalytics
    {

        public ActionAnalytics(TimeSpan time)
        {
            this.Elapsed = time;
        }

        public double TotalSeconds 
        {
            get
            {
                return this.Elapsed.TotalSeconds;
            }
        }

        public TimeSpan Elapsed { get; private set; }

    }
}
