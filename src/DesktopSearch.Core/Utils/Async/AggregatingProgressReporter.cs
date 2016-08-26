using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopSearch.Core.Utils.Async
{
    
    /// <summary>
    /// Allows aggregating progress values reported from multiple sources.
    /// Use <see cref="CreateClient"/> to get a new client that can be used to track its partial progress [0-100].
    /// </summary>
    public sealed class AggregatingProgressReporter
    {
        /// <summary>
        /// The context of the thread that created this instance.
        /// </summary>
        private readonly SynchronizationContext _context;

        /// <summary>
        /// The last reported progress value for each client.
        /// </summary>
        private readonly List<int> _progress = new List<int>();
        private readonly Action<int> _progressReportCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyProgress&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="initialProgress">The initial progress value.</param>
        public AggregatingProgressReporter(Action<int> progressReportCallback)
        {
            if (progressReportCallback == null)
                throw new ArgumentNullException("progressReportCallback");

            _context = SynchronizationContextHelpers.CurrentOrDefault;
            _progressReportCallback = progressReportCallback;
        }

        public IProgress<int> CreateClient()
        {
            _progress.Add(0);
            return new AggregatingProgress(this, _progress.Count - 1);
        }

        void Report(int id, int value)
        {
            _context.Post(_ =>
            {
                _progress[id] = value;

                // calc aggregated progress value
                double sum = (double)_progress.Sum();
                int aggregatedValue = (int)(sum/_progress.Count);

                _progressReportCallback(aggregatedValue);
            }, null);
        }

        /// <summary>
        /// A progress implementation that stores progress updates in a property. If this instance is created on a UI thread, its <see cref="Progress"/> property is suitable for data binding.
        /// </summary>
        private sealed class AggregatingProgress : IProgress<int>
        {
            private int _id;
            private AggregatingProgressReporter _aggregator;


            public AggregatingProgress(AggregatingProgressReporter aggregator, int id)
            {
                _aggregator = aggregator;
                this._id = id;
            }

            void IProgress<int>.Report(int value)
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("Reported value must be between [0-100]!");

                _aggregator.Report(_id, value);
            }
        }

    }
}
