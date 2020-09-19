namespace LBoard.Services.Security.Jwt
{
    public interface IJwtTokenProvider<in TUser>
    {
        string GenerateToken(TUser user);
    }
}