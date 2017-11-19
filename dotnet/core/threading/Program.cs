using System;
using System.Threading;

// Example below from: https://msdn.microsoft.com/en-us/library/dd997364(v=vs.110).aspx#Anchor_1

public class Program
{
	public static void Main(string[] args)
	{
		// Create the token source.
		CancellationTokenSource cts = new CancellationTokenSource();

		// Pass the token to the cancelable operation.
		ThreadPool.QueueUserWorkItem(new WaitCallback(DoSomeWork), cts.Token);
		Thread.Sleep(2500);

		// Request cancellation.
		cts.Cancel();
		Console.WriteLine("Cancellation set in token source...");
		Thread.Sleep(2500);
		// Cancellation should have happened, so call Dispose.
		cts.Dispose();
		Environment.Exit(0);
	}

	// Thread 2: The listener
	static void DoSomeWork(object obj)
	{
		CancellationToken token = (CancellationToken)obj;

		for (int i = 0; i < 100000; i++)
		{
			if (token.IsCancellationRequested)
			{
				Console.WriteLine("In iteration {0}, cancellation has been requested...",
								  i + 1);
				// Perform cleanup if necessary.
				//...
				// Terminate the operation.
				break;
			}
			// Simulate some work.
			Thread.Sleep(1000);
		}
	}
}
// The example displays output like the following:
//       Cancellation set in token source...
//       In iteration 4, cancellation has been requested...
