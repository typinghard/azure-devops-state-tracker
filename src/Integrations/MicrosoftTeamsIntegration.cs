using AzureDevopsTracker.Entities;
using AzureDevopsTracker.Statics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureDevopsTracker.Integrations
{
    internal class MicrosoftTeamsIntegration : MessageIntegration
    {
        internal class MicrosoftTeamsMessage
        {
            public string type { get; set; }
            public string context { get; set; }
            public string themeColor { get; set; }
            public string summary { get; set; }
            public string priority { get; set; }
            public Section[] sections { get; set; }
        }

        internal class Section
        {
            public string activityTitle { get; set; }
            public string activitySubtitle { get; set; }
            public string activityImage { get; set; }
            public string text { get; set; }
            public bool markdown { get; set; }
        }


        internal static class MicrosoftTeamsStatics
        {
            internal static readonly string Type = "MessageCard";
            internal static readonly string Context = "http://schema.org/extensions";
            internal static readonly string ThemeColor = "7bd1d7";
            internal static readonly int TEXT_SIZE_TO_BREAK_LINE = 100;
            internal static readonly int OPENING_DIV_SIZE = 5;
            internal static readonly int CLOSING_DIV_SIZE = 6;
        }

        internal override void Send(ChangeLog changeLog)
        {
            var values = new MicrosoftTeamsMessage()
            {
                type = MicrosoftTeamsStatics.Type,
                context = MicrosoftTeamsStatics.Context,
                themeColor = MicrosoftTeamsStatics.ThemeColor,
                summary = GetTitle(),
                sections = new Section[1]
                {
                    new Section()
                    {
                        activityTitle = GetTitle(),
                        activitySubtitle = GetVersion(changeLog),
                        activityImage = GetAnnouncementImageUrl(),
                        text = GetText(changeLog),
                        markdown = true
                    }
                }
            };

            Notify(values);
        }

        private static string GetText(ChangeLog changeLog)
        {
            if (changeLog is null || !changeLog.ChangeLogItems.Any()) return string.Empty;

            StringBuilder text = new();
            text.AppendLine(GetWorkItemsDescriptionSection("Features", changeLog.ChangeLogItems.Where(x => x.WorkItemType != WorkItemStatics.WORKITEM_TYPE_BUG)));
            text.AppendLine(GetWorkItemsDescriptionSection("Correções", changeLog.ChangeLogItems.Where(x => x.WorkItemType == WorkItemStatics.WORKITEM_TYPE_BUG)));

            text.AppendLine(GetFooter());
            return text.ToString();
        }

        private static string GetWorkItemsDescriptionSection(string sectionName, IEnumerable<ChangeLogItem> changeLogItems)
        {
            StringBuilder text = new();
            if (!changeLogItems.Any()) return string.Empty;

            text.AppendLine($"\n# **{sectionName}**\n");
            foreach (var workItem in changeLogItems)
                text.AppendLine(GetWorkItemDescriptionLine(workItem));

            text.AppendLine("\n");
            return text.ToString();
        }

        private static string GetWorkItemDescriptionLine(ChangeLogItem workItem)
        {
            var description = GetDescription(workItem.Description);
            return $"<em>**{workItem.WorkItemId}**</em> - {description} <br>";
        }

        private static string GetDescription(string description)
        {
            return description.Replace("<div>", "").Replace("</div>", "").Replace("<br>", "");
        }

        private static string GetFooter()
        {
            return $"<br><sup> <img style='width: 16px; height: 16px;' src='{GetLogoTypingHard16x16Url()}' /> {GetNugetVersion()}</sup>";
        }
    }
}
