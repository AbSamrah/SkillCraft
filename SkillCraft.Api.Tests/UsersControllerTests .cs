using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace SkillCraft.Api.Tests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSolutionRelativeContentRoot("SkillCraft.Api");
    }
}

public class UsersControllerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UsersControllerTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private string GenerateJwtToken(string role)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "test-user-id"),
            new Claim(JwtRegisteredClaimNames.Email, "test@example.com"),
            new Claim("role", role) 
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dfe5-45sd-ete8-xqpd-54w2-fqz1-3s5k-yt7j"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var issuer = "https://localhost:7158";
        var audience = "https://localhost:7158";

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WhenUserIsAdmin()
    {
        // Arrange
        var adminToken = GenerateJwtToken("Admin");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAll_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var userToken = GenerateJwtToken("User");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnUnauthorized_WhenNoTokenIsProvided()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}