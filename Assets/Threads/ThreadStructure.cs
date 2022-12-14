using System;
using System.Threading;

namespace Threads
{
    public class ThreadStructure
    {
        public static Thread SetThread(Action onFinished, Thread th, Action doThis)
        {
            th = new Thread(() =>
            {
                try
                {
                    doThis();
                }
                finally
                {
                    onFinished();
                }

            });
            return th;
        }
    }
}
