using System;
using System.Collections.Generic;
using NLog;
using Quartz;
using Quartz.Impl;

namespace NetFx40WpfTest.Toolkit
{
    public class QuartzHelper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private QuartzHelper()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler();
            _scheduler.Start();
        }

        private static QuartzHelper _instance;

        private static IScheduler _scheduler = null;

        public static QuartzHelper Instance
        {
            get { return _instance ?? (_instance = new QuartzHelper()); }
        }

        private string GetJobName(string jobId)
        {
            return string.Format("job-{0}", jobId);
        }

        private string GetGroupName(string jobId)
        {
            return string.Format("group-{0}", jobId);
        }

        private string GetTriggerName(string jobId)
        {
            return string.Format("trigger-{0}", jobId);
        }

        private JobKey GetJobKey(string jobId)
        {
            return new JobKey(GetJobName(jobId), GetGroupName(jobId));
        }

        private TriggerKey GetTriggerKey(string jobId)
        {
            return new TriggerKey(GetTriggerName(jobId), GetGroupName(jobId));
        }

        public bool IsExistsJob(string jobId)
        {
            return _scheduler.CheckExists(GetJobKey(jobId));
        }

        public bool IsNormalJob(string jobId)
        {
            try
            {
                if (TriggerState.Normal == _scheduler.GetTriggerState(GetTriggerKey(jobId)))
                {
                    return true;
                }
                else
                {
                    Log.Info("IsNormalJob-{0} {1}", jobId,
                        _scheduler.GetTriggerState(GetTriggerKey(jobId)));
                }
            }
            catch (Exception e)
            {
                Log.Error("IsNormalJob-{0} error", jobId);
                Log.Error(e);
            }

            return false;
        }

        public bool StartJob<T>(string jobId, IDictionary<string, object> jobDataMap,
            Action<SimpleScheduleBuilder> action)
            where T : IJob
        {
            try
            {
                IJobDetail job;
                if (null != jobDataMap && 0 < jobDataMap.Count)
                {
                    job = JobBuilder.Create<T>()
                        .WithIdentity(GetJobName(jobId), GetGroupName(jobId))
                        .UsingJobData(new JobDataMap(jobDataMap))
                        .Build();
                }
                else
                {
                    job = JobBuilder.Create<T>()
                        .WithIdentity(GetJobName(jobId), GetGroupName(jobId))
                        .Build();
                }

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(GetTriggerName(jobId), GetGroupName(jobId))
                    .WithSimpleSchedule(action)
                    .Build();
                _scheduler.ScheduleJob(job, trigger);
                Log.Info("StartJob-{0} ok", jobId);
                return true;
            }
            catch (Exception e)
            {
                Log.Error("StartJob-{0} error", jobId);
                Log.Error(e);
            }

            return false;
        }

        public bool StartJob<T>(string jobId, Action<SimpleScheduleBuilder> action) where T : IJob
        {
            return StartJob<T>(jobId, null, action);
        }

        public bool PauseJob(string jobId)
        {
            try
            {
                if (IsNormalJob(jobId))
                {
                    _scheduler.PauseJob(GetJobKey(jobId));
                    Log.Info("PauseJob-{0} ok", jobId);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error("PauseJob-{0} error", jobId);
                Log.Error(e);
            }

            return false;
        }

        public bool ResumeJob(string jobId)
        {
            try
            {
                if (IsExistsJob(jobId) && !IsNormalJob(jobId))
                {
                    _scheduler.ResumeJob(GetJobKey(jobId));
                    Log.Info("ResumeJob-{0} ok", jobId);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error("ResumeJob-{0} error", jobId);
                Log.Error(e);
            }

            return false;
        }

        public bool DeleteJob(string jobId)
        {
            try
            {
                if (IsExistsJob(jobId))
                {
                    _scheduler.PauseJob(GetJobKey(jobId));
                    _scheduler.UnscheduleJob(GetTriggerKey(jobId));
                    _scheduler.DeleteJob(GetJobKey(jobId));
                    Log.Info("DeleteJob-{0} ok", jobId);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error("DeleteJob-{0} error", jobId);
                Log.Error(e);
            }

            return false;
        }

        public void Dispose()
        {
            if (null != _scheduler && _scheduler.IsStarted)
            {
                _scheduler.Shutdown();
                _instance = null;
            }
        }
    }
}