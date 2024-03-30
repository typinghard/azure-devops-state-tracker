namespace AzureDevopsTracker.Tests.Entities
{
    public class WorkItemCustomFieldTests
    {
        [Theory(DisplayName = "CustomField - New")]
        [InlineData("12345", "key", "value")]
        [InlineData("65489", "key1", "value")]
        [InlineData("test", "key", "value1")]
        public void CustomField_New(string workItemId, string key, string value)
        {
            var customField = new WorkItemCustomField(workItemId, key, value);

            customField.WorkItemId.Should().Be(workItemId);
            customField.Key.Should().Be(key);
            customField.Value.Should().Be(value);
        }

        [Theory(DisplayName = "CustomField - Update")]
        [InlineData("value")]
        [InlineData("new_value")]
        [InlineData("value1")]
        public void CustomField_Update(string value)
        {
            var customField = new WorkItemCustomField(Guid.NewGuid().ToString(), "key", "");

            customField.Update(value);

            customField.Value.Should().Be(value);
        }
    }
}