using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureDevopsTracker.Entities
{
    public class ChangeLog : Entity
    {
        public string Response { get; private set; }
        public string Number { get; private set; }
        public int Revision { get; private set; }
        private readonly List<ChangeLogItem> _changeLogItems = new();
        public IReadOnlyCollection<ChangeLogItem> ChangeLogItems => _changeLogItems;

        private ChangeLog() { }
        public ChangeLog(string number) => Number = number;
        public ChangeLog(int newRevision)
        {
            Revision = newRevision;
            Number = $"{CreatedAt:yyyyMMdd}.{Revision}";
        }

        public void SetResponse(string response) => Response = response;
        public void ClearResponse() => Response = string.Empty;
        private bool CheckChangeLogItem(ChangeLogItem changeLogItem) => _changeLogItems.Any(x => x.WorkItemId == changeLogItem.WorkItemId);

        public void AddChangeLogItem(ChangeLogItem changeLogItem)
        {
            if (changeLogItem is null)
                throw new Exception("ChangeLogItem is required");

            if (CheckChangeLogItem(changeLogItem))
                return;

            changeLogItem.Release(Id);
            _changeLogItems.Add(changeLogItem);
        }

        public void AddChangeLogItems(IEnumerable<ChangeLogItem> changeLogItems)
        {
            if (changeLogItems is null)
                throw new Exception("ChangeLogItems is required");

            foreach (var changeLogItem in changeLogItems)
                AddChangeLogItem(changeLogItem);
        }
    }
}