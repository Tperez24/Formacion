using System;
using System.Threading;
using UnityEngine;

public class CreateAThread
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
