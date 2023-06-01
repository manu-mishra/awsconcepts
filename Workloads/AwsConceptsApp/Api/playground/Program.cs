namespace playground;
class Program
{
    static async Task Main(string[] args)
    {
        //Tournament.Run();
        //await TaskCancellationExample.RunAsync();
        await TaskCancellationExample.RunWithCancellationTokenAsync();
    }
}