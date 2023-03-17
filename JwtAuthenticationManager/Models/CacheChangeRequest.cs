namespace JwtAuthenticationManager.Models
{
    public class CacheChangeRequest
    {
        public int uAutoId { get; set; }

        public string uId { get; set; }
        public string cId { get; set; }
        public string role { get; set; }
    }
}
