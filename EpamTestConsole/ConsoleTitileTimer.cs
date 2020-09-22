using System;
using System.Threading;
using System.Threading.Tasks;
using static EpamTestConsole.TreeNode;

namespace EpamTestConsole
{
    [Serializable]
    public class ConsoleTitileTimer : IDisposable
    {
        [NonSerialized]
        private Timer timer;
        [NonSerialized]
        private TimerCallback tm;
        [NonSerialized]
        private static DateTime startTest;        

        public double TimeSeconds = 10;

        [NonSerialized]
        public CancellationToken token;
        [NonSerialized]
        private CancellationTokenSource cts;

        public async void StartTimer(TreeNode test, Metod WriteSectionToConsole, ManualResetEvent _eventMainMenu)
        {
            cts = new CancellationTokenSource();
            token = cts.Token;

            startTest = DateTime.Now;

            tm = new TimerCallback(Time);
            timer = new Timer(tm, null, 0, 1000);
            
            await Task.Run(() => TreeNode.WalkTheTree(test, WriteSectionToConsole, ref timer, ref _eventMainMenu, token));            
        }

        private void Time(object obj)
        {            
            var now = DateTime.Now;            
            var passed = now.Subtract(startTest).TotalSeconds;

            Console.Title = "Время начала теста: " + startTest.ToLongTimeString() + "  Сейчас:" + now.ToLongTimeString() + "  Прошло: " + Math.Round(passed);

            if (passed > TimeSeconds)
            {
                cts.Cancel();
                Dispose();
                return;
            }
        }

        public void Dispose()
        {
            timer.Dispose();                      
        }
        
    }
}

