using Microsoft.OData.ModelBuilder;
using Swashbuckle.AspNetCore.Filters;
using WebAPI.Wrapper;

namespace WebAPI.SwaggerExamples.Responses;

public class RegisterResponseStatus500Example : IExamplesProvider<RegisterResponseStatus500>
{    
    public RegisterResponseStatus500 GetExamples()
    {
        return new RegisterResponseStatus500
        {
            Succeeded = false,
            Message = "User already exists!"
        };
    }
}

public class RegisterResponseStatus500 : Response
{

}
