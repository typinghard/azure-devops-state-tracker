using AzureDevopsTracker.Extensions;
using System;

namespace AzureDevopsTracker.Entities
{
    public class TimeByState : Entity
    {
        public string WorkItemId { get; private set; }
        public string State { get; private set; }
        public double TotalTime { get; private set; }
        public double TotalWorkedTime { get; private set; }
        public WorkItem WorkItem { get; private set; }

        private TimeByState() { }
        public TimeByState(string workItemId,
                           string state,
                           TimeSpan totalTime,
                           TimeSpan totalWorkedTime)
        {
            WorkItemId = workItemId;
            State = state;
            TotalTime = totalTime.TotalSeconds;
            TotalWorkedTime = totalWorkedTime.TotalSeconds;

            Validate();
        }

        public void Validate()
        {
            if (WorkItemId.IsNullOrEmpty() || Convert.ToInt64(WorkItemId) <= 0)
                throw new Exception("WorkItemId is required");

            if (State.IsNullOrEmpty())
                throw new Exception("State is required");
        }
    }
}