using RepoDb;
using RepoDb.Interfaces;

namespace Mix.RepoDb
{
    public class MixdbTrace : ITrace
    {
        public void AfterExecution<TResult>(ResultTraceLog<TResult> log)
        {
        }

        public Task AfterExecutionAsync<TResult>(ResultTraceLog<TResult> log, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void BeforeExecution(CancellableTraceLog log)
        {
        }

        public Task BeforeExecutionAsync(CancellableTraceLog log, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
