namespace AzureDevopsTracker.Tests.Entities
{
    public class ChangeLogTests
    {
        [Theory(DisplayName = "ChangeLog - New with NewRevision")]
        [InlineData(12345)]
        [InlineData(65489)]
        public void ChangeLog_NewWithRevision(int newRevision)
        {
            var changeLog = new ChangeLog(newRevision);

            changeLog.Id.Should().NotBeNullOrEmpty();
            changeLog.Revision.Should().Be(newRevision);
            changeLog.Number.Should().Be($"{changeLog.CreatedAt:yyyyMMdd}.{newRevision}");
            changeLog.ChangeLogItems.Should().BeEmpty();
        }

        [Theory(DisplayName = "ChangeLog - New with Number")]
        [InlineData("12345")]
        [InlineData("abcde")]
        public void ChangeLog_NewWithNumber(string number)
        {
            var changeLog = new ChangeLog(number);

            changeLog.Id.Should().NotBeNullOrEmpty();
            changeLog.Number.Should().Be(number);
            changeLog.Revision.Should().Be(0);
            changeLog.ChangeLogItems.Should().BeEmpty();
        }

        [Fact(DisplayName = "ChangeLog - SetResponse")]
        public void ChangeLog_SetResponse()
        {
            var newResponse = "new response";
            var changeLog = new ChangeLog("abc");

            changeLog.SetResponse(newResponse);

            changeLog.Response.Should().Be(newResponse);
        }

        [Fact(DisplayName = "ChangeLog - ClearResponse")]
        public void ChangeLog_ClearResponse()
        {
            var newResponse = "new response";
            var changeLog = new ChangeLog("abc");
            changeLog.SetResponse(newResponse);

            changeLog.ClearResponse();

            changeLog.Response.Should().BeEmpty();
        }

        [Theory(DisplayName = "ChangeLog - AddChangeLogItem")]
        [InlineData("123", "test", "just a test", "bug", true)]
        [InlineData("654", "test", "just a test", "feature", false)]
        public void ChangeLog_CheckChangeLogItem(string workItemId, string title, string description, string type, bool exist)
        {
            var changeLog = new ChangeLog("abc");
            changeLog.AddChangeLogItem(new ChangeLogItem("123", title, description, type));

            var changeLogItem = new ChangeLogItem(workItemId, title, description, type);
            changeLog.AddChangeLogItem(changeLogItem);

            if (exist)
                changeLog.ChangeLogItems.Should().NotContain(changeLogItem);
            else
                changeLog.ChangeLogItems.Should().Contain(changeLogItem);
        }

        [Fact(DisplayName = "ChangeLog - AddChangeLogItem should throw exception")]
        public void ChangeLog_AddChangeLogItem_ThrowException()
        {
            var changeLog = new ChangeLog("abc");
            var mensagemEsperada = Assert.Throws<Exception>(() => changeLog.AddChangeLogItem(null)).Message;

            mensagemEsperada.Should().Be("ChangeLogItem is required");
        }

        [Fact(DisplayName = "ChangeLog - AddChangeLogItems")]
        public void ChangeLog_AddChangeLogItems()
        {
            var changeLog = new ChangeLog("abc");
            var changeLogItems = new List<ChangeLogItem>
            {
                new ChangeLogItem("123", "test", "just a test", "bug"),
                new ChangeLogItem("654", "test", "just a test", "feature")
            };

            changeLog.AddChangeLogItems(changeLogItems);

            changeLog.ChangeLogItems.Should().Contain(changeLogItems);
        }

        [Fact(DisplayName = "ChangeLog - AddChangeLogItems should throw exception")]
        public void ChangeLog_AddChangeLogItems_ThrowException()
        {
            var changeLog = new ChangeLog("abc");

            var mensagemEsperada = Assert.Throws<Exception>(() => changeLog.AddChangeLogItems(null)).Message;

            mensagemEsperada.Should().Be("ChangeLogItems is required");
        }
    }
}