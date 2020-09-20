using System;
using System.Threading;
using System.Threading.Tasks;
using static EpamTestConsole.TreeNode;

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

        public double TimeSeconds = 10;

        [NonSerialized]
        private CancellationTokenSource cts;
        [NonSerialized]
        private CancellationToken token;

        public async void StartTimer(TreeNode test, Metod WriteSectionToConsole)
        {
            Stop = false;
            startTest = DateTime.Now;

            tm = new TimerCallback(Time);
            timer = new Timer(tm, null, 0, 1000);

            cts = new CancellationTokenSource();
            token = cts.Token;
            
            await Task.Run(() => TreeNode.WalkTheTree(test, WriteSectionToConsole, token));            
        }

        private void Time(object obj)
        {
            var now = DateTime.Now;
            var passed = now.Subtract(startTest).TotalSeconds;

            Console.Title = "Время начала теста: " + startTest.ToLongTimeString() + "  Сейчас:" + now.ToLongTimeString() + "  Прошло: " + Math.Round(passed);

            if (passed > TimeSeconds)
            {                
                Stop = true;
                tm = null;                
                cts.Cancel();
                timer.Dispose();
                return;
            }
        }       
    }
}

