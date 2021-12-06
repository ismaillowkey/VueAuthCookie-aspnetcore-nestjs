namespace CookieAuthJwtRefresh.DataContract.Response
{
    public class ServiceResponse
    {
        public string Code { get; set; }
        public string Success { get; set; }
        public object Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public static class StatusResponse
    {
        public static string Status_Success => "success";
        public static string Status_Fail => "fail";
        public static string Status_Error => "error";
    }
}
