using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EpamTestConsole
{
    [Serializable]
    public class ConsoleTitileTimer
    {
        [NonSerialized]
        private Timer timer;
        [NonSerialized]
        private TimerCallback tm;
        [NonSerialized]
        private static DateTime startTest;
        [NonSerialized]
        public bool Stop = false;

        public double TimeSeconsd = 10;

        public void StartTimer()
        {
            Stop = false;
            startTest = DateTime.Now;

            tm = new TimerCallback(Time);
            timer = new Timer(tm, null, 0, 1000);    
        }

        private void Time(object obj)
        {
            var now = DateTime.Now;
            var passed = now.Subtract(startTest).TotalSeconds;

            Console.Title = "Время начала теста: " + startTest.ToLongTimeString() + "  Сейчас:" + now.ToLongTimeString() + "  Прошло: " + passed;

            if (passed > TimeSeconsd)
            {                
                Stop = true;
                tm = null;
                timer.Dispose();
                return;
            }
        } 
    }
}

