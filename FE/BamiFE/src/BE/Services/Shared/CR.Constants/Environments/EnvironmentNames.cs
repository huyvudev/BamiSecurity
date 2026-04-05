namespace CR.Constants.Environments
{
    public static class EnvironmentNames
    {
        public const string Production = "prod";
        public const string Development = "Development";
        public const string DevelopmentWSL = "DevelopmentWSL";
        public const string Test = "test";
        public const string Staging = "stag";
        public const string DockerDev = "DockerDev";
        public const string Aspire = "Aspire";

        public static readonly string[] DevelopEnv =
        [
            Development,
            DevelopmentWSL,
            Aspire,
            Test,
            DockerDev,
            Staging
        ];

        public static readonly string[] Productions = [Staging, Production];
    }
}
