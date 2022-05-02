using System.Security.Claims;

namespace UptimeR.Policies
{
    public static class Storage_Claims
    {
         public readonly static Claim Admin = new Claim (ClaimTypes.Role, "Admin");
    }
}
