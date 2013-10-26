namespace Server_chiruclande_Cessenger
{
    public struct _prepared
    {
        public string get_account_id { get { return "SELECT `id` FROM `accounts` WHERE `username` = ?"; } }
        public string check_login_id { get { return "SELECT `id` FROM `accounts` WHERE `username` = ? AND `sha1_pass_hash` = ?"; } }
    };
}