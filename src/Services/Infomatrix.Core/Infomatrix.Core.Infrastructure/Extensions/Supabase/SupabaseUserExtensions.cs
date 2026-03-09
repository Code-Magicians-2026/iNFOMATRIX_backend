using Supabase.Gotrue;

namespace Infomatrix.Core.Infrastructure.Extensions.Supabase;

public static class SupabaseUserExtensions
{
    public static bool IsFakeUser(
        this User user)
    {
        return user.Identities is null || user.Identities.Count == 0;
    }
}
