using AzureDevopsTracker.Entities;
using AzureDevopsTracker.Helpers;
using AzureDevopsTracker.Statics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Integrations
{
    internal class DiscordIntegration : MessageIntegration
    {
        internal override async Task Send(ChangeLog changeLog)
        {
            var embedsDTO = new EmbedsDTO
            {
                Embeds = new List<Embed>
                {
                    new Embed()
                    {
                        Author = new Author() { Name = $"{GetTitle()} - {GetVersion(changeLog)} \nData: {DateTime.Now:dd/MM/yyyy}" },
                        Footer = new Footer() { Text = $"Powered by Typing Hard • nuget version {GetNugetVersion()}", IconUrl = GetLogoTypingHard16x16Url() },
                        Thumbnail = new Thumbnail() { Url = GetAnnouncementImageUrl() },
                        Fields = GetText(changeLog)
                    }
                },
            };

           await Notify(embedsDTO);
        }

        public class EmbedsDTO
        {
            public EmbedsDTO()
            {
                Embeds = new List<Embed>();
            }

            [JsonProperty("embeds")]
            public IEnumerable<Embed> Embeds { get; set; }
        }

        public class Embed
        {
            [JsonProperty("author")]
            public Author Author { get; set; }

            [JsonProperty("footer")]
            public Footer Footer { get; set; }

            [JsonProperty("thumbnail")]
            public Thumbnail Thumbnail { get; set; }

            [JsonProperty("Description")]
            public string Description { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("color")]
            public ulong? Color { get; set; }

            [JsonProperty("fields")]
            public IEnumerable<Field> Fields { get; set; }
        }

        public class Author
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Footer
        {
            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("icon_url")]
            public string IconUrl { get; set; }
        }

        public class Thumbnail
        {
            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class Field
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("inline")]
            public bool IsInline { get; set; }
        }

        private static IEnumerable<Field> GetText(ChangeLog changeLog)
        {
            var changeLogItemsAgrupado = changeLog.ChangeLogItems.GroupBy(d => d.WorkItemType);

            var listFields = new List<Field>();

            var features = changeLogItemsAgrupado.Where(x => !x.Key.Equals(WorkItemStatics.WORKITEM_TYPE_BUG)).SelectMany(x => x).Select(x => x).ToList();
            var bugFixes = changeLogItemsAgrupado.Where(x => x.Key.Equals(WorkItemStatics.WORKITEM_TYPE_BUG)).SelectMany(x => x).Select(x => x).ToList();

            if (features.Any())
                listFields.Add(new Field() { Name = "Atualizações", Value = string.Join("\n", features.Select(d => string.Format("\n{0} -   {1}", d.WorkItemId, d.Description.HtmlToRawText()))), IsInline = false });

            if (bugFixes.Any())
                listFields.Add(new Field() { Name = "Correções", Value = string.Join("\n", bugFixes.Select(d => string.Format("\n{0} -   {1}", d.WorkItemId, d.Description.HtmlToRawText()))), IsInline = false });

            return listFields;
        }
    }
}