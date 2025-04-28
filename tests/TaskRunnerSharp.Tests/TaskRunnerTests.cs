using System.Text;

namespace TaskRunnerSharp.Tests
{
    public class TaskRunnerTests
    {
        [Fact]
        public void Add_NullTask_ThrowsArgumentNullException()
        {
            var taskRunner = new TaskRunner();

            Assert.Throws<ArgumentNullException>(() => taskRunner.Add((Task)null!));
            Assert.Throws<ArgumentNullException>(() => taskRunner.Add((Task<int>)null!));
        }

        [Fact]
        public void Add_Task_AddsTaskSuccessfully()
        {
            var taskRunner = new TaskRunner();
            var task = Task.CompletedTask;

            taskRunner.Add(task);

            Assert.Contains($"Task added: {task.GetHashCode()}", taskRunner.Logs);
        }

        [Fact]
        public void Add_TypedTask_AddsTypedTaskSuccessfully()
        {
            var taskRunner = new TaskRunner();
            var task = Task.FromResult(42);

            taskRunner.Add(task);

            Assert.Contains($"Typed task added: {task.GetHashCode()}", taskRunner.Logs);
        }

        [Fact]
        public async Task RunAsync_ExecutesAllTasksSuccessfully()
        {
            var taskRunner = new TaskRunner();
            var task1 = Task.Delay(100);
            var task2 = Task.FromResult(42);

            taskRunner.Add(task1);
            taskRunner.Add(task2);

            await taskRunner.RunAsync();

            Assert.Contains("Running all tasks asynchronously...", taskRunner.Logs);
            Assert.Contains("All tasks completed asynchronously.", taskRunner.Logs);
        }

        [Fact]
        public void Run_ExecutesAllTasksSuccessfully()
        {
            var taskRunner = new TaskRunner();
            var task1 = Task.Delay(100);
            var task2 = Task.FromResult(42);

            taskRunner.Add(task1);
            taskRunner.Add(task2);

            taskRunner.Run();

            Assert.Contains("Running all tasks synchronously...", taskRunner.Logs);
            Assert.Contains("All tasks completed synchronously.", taskRunner.Logs);
        }

        [Fact]
        public void Run_TaskThrowsException_LogsErrorAndThrows()
        {
            var runner = new TaskRunner();
            runner.Add(Task.Run(() => throw new InvalidOperationException("Test exception")));

            Assert.Throws<AggregateException>(() => runner.Run());

            var logs = runner.Logs;

            bool logContainsError = logs.Any(log => log.Contains("Error occurred during task execution"));

            Assert.True(logContainsError);
        }

        [Fact]
        public void Clear_RemovesAllTasksAndLogs()
        {
            var taskRunner = new TaskRunner();
            var task = Task.CompletedTask;

            taskRunner.Add(task);
            taskRunner.Clear();

            Assert.Empty(taskRunner.Results);
            Assert.Contains("All tasks cleared.", taskRunner.Logs);
        }

        [Fact]
        public void Results_ReturnsCompletedTypedTaskResults()
        {
            var taskRunner = new TaskRunner();
            var task1 = Task.FromResult(42);
            var task2 = Task.FromResult("Hello");

            taskRunner.Add(task1);
            taskRunner.Add(task2);

            var results = taskRunner.Results;

            Assert.Contains(42, results);
            Assert.Contains("Hello", results);
        }

        [Fact]
        public void Logs_ReturnsAllLogs()
        {
            var taskRunner = new TaskRunner();
            var task = Task.CompletedTask;

            taskRunner.Add(task);

            var logs = taskRunner.Logs.ToList();

            Assert.Contains($"Task added: {task.GetHashCode()}", logs);
        }
    }
}