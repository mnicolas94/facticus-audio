using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Audio.Utils
{
    public static class AsyncUtils
    {
        public static async Task Delay(float seconds, CancellationToken ct)
        {
#if UNITY_WEBGL
            var start = Time.realtimeSinceStartup;
            var stop = start + seconds;
            while (Time.realtimeSinceStartup < stop && !ct.IsCancellationRequested)
            {
                await Task.Yield();
            }
#else
            int waitMillis = (int) (seconds * 1000);
            await Task.Delay(waitMillis, ct);
#endif
        }
    }
}