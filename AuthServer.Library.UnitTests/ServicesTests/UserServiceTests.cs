using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using FakeItEasy;
using AuthServer.Library.Enums;
using AuthServer.Library.DataAccess;
using AuthServer.Library.Services;
using AuthServer.Library.Repositories;
using AuthServer.Library.Models;
using AuthServer.Library.Models.Requests;

namespace AuthServer.Library.UnitTests.ServicesTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;
        private ICryptographyService _cryptographyService;
        private IUserRepository _userRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _cryptographyService = A.Fake<ICryptographyService>();
            _userRepository = A.Fake<IUserRepository>();
            _userService = new UserService(_userRepository, _cryptographyService);

            A.CallTo(() => _userRepository.GetUser(GivenValidUser().Username))
                .Returns(GivenValidUserResponse());

            A.CallTo(() => _cryptographyService.GetHash(GivenValidUser().Password, GivenValidUser().Salt))
                .Returns(GivenValidUser().Password);
        }

        [Test]
        public async Task Authenticate_ReturnsUser_ForValidRequest()
        {
            var request = new LoginRequest
            {
                Username = GivenValidUser().Username,
                Password = GivenValidUser().Password
            };

            var expected = GivenValidUser();
            var result = await _userService.Authenticate(request);

            result.Should().BeEquivalentTo(expected);
        }

        private User GivenValidUser() =>
            new User
            {
                Id = 1
                Username = "ValidUserUsername",
                Password = "ValidUserPassword",
                Salt = "ValidUserSalt",
                UserType = UserType.Member
            };

        private DbResponse<User> GivenValidUserResponse() =>
            DbResponse<User>.Success<User>(GivenValidUser());
    }
}
