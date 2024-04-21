using Blogger.Sdk;
using Blogger.Contracts.Requests;
using Refit;

var cachedToken = string.Empty;

var identityApi = RestService.For<IIdentityApi>("https://localhost:44395/");

var bloggerApi = RestService.For<IBloggerApi>("https://localhost:44395/", new RefitSettings
{
    AuthorizationHeaderValueGetter = (request, cancellationToken) => Task.FromResult(cachedToken)
});
//var register = await identityApi.RegisterAsync(new RegisterModel()
//{
//    Email = "sdkaccount@gmail.com",
//    UserName = "sdkaccount",
//    Password = "Pa$$w0rd123!"
//});

var login = await identityApi.LoginAsync(new LoginModel()
{
    UserName = "sdkaccount",
    Password = "Pa$$w0rd123!"
});

cachedToken = login.Content.Token;

var createdPost = await bloggerApi.CreatePostAsync(new CreatePostDto
{
    Title = "Post Sdk",
    Content = "Treść Sdk"
});

var retrievedPost = await bloggerApi.GetPostAsync(createdPost.Content.Data.Id);

await bloggerApi.UpdatePostAsync(new UpdatePostDto
{
    Id = retrievedPost.Content.Data.Id,
    Content = "Nowa treść sdk"
});

await bloggerApi.DeletePostAsync(retrievedPost.Content.Data.Id);

var moj = "string";
