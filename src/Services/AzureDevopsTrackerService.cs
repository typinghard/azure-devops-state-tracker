using AzureDevopsTracker.Dtos;
using AzureDevopsTracker.Dtos.Create;
using AzureDevopsTracker.Dtos.Delete;
using AzureDevopsTracker.Dtos.Restore;
using AzureDevopsTracker.Dtos.Update;
using AzureDevopsTracker.Entities;
using AzureDevopsTracker.Extensions;
using AzureDevopsTracker.Helpers;
using AzureDevopsTracker.Interfaces;
using AzureDevopsTracker.Interfaces.Internals;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Services
{
    public class AzureDevopsTrackerService : IAzureDevopsTrackerService
    {
        public readonly IWorkItemRepository _workItemRepository;
        public readonly IWorkItemAdapter _workItemAdapter;
        public readonly IChangeLogItemRepository _changeLogItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AzureDevopsTrackerService(
            IWorkItemAdapter workItemAdapter,
            IWorkItemRepository workItemRepository,
            IChangeLogItemRepository changeLogItemRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _workItemAdapter = workItemAdapter;
            _workItemRepository = workItemRepository;
            _changeLogItemRepository = changeLogItemRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Create(CreateWorkItemDto create, bool addWorkItemChange = true)
        {
            var workItem = new WorkItem(create.Resource.Id);

            workItem.Update(create.Resource.Fields.Title,
                            create.Resource.Fields.TeamProject,
                            create.Resource.Fields.AreaPath,
                            create.Resource.Fields.IterationPath,
                            create.Resource.Fields.Type,
                            create.Resource.Fields.CreatedBy.ExtractEmail(),
                            create.Resource.Fields.AssignedTo.ExtractEmail(),
                            create.Resource.Fields.Tags,
                            create.Resource.Fields.Parent,
                            create.Resource.Fields.Effort,
                            create.Resource.Fields.StoryPoints,
                            create.Resource.Fields.OriginalEstimate,
                            create.Resource.Fields.Activity);

            if (addWorkItemChange)
                AddWorkItemChange(workItem, create);

            await CheckWorkItemAvailableToChangeLog(workItem, create.Resource.Fields);

            await AddCustomFields(workItem);

            await _workItemRepository.Add(workItem);
            await _workItemRepository.SaveChangesAsync();
        }

        public async Task Create(string workItemId, Fields fields)
        {
            var createDto = new CreateWorkItemDto()
            {
                Resource = new Dtos.Resource()
                {
                    Fields = fields,
                    Id = workItemId,
                }
            };

            await Create(createDto, false);
        }

        public async Task Update(UpdatedWorkItemDto update)
        {
            if (!await _workItemRepository.Exist(update.Resource.WorkItemId))
                await Create(update.Resource.WorkItemId, update.Resource.Revision.Fields);

            var workItem = await _workItemRepository.GetByWorkItemId(update.Resource.WorkItemId);
            if (workItem is null)
                return;

            workItem.Update(update.Resource.Revision.Fields.Title,
                            update.Resource.Revision.Fields.TeamProject,
                            update.Resource.Revision.Fields.AreaPath,
                            update.Resource.Revision.Fields.IterationPath,
                            update.Resource.Revision.Fields.Type,
                            update.Resource.Revision.Fields.CreatedBy.ExtractEmail(),
                            update.Resource.Revision.Fields.AssignedTo.ExtractEmail(),
                            update.Resource.Revision.Fields.Tags,
                            update.Resource.Revision.Fields.Parent,
                            update.Resource.Revision.Fields.Effort,
                            update.Resource.Revision.Fields.StoryPoints,
                            update.Resource.Revision.Fields.OriginalEstimate,
                            update.Resource.Revision.Fields.Activity);

            AddWorkItemChange(workItem, update);

            await CheckWorkItemAvailableToChangeLog(workItem, update.Resource.Revision.Fields);

            await AddCustomFields(workItem);

            _workItemRepository.Update(workItem);
            await _workItemRepository.SaveChangesAsync();
        }

        public async Task Delete(DeleteWorkItemDto delete)
        {
            if (!await _workItemRepository.Exist(delete.Resource.Id))
                await Create(delete.Resource.Id, delete.Resource.Fields);

            var workItem = await _workItemRepository.GetByWorkItemId(delete.Resource.Id);
            if (workItem is null)
                return;

            workItem.Delete();

            workItem.Update(delete.Resource.Fields.Title,
                delete.Resource.Fields.TeamProject,
                delete.Resource.Fields.AreaPath,
                delete.Resource.Fields.IterationPath,
                delete.Resource.Fields.Type,
                delete.Resource.Fields.CreatedBy.ExtractEmail(),
                delete.Resource.Fields.AssignedTo.ExtractEmail(),
                delete.Resource.Fields.Tags,
                delete.Resource.Fields.Parent,
                delete.Resource.Fields.Effort,
                delete.Resource.Fields.StoryPoints,
                delete.Resource.Fields.OriginalEstimate,
                delete.Resource.Fields.Activity);

            await AddCustomFields(workItem);

            _workItemRepository.Update(workItem);
            await _workItemRepository.SaveChangesAsync();
        }

        public async Task Restore(RestoreWorkItemDto restore)
        {
            if (!await _workItemRepository.Exist(restore.Resource.Id))
                await Create(restore.Resource.Id, restore.Resource.Fields);

            var workItem = await _workItemRepository.GetByWorkItemId(restore.Resource.Id);
            if (workItem is null)
                return;

            workItem.Restore();

            workItem.Update(restore.Resource.Fields.Title,
                restore.Resource.Fields.TeamProject,
                restore.Resource.Fields.AreaPath,
                restore.Resource.Fields.IterationPath,
                restore.Resource.Fields.Type,
                restore.Resource.Fields.CreatedBy.ExtractEmail(),
                restore.Resource.Fields.AssignedTo.ExtractEmail(),
                restore.Resource.Fields.Tags,
                restore.Resource.Fields.Parent,
                restore.Resource.Fields.Effort,
                restore.Resource.Fields.StoryPoints,
                restore.Resource.Fields.OriginalEstimate,
                restore.Resource.Fields.Activity);

            await AddCustomFields(workItem);

            _workItemRepository.Update(workItem);
            await _workItemRepository.SaveChangesAsync();
        }

        public async Task<WorkItemDto> GetByWorkItemId(string workItemId)
        {
            var workItem = await _workItemRepository.GetByWorkItemId(workItemId);
            if (workItem is null)
                return null;

            return _workItemAdapter.ToWorkItemDto(workItem);
        }

        #region Support Methods
        public async Task AddCustomFields(WorkItem workItem)
        {
            try
            {
                var jsonText = await GetRequestBody();
                if (jsonText.IsNullOrEmpty())
                    return;

                var customFields = ReadJsonHelper.ReadJson(workItem.Id, jsonText);
                if (customFields is null || !customFields.Any())
                    return;

                workItem.UpdateCustomFields(customFields);
            }
            catch
            { }
        }

        public async Task<string> GetRequestBody()
        {
            string corpo;
            var request = _httpContextAccessor?.HttpContext?.Request;
            using (StreamReader reader = new(request.Body,
                                             encoding: Encoding.UTF8,
                                             detectEncodingFromByteOrderMarks: false,
                                             leaveOpen: true))
            {
                request.Body.Position = 0;
                corpo = await reader.ReadToEndAsync();
            }

            return corpo;
        }

        public static WorkItemChange ToWorkItemChange(
            string workItemId, string changedBy,
            string iterationPath, DateTime newDate, string newState,
            string oldState = null, DateTime? oldDate = null)
        {
            return new WorkItemChange(workItemId, changedBy.ExtractEmail(), iterationPath, newDate, newState, oldState, oldDate);
        }

        public static void AddWorkItemChange(WorkItem workItem, CreateWorkItemDto create)
        {
            var workItemChange = ToWorkItemChange(workItem.Id,
                                                  create.Resource.Fields.ChangedBy,
                                                  workItem.IterationPath,
                                                  create.Resource.Fields.CreatedDate.ToDateTimeFromTimeZoneInfo(),
                                                  create.Resource.Fields.State);

            if (CheckWorkItemChangeExists(workItem, workItemChange))
                return;

            workItem.AddWorkItemChange(workItemChange);
        }

        public void AddWorkItemChange(WorkItem workItem, UpdatedWorkItemDto update)
        {
            if (update.Resource.Fields.State is null) return;
            if (update.Resource.Fields.StateChangeDate is null) return;

            var changedBy = update.Resource.Revision.Fields.ChangedBy ?? update.Resource.Fields.ChangedBy.NewValue;
            var workItemChange = ToWorkItemChange(workItem.Id,
                                      changedBy,
                                      workItem.IterationPath,
                                      update.Resource.Fields.StateChangeDate.NewValue.ToDateTimeFromTimeZoneInfo(),
                                      update.Resource.Fields.State.NewValue,
                                      update.Resource.Fields.State.OldValue,
                                      update.Resource.Fields.StateChangeDate.OldValue.ToDateTimeFromTimeZoneInfo());

            if (CheckWorkItemChangeExists(workItem, workItemChange))
                return;

            workItem.AddWorkItemChange(workItemChange);

            UpdateTimeByStates(workItem);
        }

        public void UpdateTimeByStates(WorkItem workItem)
        {
            RemoveTimeByStateFromDataBase(workItem);

            workItem.ClearTimesByState();
            workItem.AddTimesByState(workItem.CalculateTotalTimeByState());
        }

        public void RemoveTimeByStateFromDataBase(WorkItem workItem)
        {
            _workItemRepository.RemoveAllTimeByState(workItem.TimeByStates.ToList());
        }

        public async Task CheckWorkItemAvailableToChangeLog(WorkItem workItem, Fields fields)
        {
            if (workItem.CurrentStatus != "Closed" &&
                workItem.LastStatus == "Closed" &&
                workItem.ChangeLogItem is not null &&
                !workItem.ChangeLogItem.WasReleased)
                await RemoveChangeLogItem(workItem);

            if (workItem.CurrentStatus != "Closed" ||
                fields.ChangeLogDescription.IsNullOrEmpty())
                return;

            if (workItem.ChangeLogItem is null)
                workItem.VinculateChangeLogItem(ToChangeLogItem(workItem, fields));
            else
                workItem.ChangeLogItem.Update(workItem.Title, workItem.Type, fields.ChangeLogDescription);
        }

        public static bool CheckWorkItemChangeExists(WorkItem workItem, WorkItemChange newWorkItemChange)
        {
            return workItem.WorkItemsChanges.Any(x => x.NewDate == newWorkItemChange.NewDate &&
                                                      x.OldDate == newWorkItemChange.OldDate &&
                                                      x.NewState == newWorkItemChange.NewState &&
                                                      x.OldState == newWorkItemChange.OldState &&
                                                      x.ChangedBy == newWorkItemChange.ChangedBy &&
                                                      x.IterationPath == newWorkItemChange.IterationPath &&
                                                      x.TotalWorkedTime == newWorkItemChange.TotalWorkedTime);
        }

        public static ChangeLogItem ToChangeLogItem(WorkItem workItem, Fields fields)
        {
            return new ChangeLogItem(workItem.Id, workItem.Title, fields.ChangeLogDescription, workItem.Type);
        }

        public async Task RemoveChangeLogItem(WorkItem workItem)
        {
            var changeLogItem = await _changeLogItemRepository.GetById(workItem.ChangeLogItem?.Id);
            if (changeLogItem is not null)
            {
                _changeLogItemRepository.Delete(changeLogItem);
                await _changeLogItemRepository.SaveChangesAsync();

                workItem.RemoveChangeLogItem();
            }
        }
        #endregion
    }
}