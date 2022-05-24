namespace dev_in_house_basic_jwt
{
    public static class JWTConfiguracaoAppsetings
    {
        public static string Secrets { get; set; }
        public static string Issuer { get; set; }
        public static string Audience { get; set; }
        public static byte[] Key { get; set; }
    }
}
