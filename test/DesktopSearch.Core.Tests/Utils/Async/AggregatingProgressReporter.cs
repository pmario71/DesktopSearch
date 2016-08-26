using DesktopSearch.Core.Utils.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DesktopSearch.Core.Tests.Utils.Async
{
    public class AggregatingProgressReporterTests
    {
        [Fact]
        public async void Report_single_progress_value()
        {
            int reportedValue = -1;
            var l = new AsyncLock();
            l.Lock();

            Action<int> callback = t =>
            {
                reportedValue = t;
                l.ReleaseLock();
            };

            var sut = new AggregatingProgressReporter(callback);

            var c1 = sut.CreateClient();

            c1.Report(30);

            await l.LockAsync();

            Assert.Equal(30, reportedValue);
        }

        [Fact]
        public async void Reports_from_multiple_clients_are_averaged()
        {
            int reportedValue = -1;
            var l = new AsyncLock();
            l.Lock();

            Action<int> callback = t => 
            {
                reportedValue = t;
                l.ReleaseLock();
            };

            var sut = new AggregatingProgressReporter(callback);

            var c1 = sut.CreateClient();
            var c2 = sut.CreateClient();
            var c3 = sut.CreateClient();

            c1.Report(30);

            await l.LockAsync();

            Assert.Equal(10, reportedValue);
        }

        [Fact]
        public async void Reports_from_different_clients_are_summed()
        {
            int reportedValue = -1;
            var l = new AsyncLock();
            l.Lock();

            int timesToCall = 2;
            Action<int> callback = t =>
            {
                reportedValue = t;

                if ((--timesToCall) <= 0)
                    l.ReleaseLock();
            };

            var sut = new AggregatingProgressReporter(callback);

            var c1 = sut.CreateClient();
            var c2 = sut.CreateClient();
            var c3 = sut.CreateClient();

            c1.Report(30);
            c2.Report(60);

            await l.LockAsync();

            Assert.Equal((30+60)/3, reportedValue);
        }

        [Fact]
        public async void Reports_from_same_client()
        {
            int reportedValue = -1;
            var l = new AsyncLock();
            l.Lock();

            int timesToCall = 2;
            Action<int> callback = t =>
            {
                reportedValue = t;

                if ((--timesToCall) <= 0)
                    l.ReleaseLock();
            };

            var sut = new AggregatingProgressReporter(callback);

            var c1 = sut.CreateClient();
            var c2 = sut.CreateClient();
            var c3 = sut.CreateClient();

            c1.Report(30);
            c1.Report(60);

            await l.LockAsync();

            Assert.Equal(60 / 3, reportedValue);
        }
    }
}
