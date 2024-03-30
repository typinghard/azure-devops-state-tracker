using AzureDevopsTracker.Entities;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AzureDevopsTracker.Helpers
{
    public static class ReadJsonHelper
    {
        public static IEnumerable<WorkItemCustomField> ReadJson(string workItemId, string jsonTexto)
        {
            try
            {
                var workItemCustomFields = new List<WorkItemCustomField>();
                foreach (KeyValuePair<string, JToken> element in JObject.Parse(jsonTexto))
                {
                    if (element.Value is JObject)
                        ReadJsonObject(workItemId, workItemCustomFields, (JObject)element.Value);
                    else
                        GetWorkItemCustomField(workItemCustomFields, workItemId, element.Key, element.Value.ToString());
                }

                return workItemCustomFields;
            }
            catch
            {
                return Enumerable.Empty<WorkItemCustomField>();
            }
        }

        private static void ReadJsonObject(string workItemId, List<WorkItemCustomField> workItemCustomFields, JObject objeto)
        {
            foreach (KeyValuePair<string, JToken> item in objeto)
                if (item.Value is JObject)
                    ReadJsonObject(workItemId, workItemCustomFields, (JObject)item.Value);
                else
                    GetWorkItemCustomField(workItemCustomFields, workItemId, item.Key, item.Value.ToString());
        }

        private static void GetWorkItemCustomField(List<WorkItemCustomField> workItemCustomFields, string workItemId, string key, string value)
        {
            if (key is not null && !key.ToLower().Contains("custom")) return;
            workItemCustomFields.Add(new WorkItemCustomField(workItemId, key, value));
        }
    }
}
