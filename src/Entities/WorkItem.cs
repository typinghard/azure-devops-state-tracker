using AzureDevopsTracker.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureDevopsTracker.Entities
{
    public class WorkItem : Entity
    {
        public string AreaPath { get; private set; }
        public string TeamProject { get; private set; }
        public string IterationPath { get; private set; }
        public string AssignedTo { get; private set; }
        public string Type { get; private set; }
        public string CreatedBy { get; private set; }
        public string Title { get; private set; }
        public string Tags { get; private set; }
        public string Effort { get; private set; }
        public string OriginalEstimate { get; private set; }
        public string StoryPoints { get; private set; }
        public string WorkItemParentId { get; private set; }
        public string Activity { get; private set; }
        public bool Deleted { get; private set; }
        public ChangeLogItem ChangeLogItem { get; private set; }

        private readonly List<WorkItemChange> _workItemsChanges = new();
        public IReadOnlyCollection<WorkItemChange> WorkItemsChanges => _workItemsChanges;

        private readonly List<TimeByState> _timeByState = new();
        public IReadOnlyCollection<TimeByState> TimeByStates => _timeByState;

        private readonly List<WorkItemCustomField> _workItemCustomFields = new();
        public IReadOnlyCollection<WorkItemCustomField> CustomFields => _workItemCustomFields;
        public string CurrentStatus => _workItemsChanges?.OrderBy(x => x.CreatedAt)?.LastOrDefault()?.NewState;
        public string LastStatus => _workItemsChanges?.OrderBy(x => x.CreatedAt)?.ToList()?.Skip(1)?.LastOrDefault()?.OldState;

        private WorkItem() { }
        public WorkItem(string workItemId) : base(workItemId)
        {
            Validate();
        }

        public void Update(string title,
                           string teamProject, string areaPath,
                           string iterationPath, string type,
                           string createdBy, string assignedTo,
                           string tags,
                           string workItemParentId,
                           string effort,
                           string storyPoint,
                           string originalEstimate,
                           string activity)
        {
            TeamProject = teamProject;
            AreaPath = areaPath;
            IterationPath = iterationPath;
            Type = type;
            Title = title;
            CreatedBy = createdBy;
            AssignedTo = assignedTo;
            Tags = tags;
            WorkItemParentId = workItemParentId;
            Effort = effort;
            StoryPoints = storyPoint;
            OriginalEstimate = originalEstimate;
            Activity = activity;
        }

        public void Restore() => Deleted = false;
        public void Delete() => Deleted = true;
        public void ClearTimesByState() => _timeByState.Clear();
        public void RemoveChangeLogItem() => ChangeLogItem = null;

        public void Validate()
        {
            if (Id.IsNullOrEmpty())
                throw new ArgumentException("WorkItemId is required");
        }

        public void AddWorkItemChange(WorkItemChange workItemChange)
        {
            if (workItemChange is null)
                throw new ArgumentException("WorkItemChange is null");

            _workItemsChanges.Add(workItemChange);
        }

        public void AddTimeByState(TimeByState timeByState)
        {
            if (timeByState is null)
                throw new ArgumentException("TimeByState is null");

            _timeByState.Add(timeByState);
        }

        public void AddTimesByState(IEnumerable<TimeByState> timesByState)
        {
            if (timesByState is not null && !timesByState.Any())
                return;

            foreach (var timeByState in timesByState)
                AddTimeByState(timeByState);
        }

        public void AddCustomField(WorkItemCustomField customField)
        {
            if (customField is null)
                throw new ArgumentException("CustomField is null");

            _workItemCustomFields.Add(customField);
        }

        public void AddCustomFields(IEnumerable<WorkItemCustomField> customFields)
        {
            if (customFields is null || !customFields.Any())
                return;

            foreach (var customField in customFields)
                AddCustomField(customField);
        }

        public void UpdateCustomFields(IEnumerable<WorkItemCustomField> newCustomFields)
        {
            if (newCustomFields is null || !newCustomFields.Any())
                return;

            _workItemCustomFields.Clear();
            AddCustomFields(newCustomFields);
        }

        public void VinculateChangeLogItem(ChangeLogItem changeLogItem)
        {
            if (changeLogItem is null)
                throw new ArgumentException("ChangeLogItem is null");

            ChangeLogItem = changeLogItem;
        }

        public IEnumerable<TimeByState> CalculateTotalTimeByState()
        {
            var timesByStateList = new List<TimeByState>();
            if (!_workItemsChanges.Any())
                return timesByStateList;

            foreach (var workItemChange in _workItemsChanges.OrderBy(x => x.CreatedAt)
                                                            .GroupBy(x => x.OldState)
                                                            .Where(x => x.Key is not null))
            {
                var totalTime = TimeSpan.Zero;
                var totalWorkedTime = TimeSpan.Zero;
                foreach (var data in workItemChange)
                {
                    totalTime += data.CalculateTotalTime();
                    totalWorkedTime += TimeSpan.FromSeconds(data.CalculateTotalWorkedTime());
                }

                timesByStateList.Add(new TimeByState(Id, workItemChange.Key, totalTime, totalWorkedTime));
            }

            return timesByStateList;
        }
    }
}