namespace TaskRunnerSharp
{
    /// <summary>
    /// Interface for a utility that manages and runs multiple asynchronous tasks.
    /// </summary>
    public interface ITaskRunner
    {
        /// <summary>
        /// Gets the results of all completed typed tasks.
        /// </summary>
        List<object?> Results { get; }

        /// <summary>
        /// Gets the logs of task executions.
        /// </summary>
        IEnumerable<string> Logs { get; }

        /// <summary>
        /// Adds a task to the runner.
        /// </summary>
        void Add(Task task);

        /// <summary>
        /// Adds a typed task (returning a result) to the runner.
        /// </summary>
        /// <typeparam name="T">Type of result returned by the task.</typeparam>
        void Add<T>(Task<T> task);

        /// <summary>
        /// Adds a synchronous action to the runner.
        /// </summary>
        void Add(Action action);

        /// <summary>
        /// Adds a synchronous function with a return value to the runner.
        /// </summary>
        /// <typeparam name="T">Type of result returned by the function.</typeparam>
        void Add<T>(Func<T> func);

        /// <summary>
        /// Executes all added tasks asynchronously.
        /// </summary>
        Task RunAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes all added tasks synchronously.
        /// </summary>
        void Run(CancellationToken cancellationToken = default);

        /// <summary>
        /// Clears all stored tasks and logs.
        /// </summary>
        void Clear();
    }
}