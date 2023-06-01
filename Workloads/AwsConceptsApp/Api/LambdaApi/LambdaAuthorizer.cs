namespace LambdaApi
{
    public class tokenGenerationEvent
    {
        public tokenRequest request { get; set; }
        public tokenResponse response { get; set; }
    }

    public class callerContext
    {
        public string awsSdkVersion { get; set; }
        public string clientId { get; set; }
    }

    public class tokenRequest
    {
        public Dictionary<string, string> userAttributes { get; set; }
        public groupConfiguration groupConfiguration { get; set; }
    }

    public class groupConfiguration
    {
        public List<string> groupsToOverride { get; set; }
        public List<string> iamRolesToOverride { get; set; }
        public string preferredRole { get; set; }
    }

    public class tokenResponse
    {
        public claimsOverrideDetails claimsOverrideDetails { get; set; }
    }

    public class claimsOverrideDetails
    {
        public Dictionary<string, string> claimsToAddOrOverride { get; set; }
        public List<string> claimsToSuppress { get; set; }
        public groupOverrideDetails groupOverrideDetails { get; set; }
    }

    public class groupOverrideDetails
    {
        public List<string> groupsToOverride { get; set; }
        public List<string> iamRolesToOverride { get; set; }
        public string preferredRole { get; set; }
    }
}
