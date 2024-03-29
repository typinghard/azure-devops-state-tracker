using AzureDevopsTracker.Extensions;
using System;

namespace AzureDevopsTracker.Entities
{
    public class WorkItemChange : Entity
    {
        public string WorkItemId { get; private set; }
        public DateTime NewDate { get; private set; }
        public DateTime? OldDate { get; private set; }
        public string NewState { get; private set; }
        public string OldState { get; private set; }
        public string ChangedBy { get; private set; }
        public string IterationPath { get; private set; }
        public double TotalWorkedTime { get; private set; }
        public WorkItem WorkItem { get; private set; }

        private WorkItemChange() { }
        public WorkItemChange(string workItemId,
                              string changedBy,
                              string iterationPath,
                              DateTime newDate,
                              string newState,
                              string oldState,
                              DateTime? oldDate)
        {
            WorkItemId = workItemId;
            NewDate = newDate;
            OldDate = oldDate;
            NewState = newState;
            OldState = oldState;
            ChangedBy = changedBy;
            IterationPath = iterationPath;
            TotalWorkedTime = CalculateTotalWorkedTime();

            Validate();
        }

        public void Validate()
        {
            if (WorkItemId.IsNullOrEmpty() || Convert.ToInt64(WorkItemId) <= 0)
                throw new Exception("WorkItemId is required");
        }

        public TimeSpan CalculateTotalTime() => OldDate is null ? TimeSpan.Zero : NewDate.ToDateTimeFromTimeZoneInfo() - OldDate.Value.ToDateTimeFromTimeZoneInfo();

        public double CalculateTotalWorkedTime()
        {
            if (OldDate.GetValueOrDefault() == DateTime.MinValue)
                return 0;

            var oldDate = OldDate.Value.ToDateTimeFromTimeZoneInfo();
            var newDate = NewDate.ToDateTimeFromTimeZoneInfo();

            var secondsWorked = 0D;
            var majorDateBeforeLunch = DateTime.MinValue;
            var minorDateBeforeLunch = DateTime.MinValue;
            var majorDateAfterLunch = DateTime.MinValue;
            var minorDateAfterLunch = DateTime.MinValue;

            DateTime dataAux = DateTime.MinValue;
            int diasCompletos = 0;

            TimeSpan afterLunch;
            TimeSpan beforeLunch;

            for (var dateAux = oldDate; dateAux <= newDate; dateAux = dateAux.AddSeconds(1))
            {
                if (dateAux.DayOfWeek == DayOfWeek.Saturday || dateAux.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                if (dateAux.TimeOfDay.Hours >= 12 && dateAux.TimeOfDay.Hours < 14)
                    continue;

                if (dateAux.TimeOfDay.Hours > 18 && dateAux.TimeOfDay.Hours < 23 ||
                    dateAux.TimeOfDay.Hours >= 0 && dateAux.TimeOfDay.Hours < 8)
                    continue;

                if (dataAux == DateTime.MinValue)
                    dataAux = dateAux;

                if (dataAux != DateTime.MinValue && dataAux.Date != dateAux.Date)
                {
                    beforeLunch = SubtractDates(majorDateBeforeLunch, minorDateBeforeLunch);
                    afterLunch = SubtractDates(majorDateAfterLunch, minorDateAfterLunch);

                    if ((beforeLunch + afterLunch).Hours == 8)
                        diasCompletos++;
                    else
                        secondsWorked += (beforeLunch + afterLunch).TotalSeconds;

                    majorDateBeforeLunch = DateTime.MinValue;
                    minorDateBeforeLunch = DateTime.MinValue;

                    majorDateAfterLunch = DateTime.MinValue;
                    minorDateAfterLunch = DateTime.MinValue;
                    dataAux = dateAux;
                }

                if (dateAux.TimeOfDay.Hours >= 8 && dateAux.TimeOfDay.Hours < 12)
                {
                    if (minorDateBeforeLunch == DateTime.MinValue || dateAux < minorDateBeforeLunch)
                        minorDateBeforeLunch = dateAux;

                    if (majorDateBeforeLunch == DateTime.MinValue || dateAux > majorDateBeforeLunch)
                        majorDateBeforeLunch = dateAux;

                    continue;
                }

                if (dateAux.TimeOfDay.Hours >= 14 && dateAux.TimeOfDay.Hours < 18)
                {
                    if (minorDateAfterLunch == DateTime.MinValue || dateAux < minorDateAfterLunch)
                        minorDateAfterLunch = dateAux;

                    if (majorDateAfterLunch == DateTime.MinValue || dateAux > majorDateAfterLunch)
                        majorDateAfterLunch = dateAux;

                    continue;
                }

            }

            if (dataAux != DateTime.MinValue &&
                oldDate.Date == newDate.Date ||
                (majorDateBeforeLunch != DateTime.MinValue || minorDateBeforeLunch != DateTime.MinValue) ||
                (majorDateAfterLunch != DateTime.MinValue || minorDateAfterLunch != DateTime.MinValue))
            {
                beforeLunch = SubtractDates(majorDateBeforeLunch, minorDateBeforeLunch);
                afterLunch = SubtractDates(majorDateAfterLunch, minorDateAfterLunch);

                secondsWorked += (beforeLunch + afterLunch).TotalSeconds;
            }

            if (diasCompletos > 0)
                secondsWorked += diasCompletos * 28800;

            return secondsWorked;
        }

        #region Private Methods
        private static TimeSpan SubtractDates(DateTime biger, DateTime minor)
        {
            if (biger.Hour == minor.Hour)
            {
                return biger.Subtract(minor);
            }
            else if (biger.Hour > 8 && biger.Hour == 11 && biger.Minute == 59)
            {
                var newMajorDate = new DateTime(biger.Year, biger.Month, biger.Day, 12, 0, 0);
                return newMajorDate.Subtract(minor);
            }
            else if (biger.Hour > 14 && biger.Hour < minor.Hour)
            {
                var newMinorDate = new DateTime(minor.Year, minor.Month, minor.Day, 14, 0, 0);
                return biger.Subtract(newMinorDate);
            }
            else if (biger.Hour > 14 && biger.Hour == 17 && biger.Minute == 59)
            {
                var newMajorDate = new DateTime(biger.Year, biger.Month, biger.Day, 18, 0, 0);
                return newMajorDate.Subtract(minor);
            }
            else
            {
                return biger.Subtract(minor);
            }
        }
        #endregion
    }
}