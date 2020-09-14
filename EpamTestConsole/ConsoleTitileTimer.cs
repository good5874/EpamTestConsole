using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EpamTestConsole
{
    [Serializable]
    public class ConsoleTitileTimer
    {
        private static DateTime startTest;
        private static DateTime timeNow;
        private static TimeSpan timePassed;

        [NonSerialized]
        private Timer timer;
        [NonSerialized]
        private TimerCallback tm;

        private static TimeSpan timeLeft;
        private static TimeSpan timelTest;


        public int hours;

        public int minutes;

        public int seconds;       

        public void StartTimer()
        {
            tm = new TimerCallback(Time);            
            timeLeft = new TimeSpan();
            timelTest = new TimeSpan(hours, minutes, seconds);

            timer = new Timer(tm, null, 0, 1000);
            startTest = DateTime.Now;
        }

        private static void Time(object obj)
        {
            timeNow = DateTime.Now;
            timePassed = timeNow - startTest;
            timeLeft = timelTest - timePassed;
            
            Console.Title = "Прошло времени:" +
                "" + timePassed.Hours.ToString() + ":" + timePassed.Minutes.ToString() + ":"
                + timePassed.Seconds.ToString() + "  " +
                " " + "Осталось времени:" + "" + timeLeft.Hours.ToString() + ":" + timeLeft.Minutes.ToString() + ":"
                + timeLeft.Seconds.ToString();


            if (timeLeft.Hours == 0 &&
               timeLeft.Minutes == 0 &&
               timeLeft.Seconds == 0)
            {
                //stop and save
            }
        }
    }
}

