namespace LBoard.Services.Security
{
    public interface IJwtTokenProvider<in TUser>
    {
        string GenerateToken(TUser user);
    }
}