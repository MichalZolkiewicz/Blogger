namespace Infrastructure.Identity;

public class UserService
{
    public bool IsUserEmailConfirmed(ApplicationUser applicationUser)
        => applicationUser.EmailConfirmed ? true : false;
}
