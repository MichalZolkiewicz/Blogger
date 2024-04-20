using Swashbuckle.AspNetCore.Filters;
using WebAPI.Models;

namespace WebAPI.SwaggerExamples.Requests;

public class RegisterModelExample : IExamplesProvider<RegisterModel>
{
    public RegisterModel GetExamples()
    {
        return new RegisterModel
        {
            UserName = "yourUniqeName",
            Email = "yourEmailAddress@example.com",
            Password = "Pa$$w0rd123!"
        };
    }
}
