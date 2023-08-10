using Auth.Application.Services.Interface;
using Auth.Domain.Model;
using Auth.Infrastructure.Repository;
using Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Auth.Domain.Entities;

namespace Auth.Application.Services.Implementation
{
    public class AuthService : IAuthService
    {
        
        private readonly AuthRepository _authRepo;
        public AuthService(AuthRepository authRepo)
        {
            _authRepo = authRepo;
        }
        public async Task<IResponse<LoginResponseModel>> GetLoginToken(LoginRequestModel authenticationRequest)
        {
            return await _authRepo.GetLoginToken(authenticationRequest);
        }
    }
}
