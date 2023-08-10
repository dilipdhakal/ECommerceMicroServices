using Auth.Domain.Model;
using Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services.Interface
{
    public interface IAuthService
    {
        Task<IResponse<LoginResponseModel>> GetLoginToken(LoginRequestModel authenticationRequest);
    }
}
