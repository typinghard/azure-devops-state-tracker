using AzureDevopsTracker.Extensions;
using System;

namespace AzureDevopsTracker.Entities
{
    public class ChangeLogItem : Entity
    {
        public string WorkItemId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string WorkItemType { get; private set; }
        public string ChangeLogId { get; private set; }
        public bool WasReleased => string.IsNullOrWhiteSpace(ChangeLogId);
        public ChangeLog ChangeLog { get; private set; }

        private ChangeLogItem() { }
        public ChangeLogItem(string workItemId,
                             string title,
                             string description,
                             string workItemType)
        {
            WorkItemId = workItemId;
            Title = title;
            Description = description;
            WorkItemType = workItemType;

            Validate();
        }

        public void Release(string changeLogId) => ChangeLogId = changeLogId;

        public void Update(string title,
                           string workItemType,
                           string description)
        {
            Title = title;
            WorkItemType = workItemType;
            Description = description;
        }

        public void Validate()
        {
            if (WorkItemId.IsNullOrEmpty() || Convert.ToInt64(WorkItemId) <= 0)
                throw new Exception("WorkItemId is required");

            if (Title.IsNullOrEmpty())
                throw new Exception("Title is required");

            if (WorkItemType.IsNullOrEmpty())
                throw new Exception("WorkItemType is required");
        }

    }
}