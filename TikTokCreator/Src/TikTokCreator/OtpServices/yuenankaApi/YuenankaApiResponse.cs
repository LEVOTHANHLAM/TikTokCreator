namespace AppDestop.TelegramCreator.yuenankaApi
{
    public class YuenankaApiResponse<T> where T : class 
    {
        public  T Result { get; set; }
        public int ResponseCode { get; set; }
        public string Msg { get; set; }

    }
}
