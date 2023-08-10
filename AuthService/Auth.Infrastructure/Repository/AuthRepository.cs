using Auth.Domain.Entities;
using Auth.Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Wrapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Infrastructure.Repository
{
    public class AuthRepository
    {
        private readonly ILogger<AuthRepository> _logger;
        private readonly AuthDbContext _db;
        public const int JWT_TOKEN_VALIDITY_MINS = 20;
        public const string JWT_SECURITY_KEY = "CBS5M8B1W63E511B232SAI9CE316R69V57@sdsU&";
        public AuthRepository(IConfiguration configuration,ILogger<AuthRepository> logger, AuthDbContext DbContext)
        {
            _db = DbContext;
            _logger = logger;
            _db = DbContext;
        }
        public async Task<IResponse<LoginResponseModel>> GetLoginToken(LoginRequestModel authenticationRequest)
        {
            try
            {
                var User = _db.Users.Where(x => x.UserName == authenticationRequest.UserName).FirstOrDefault();

                ////CHECK USERNAME EXIST ON USER TABLE OR  NOT////
                if (User == null)
                {
                    return await Response<LoginResponseModel>.FailAsync("Invalid Username or Password.");
                }
                if (User.Password != authenticationRequest.Password)
                {
                    return await Response<LoginResponseModel>.FailAsync("Invalid Username or Password.");
                }
                //var Roles = _db.userRoles.Where(x => x.UserID == User.UserId).Select(x => x.RoleId).ToList();
                var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
                LoginResponseModel loginResponse = new LoginResponseModel
                {
                    UserName = User.UserName!,
                    ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                    JwtToken = CreateToken(User)
                };
                return await Response<LoginResponseModel>.SuccessAsync(loginResponse);
            }
            catch (Exception ex)
            {
                throw ex is ArgumentException ? new ArgumentException(ex.Message) : ex;
            }
        }
        private string CreateToken(User users)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                       new Claim(ClaimTypes.Name, users.UserName),
                       new Claim(ClaimTypes.Role,users.Role)
                   }),
                Expires = DateTime.UtcNow.AddMinutes(1000),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
