
namespace org.newpointe.profilemanager.api.structures
{
    class AutoJoinProfile
    {
        public string admin_session { get; set; }
        public string created_at { get; set; }
        public int[] device_groups { get; set; }
        public bool has_complete_data { get; set; }
        public int id { get; set; }
        public bool is_wildcard { get; set; }
        public string name { get; set; }
        public string reg_challenge { get; set; }
        public int temporary_id { get; set; }
        public string updated_at { get; set; }
        public AutoJoinProfileUsageLogItem[] usage_log { get; set; }
    }
}