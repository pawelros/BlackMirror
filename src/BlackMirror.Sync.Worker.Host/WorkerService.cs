namespace BlackMirror.Sync.Worker.Host
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Interfaces;

    public class WorkerService
    {
        private readonly ISyncQueue syncQueue;
        private readonly SyncHandler handler;
        private readonly List<Task> tasks;

        public WorkerService(ISyncQueue syncQueue, SyncHandler handler)
        {
            this.syncQueue = syncQueue;
            this.handler = handler;
            this.tasks = new List<Task>();
        }

        public void Start()
        {
            Logging.Log().Debug("Waiting for upcoming jobs...");

            var newTask = Task.Factory.StartNew(
                 () =>
                {
                    while (true)
                    {
                        try
                        {
                            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
                            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                            var job = this.syncQueue.Pull();

                            if (job != null)
                            {
                                Logging.Log().Information("Found job to process. {job}", job);

                                try
                                {
                                    this.syncQueue.SetSynchronizationStatus(job, SynchronizationStatus.InProgress);
                                    this.handler.Handle(job);
                                    this.syncQueue.SetSynchronizationStatus(job, SynchronizationStatus.Done);
                                }
                                catch (Exception ex)
                                {
                                    Logging.Log().Warning("Job {job} failed! {ex}", job, ex);
                                    this.syncQueue.SetSynchronizationStatus(job, SynchronizationStatus.Failed, ex.ToString());
                                }
                            }
                            else
                            {
                                //await Task.Delay(1000);
                                Logging.Log().Debug("No job found to process. Waiting for the next one.");
                                Thread.Sleep(5000);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null
                                && ex.InnerException.Message
                                == "Response status code does not indicate success: 401 (Cannot authenticate user.).")
                            {
                                Logging.Log().Error(ex, "401 (Cannot authenticate worker). Will retry in 5 minutes.");
                                Thread.Sleep(1000 * 60 * 5);
                            }
                            else
                            {
                                Logging.Log().Warning(ex, "Exception occured while waiting for upcoming event.");
                            }

                        }
                    }
                });

            newTask.Wait();
        }
    }
}