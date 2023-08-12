using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendarService
{
    public class CalendarDetails
    {
        public Calendar Calendar { get; set; }
        public IList<Event> Events { get; set; }
        public string EmbedCode { get; set; }
    }
}
