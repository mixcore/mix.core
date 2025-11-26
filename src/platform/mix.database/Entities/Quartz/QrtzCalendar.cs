namespace Mix.Database.Entities.Quartz
{
    public partial class QrtzCalendar
    {
        public string SchedName { get; set; }
        public string CalendarName { get; set; }
        public byte[] Calendar { get; set; }
    }
}
