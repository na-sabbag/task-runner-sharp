namespace TaskRunnerSharp
{
    /// <summary>
    /// Utility class to manage and run multiple asynchronous tasks.
    /// </summary>
    public sealed class TaskRunner
    {
        private readonly List<Task> _tasks = [];
        private readonly List<Task<object?>> _typedTasks = [];
        private readonly object _lock = new();
        private readonly List<string> _taskLogs = [];

        /// <summary>
        /// Retrieves the results of all typed tasks as objects.
        /// </summary>
        public List<object?> Results
        {
            get
            {
                lock (_lock)
                {
                    var list = new List<object?>();
                    foreach (var task in _typedTasks)
                    {
                        if (task.IsCompletedSuccessfully)
                            list.Add(task.Result);
                    }
                    return list;
                }
            }
        }

        /// <summary>
        /// Gets all logs of the task executions.
        /// </summary>
        public IEnumerable<string> Logs
        {
            get
            {
                lock (_lock)
                {
                    foreach (var log in _taskLogs)
                    {
                        yield return log;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a non-returning task to the runner.
        /// </summary>
        public void Add(Task task)
        {
            ArgumentNullException.ThrowIfNull(task);

            lock (_lock)
            {
                _tasks.Add(task);
                _taskLogs.Add($"Task added: {task.GetHashCode()}");
            }
        }

        /// <summary>
        /// Adds a typed task (returning a result) to the runner.
        /// </summary>
        public void Add<T>(Task<T> task)
        {
            ArgumentNullException.ThrowIfNull(task);

            lock (_lock)
            {
                var wrappedTask = task.ContinueWith(t => (object?)t.Result, TaskContinuationOptions.ExecuteSynchronously);
                _typedTasks.Add(wrappedTask);
                _taskLogs.Add($"Typed task added: {task.GetHashCode()}");
            }
        }

        /// <summary>
        /// Adds a synchronous action to the runner. The action is executed on a separate thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void Add(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);
            Add(Task.Run(action));
        }

        /// <summary>
        /// Adds a synchronous function that returns a value to the runner. The function is executed on a separate thread.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the function.</typeparam>
        /// <param name="func">A function that returns a result of type T.</param>
        public void Add<T>(Func<T> func)
        {
            ArgumentNullException.ThrowIfNull(func);
            Add(Task.Run(() => func()));
        }

        /// <summary>
        /// Executes all added tasks asynchronously.
        /// </summary>
        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            Task[] allTasks;

            lock (_lock)
            {
                allTasks = [.. _tasks, .. _typedTasks];
            }

            if (allTasks.Length == 0)
                return;

            _taskLogs.Add("Running all tasks asynchronously...");

            await Task.WhenAll(allTasks).WaitAsync(cancellationToken).ConfigureAwait(false);

            _taskLogs.Add("All tasks completed asynchronously.");
        }

        /// <summary>
        /// Executes all added tasks synchronously.
        /// </summary>
        public void Run(CancellationToken cancellationToken = default)
        {
            Task[] allTasks;

            lock (_lock)
            {
                allTasks = [.. _tasks, .. _typedTasks];
            }

            if (allTasks.Length == 0)
                return;

            _taskLogs.Add("Running all tasks synchronously...");

            try
            {
                Task.WhenAll(allTasks).Wait(cancellationToken);
                _taskLogs.Add("All tasks completed synchronously.");
            }
            catch (AggregateException ex)
            {
                _taskLogs.Add($"Error occurred during task execution: {ex.Message}");
                throw ex.Flatten();
            }
        }

        /// <summary>
        /// Removes all stored tasks.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _tasks.Clear();
                _typedTasks.Clear();
                _taskLogs.Add("All tasks cleared.");
            }
        }
    }
}