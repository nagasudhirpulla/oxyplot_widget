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
            DateTime nowTime = DateTime.Now;
            if (IsYearsVariable)
            {
                time = time.AddYears(nowTime.AddYears(YearsOffset).Year - time.Year);
            }
            if (IsMonthsVariable)
            {
                time = time.AddMonths(nowTime.AddMonths(MonthsOffset).Month - time.Month);
            }
            if (IsDaysVariable)
            {
                time = time.AddDays(nowTime.AddDays(DaysOffset).Day - time.Day);
            }
            if (IsHoursVariable)
            {
                time = time.AddHours(nowTime.AddHours(HoursOffset).Hour - time.Hour);
            }
            if (IsMinutesVariable)
            {
                time = time.AddMinutes(nowTime.AddMinutes(MinutesOffset).Minute - time.Minute);
            }
            if (IsSecondsVariable) {
                time = time.AddSeconds(nowTime.AddSeconds(SecondsOffset).Second - time.Second);
            }
            return time;
        }
    }
}
