using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.UserControls.VariableTimePicker
{
    public class VariableTime
    {
        public bool IsYearsVariable { get; set; } = false;
        public bool IsMonthsVariable { get; set; } = false;
        public bool IsDaysVariable { get; set; } = false;
        public bool IsHoursVariable { get; set; } = false;
        public bool IsMinutesVariable { get; set; } = false;
        public bool IsSecondsVariable { get; set; } = false;
        public int YearsOffset { get; set; } = 0;
        public int MonthsOffset { get; set; } = 0;
        public int DaysOffset { get; set; } = 0;
        public int HoursOffset { get; set; } = 0;
        public int MinutesOffset { get; set; } = 0;
        public int SecondsOffset { get; set; } = 0;

        public DateTime AbsoluteTime { get; set; } = DateTime.Now;

        internal VariableTime Clone()
        {
            VariableTime variableTime = new VariableTime
            {
                IsYearsVariable = IsYearsVariable,
                IsMonthsVariable = IsMonthsVariable,
                IsDaysVariable = IsDaysVariable,
                IsHoursVariable = IsHoursVariable,
                IsMinutesVariable = IsMinutesVariable,
                IsSecondsVariable = IsSecondsVariable,
                YearsOffset = YearsOffset,
                MonthsOffset = MonthsOffset,
                DaysOffset = DaysOffset,
                HoursOffset = HoursOffset,
                MinutesOffset = MinutesOffset,
                SecondsOffset = SecondsOffset,
                AbsoluteTime = AbsoluteTime
            };
            return variableTime;
        }

        public DateTime GetTime()
        {
            DateTime time = AbsoluteTime;
            if (IsYearsVariable) { time.AddYears(YearsOffset); }
            if (IsMonthsVariable) { time.AddMonths(MonthsOffset); }
            if (IsDaysVariable) { time.AddDays(DaysOffset); }
            if (IsHoursVariable) { time.AddHours(HoursOffset); }
            if (IsMinutesVariable) { time.AddHours(MinutesOffset); }
            if (IsSecondsVariable) { time.AddSeconds(SecondsOffset); }
            return time;
        }
    }
}
