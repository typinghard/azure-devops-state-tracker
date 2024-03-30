namespace AzureDevopsTracker.Tests.Entities
{
    public class TimeByStateTests
    {
        [Fact(DisplayName = "TimeByState - New")]
        public void TimeByState_New()
        {
            var workItemId = "123";
            var state = "new";
            var totalTime = new TimeSpan(1, 0, 3);
            var totalWorkedTime = new TimeSpan(2, 3, 40);
            var timeByState = new TimeByState(workItemId, state, totalTime, totalWorkedTime);

            timeByState.Id.Should().NotBeNullOrEmpty();
            timeByState.WorkItemId.Should().Be(workItemId);
            timeByState.State.Should().Be(state);
            timeByState.TotalTime.Should().Be(totalTime.TotalSeconds);
            timeByState.TotalWorkedTime.Should().Be(totalWorkedTime.TotalSeconds);
        }

        [Theory(DisplayName = "TimeByState - Validate")]
        [InlineData("", "new", "WorkItemId is required")]
        [InlineData(null, "", "WorkItemId is required")]
        [InlineData("123", "", "State is required")]
        [InlineData("123", null, "State is required")]
        public void TimeByState_Update(string workItemId, string state, string throwMessage)
        {
            var totalTime = new TimeSpan(1, 0, 3);
            var totalWorkedTime = new TimeSpan(2, 3, 40);

            var mensagemEsperada = Assert.Throws<Exception>(() => new TimeByState(workItemId, state, totalTime, totalWorkedTime)).Message;

            mensagemEsperada.Should().Be(throwMessage);
        }
    }
}