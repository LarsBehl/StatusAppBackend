using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace StatusAppBackend.Services
{
    public abstract class CronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly double _offset;

        public CronJobService(double offset)
        {
            if (offset <= 0)
                throw new ArgumentException("Offset has to be greater than 0");
            this._offset = offset;
        }

        public void Dispose() => this._timer?.Dispose();

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
                await this.DoWork(cancellationToken);
            // set timer for passed offset
            this._timer = new System.Timers.Timer(this._offset * 1000);
            this._timer.Elapsed += async (sender, args) =>
            {
                if (!cancellationToken.IsCancellationRequested)
                    await this.DoWork(cancellationToken);
            };

            _timer.Start();

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this._timer?.Stop();
            await Task.CompletedTask;
        }

        public abstract Task DoWork(CancellationToken ct);
    }
}