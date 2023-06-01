using Microsoft.AspNetCore.Http;
using RestApi.Services;
using System.Security.Claims;

namespace RestApi.Tests.Services
{
    public class CurrentIdentityServiceTests
    {
        [Fact(DisplayName = "CurrentIdentityClaimsTest")]
        public void CurrentIdentityService_ReturnsIdFromClaims()
		{
			// Arrange
			var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
			var httpContextMock = new Mock<HttpContext>();
			var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
			var claimsIdentityMock = new Mock<ClaimsIdentity>();
			var userIdClaim = new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "test_user_id");
			claimsIdentityMock.Setup(ci => ci.Claims).Returns(new List<Claim> { userIdClaim });
            claimsPrincipalMock.Setup(cp => cp.Claims).Returns(new List<Claim> { userIdClaim });
			claimsPrincipalMock.Setup(cp => cp.Identity).Returns(claimsIdentityMock.Object);
            httpContextMock.Setup(hc => hc.User).Returns(claimsPrincipalMock.Object);
			httpContextAccessorMock.Setup(hca => hca.HttpContext).Returns(httpContextMock.Object);
			var identityService = new CurrentIdentityService(httpContextAccessorMock.Object);

			// Act
			var result = identityService.Id;
			
			// Assert
			Assert.Equal("test_user_id", result);
		}

        [Fact(DisplayName = "CurrentIdentityAnonomousTest")]
        public void CurrentIdentityService_ReturnsAnonymousIdWhenNoClaims()
		{
			// Arrange
			var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
			var httpContextMock = new Mock<HttpContext>();
			var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
			var claimsIdentityMock = new Mock<ClaimsIdentity>();
			claimsIdentityMock.Setup(ci => ci.Claims).Returns(new List<Claim>());
			claimsPrincipalMock.Setup(cp => cp.Identity).Returns(claimsIdentityMock.Object);
			httpContextMock.Setup(hc => hc.User).Returns(claimsPrincipalMock.Object);
			httpContextAccessorMock.Setup(hca => hca.HttpContext).Returns(httpContextMock.Object);
			var identityService = new CurrentIdentityService(httpContextAccessorMock.Object);

			// Act
			var result = identityService.Id;

			// Assert
			Assert.Equal("Anonomous", result);
		}
	}
}
