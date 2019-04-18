using System.Collections.Generic;

namespace org.newpointe.profilemanager.api
{
    public class DoMagicRequestItem
    {
        public string Item { get; set; }
        public string Method { get; set; }
        public object[] Parameters { get; set; }
    }
}