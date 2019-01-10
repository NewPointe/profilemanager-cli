using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace org.newpointe.profilemanager.api
{

    public sealed class KnownEntities
    {
        public static readonly string user_group = "user_group";
        public static readonly string user = "user";
        public static readonly string device_group = "device_group";
        public static readonly string device = "device";
        public static readonly string edu_class = "edu_class";
        public static readonly string xsan_network = "xsan_network";
        public static readonly string auto_join_profile = "auto_join_profile";
        public static readonly string completed_library_item_task = "completed_library_item_task";
        public static readonly string library_item_task = "library_item_task";
        public static readonly string settings = "settings";
        public static readonly string unified_application = "unified_application";
        public static readonly string unified_book = "unified_book";
        public static readonly string preference_pane = "preference_pane";
    }

    class Magic
    {



    }
}