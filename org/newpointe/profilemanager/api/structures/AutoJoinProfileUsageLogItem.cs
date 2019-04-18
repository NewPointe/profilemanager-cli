
namespace org.newpointe.profilemanager.api.structures
{
    class AutoJoinProfileUsageLogItem
    {
        public string DeviceName { get; set; }
        public string ProductName { get; set; }
        public int auto_join_profile_id { get; set; }
        public string checkin_token { get; set; }
        public string created_at { get; set; }
        public int device_id { get; set; }
        public int? device_tombstone_id { get; set; }
        public bool has_complete_data { get; set; }
        public int id { get; set; }
        public string udid { get; set; }
    }
}