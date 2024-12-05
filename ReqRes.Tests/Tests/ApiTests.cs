using Models;
using Services;

namespace Tests
{
    [TestFixture]
    public class ApiTests
    {
        private ApiClient _apiClient;

        [OneTimeSetUp]
        public void Setup()
        {
            _apiClient = new ApiClient(Configuration.BaseUrl);
        }

        [Test]
        public async Task UsersListed_When_GetListUsers()
        {
            var getUsersListResponse = await _apiClient.GetUsers(1);

            Assert.Multiple(() =>
            {
                Assert.That(getUsersListResponse, Is.Not.Null);
                Assert.That(getUsersListResponse.Page, Is.EqualTo(1));
                Assert.That(getUsersListResponse.Data, Is.Not.Empty);
                foreach (var user in getUsersListResponse.Data)
                {
                    Assert.That(user.Id, Is.GreaterThan(0), $"Ivanlid UserId");
                    Assert.That(user.Email, Is.Not.Null.And.Not.Empty, "User email is null or empty.");
                    Assert.That(user.FirstName, Is.Not.Null.And.Not.Empty, "User first name is null or empty.");
                    Assert.That(user.LastName, Is.Not.Null.And.Not.Empty, "User last name is null or empty.");
                    Assert.That(Uri.IsWellFormedUriString(user.Avatar, UriKind.Absolute), Is.True, $"User avatar URL is invalid: {user.Avatar}");
                }
            });

            var firstUser = getUsersListResponse.Data.First();

            Assert.Multiple(() =>
            {
                Assert.That(firstUser.Id, Is.GreaterThan(0));
                Assert.That(firstUser.Email, Is.Not.Empty);
            });

            var sortedUsers = getUsersListResponse.Data
                .OrderBy(u => u.FirstName)
                .ToList();

            Console.WriteLine("Sorted users by FirstName:");
            foreach (var user in sortedUsers)
            {
                Console.WriteLine($"{user.FirstName} {user.LastName}");
            }
        }

        [Test]
        public async Task UserReturned_When_GetSingleUserWithValidId()
        {
            var userId = 2;

            var getUserResponse = await _apiClient.GetUserById(userId);

            Assert.Multiple(() =>
            {
                Assert.That(getUserResponse, Is.Not.Null);
                Assert.That(getUserResponse.Data.Id, Is.EqualTo(2));
                Assert.That(getUserResponse.Data.Email, Is.Not.Empty);

                Assert.That(getUserResponse.Data, Is.Not.Null, "The 'data' section is null.");
                Assert.That(getUserResponse.Data.Id, Is.EqualTo(userId), "The user ID does not match the requested ID.");
                Assert.That(getUserResponse.Data.Email, Is.Not.Null.And.Not.Empty, "The user email is null or empty.");
                Assert.That(getUserResponse.Data.FirstName, Is.Not.Null.And.Not.Empty, "The user first name is null or empty.");
                Assert.That(getUserResponse.Data.LastName, Is.Not.Null.And.Not.Empty, "The user last name is null or empty.");
                Assert.That(Uri.IsWellFormedUriString(getUserResponse.Data.Avatar, UriKind.Absolute), Is.True, "The user avatar URL is not valid.");

                Assert.That(getUserResponse.Support, Is.Not.Null, "The 'support' section is null.");
                Assert.That(Uri.IsWellFormedUriString(getUserResponse.Support.Url, UriKind.Absolute), Is.True, "The support URL is not valid.");
                Assert.That(getUserResponse.Support.Text, Is.Not.Null.And.Not.Empty, "The support text is null or empty.");
            });
        }

        [Test]
        public async Task ExceptionThrown_When_GetUserByInvalidId()
        {
            var invalidId = 10000;

            Assert.ThrowsAsync<Exception>(async () => await _apiClient.GetUserById(invalidId));
        }

        [Test]
        public async Task UserCreated_When_PostRequestSent()
        {
            var userRequest = new UserRequest
            {
                Name = $"User {Guid.NewGuid()}",
                Job = $"Information Technology {Guid.NewGuid()}"
            };

            var createUserResponse = await _apiClient.CreateUser(userRequest);

            Assert.Multiple(() =>
            {
                Assert.That(createUserResponse, Is.Not.Null, "Response should not be null");
                Assert.That(int.Parse(createUserResponse.Id), Is.GreaterThan(0), "Id should be a numeric string greater than 0");
                Assert.That(createUserResponse.Name, Is.EqualTo(userRequest.Name), "Name should match the userRequest.Name");
                Assert.That(createUserResponse.Job, Is.EqualTo(userRequest.Job), "Job should match the userRequest.Job");
            });

            await _apiClient.DeleteUser(int.Parse(createUserResponse.Id));
        }

        [Test]
        public async Task UserDeleted_When_DeleteRequestSent()
        {
            var userRequest = new UserRequest
            {
                Name = $"User to be deleted {Guid.NewGuid()}",
                Job = $"Information Technology {Guid.NewGuid()}"
            };

            var createUserResponse = await _apiClient.CreateUser(userRequest);

            Assert.Multiple(() =>
            {
                Assert.That(createUserResponse, Is.Not.Null, "Response should not be null");
                Assert.That(int.Parse(createUserResponse.Id), Is.GreaterThan(0), "Id should be a numeric string greater than 0");
                Assert.That(createUserResponse.Name, Is.EqualTo(userRequest.Name), "Name should match the userRequest.Name");
                Assert.That(createUserResponse.Job, Is.EqualTo(userRequest.Job), "Job should match the userRequest.Job");
            });

            await _apiClient.DeleteUser(int.Parse(createUserResponse.Id));
        }
    }
} 