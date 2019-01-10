using System;

namespace org.newpointe.profilemanager.api
{
    public sealed class KnownEndpoints
    {



        #region Pages

        /// <summary>
        /// The homepage.
        /// </summary>
        public static readonly string PAGE_ROOT = "/";

        /// <summary>
        /// The login page.
        /// </summary>
        public static readonly string PAGE_LOGIN = "/auth";

        /// <summary>
        /// The My Devices page.
        /// </summary>
        public static readonly string PAGE_MYDEVICES = "/mydevices";

        /// <summary>
        /// The Profile Manager page.
        /// </summary>
        public static readonly string PAGE_PROFILEMANAGER = "/profilemanager";

        #endregion



        #region APIs - Auth

        /// <summary>
        /// The endpoint for authenticating a user in Server 5.7.1+.
        /// </summary>
        public static readonly string API_AUTH = "/auth/user";

        /// <summary>
        /// The callback endpoint for authenticating with Profile Manager using a server cookie.
        ///
        /// Still exists but not needed since Server 5.7.1+.
        /// </summary>
        [Obsolete("Not needed since Server 5.7.1")]
        public static readonly string API_PROFILEMANAGER_AUTH_CALLBACK = "/devicemanagement/webapi/authentication/callback";

        /// <summary>
        /// The callback endpoint for authenticating with My Devices using a server cookie.
        ///
        /// Still exists but not needed since Server 5.7.1+.
        /// </summary>
        [Obsolete("Not needed since Server 5.7.1")]
        public static readonly string API_MYDEVICES_AUTH_CALLBACK = "/devicemanagement/webapi/authentication/device_callback";

        /// <summary>
        /// The old endpoint for authenticating with the server.
        ///
        /// Removed since Server 5.7.1+.
        /// </summary>
        [Obsolete("Removed since Server 5.7.1")]
        public static readonly string API_AUTH_COLLABDPROXY = "/collabdproxy";

        #endregion



        #region APIs - Profile Manager - Magic

        /// <summary>
        /// Does Magic.
        ///
        /// (And by magic I mean some weird RPC stuff - Ugh, why can't apps use nice REST APIs?)
        /// </summary>
        public static readonly string API_PROFILEMANAGER_MAGIC_DO_MAGIC = "/devicemanagement/webapi/magic/do_magic";

        /// <summary>
        /// Unknown. Seems to be used to check if the user is authorised.
        /// </summary>
        public static readonly string API_PROFILEMANAGER_MAGIC_ADMIN_WILL_LOAD = "/devicemanagement/webapi/magic/admin_will_load";

        /// <summary>
        /// Unknown. Used by the webapp to check for model updates.
        /// </summary>
        public static readonly string API_PROFILEMANAGER_MAGIC_GET_UPDATED = "/devicemanagement/webapi/magic/get_updated";

        #endregion
    }
}