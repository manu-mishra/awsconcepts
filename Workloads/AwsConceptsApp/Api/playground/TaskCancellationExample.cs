namespace playground
{
    internal class TaskCancellationExample
    {
        async Task RunInBackground(int seconds)
        {
            for (int i = 0; i < seconds; i++)
            {
                Console.WriteLine($"on second {i}");
                await Task.Delay(seconds);
            }
        }

        async Task RunInBackground(CancellationToken cancellationToken)
        {
            int i = 1;
            while (!cancellationToken.IsCancellationRequested)
            {

                Console.WriteLine($"second {i} - started");
                await Task.Delay(TimeSpan.FromSeconds(1));
                Console.WriteLine($"second {i} - done");
                i++;
            }
            Console.WriteLine("cancellation recieved");
        }
        public static async Task RunAsync()
        {
            try
            {
                TaskCancellationExample i = new TaskCancellationExample();
                await i.RunInBackground(20).WaitAsync(TimeSpan.FromSeconds(5));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
            Console.ReadLine();
        }
        public static async Task RunWithCancellationTokenAsync()
        {
            try
            {
                TaskCancellationExample i = new TaskCancellationExample();
                CancellationTokenSource cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await i.RunInBackground(cancellationToken.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
            Console.ReadLine();
        }
    }
}
