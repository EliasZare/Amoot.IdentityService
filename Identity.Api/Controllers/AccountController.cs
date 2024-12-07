using Identity.Application.Interfaces.Domain;
using Identity.Application.ViewModels.Account;
using Identity.Application.ViewModels.JwtToken;
using identity.Common.Models;
using Identity.Domain.DTOs.Account;
using Identity.Domain.DTOs.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController(IAccountService _accountService, IJwtTokenService _jwtTokenService) : ControllerBase
    {
        [HttpGet]
        public async Task<List<AccountInfoDto>> Get()
        {
            return await _accountService.GetAccountsAsync();
        }

        [HttpGet]
        [Authorize]
        public async Task<OperationResponse> Check()
        {
            return new OperationResponse(true, "باححححح بااااح");
        }

        [HttpPost]
        public async Task<OperationResponse> Create(CreateAccountRequest command)
        {
            return await _accountService.CreateAsync(command);
        }

        [HttpPost]
        public async Task<JwtTokenInfoDto> Login(LoginAccountRequest command)
        {
            await _accountService.Login(command);
            return await _jwtTokenService.GenerateJwtToken(command);
        }

        [HttpGet]
        public async Task<OperationResponse> Logout(string token)
        {
            return await _jwtTokenService.DestroyAsync(token);
        }

        [HttpPost]
        public async Task<OperationResponse> GenerateNew(JwtTokenRequest command)
        {
            return await _jwtTokenService.GetNewTokenAsync(command);
        }
    }
}
