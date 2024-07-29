using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using TheGentlemanLibrary.Application.Models.Users.Commands;
using TheGentlemanLibrary.Application.Models.Users.Interfaces;
using TheGentlemanLibrary.Application.Models.Users.JWT;
using TheGentlemanLibrary.Application.Models.Users.Responses;
using FluentValidation;

namespace TheGentlemanLibrary.Application.Models.Users.Handlers
{
    namespace TheGentlemanLibrary.Application.Models.Users.Handlers
    {
        public class RegisterCommandHandler(IUserRepository userRepo, IOptions<JWTConfiguration> options, ILogger<RegisterCommandHandler> logger, IValidator<RegisterCommand> validator) : IRequestHandler<RegisterCommand, JWTModel>
        {
            public async Task<JWTModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                    throw new ValidationException(validationResult.Errors);
                }

                try
                {
                    var user = await userRepo.CreateUserAsync(request);
                    if (user == null)
                    {
                        return null;
                    }

                    var profileResponse = new ProfileResponseModel
                    {
                        Email = user.Email,
                        Id = user.Id
                    };

                    var token = JWTExtensions.GenerateSecurityToken(profileResponse, options);
                    return token;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while registering the user");
                    return null;
                }
            }
        }
    }
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                     .NotEmpty().WithMessage("Email must not be empty.")
                     .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password must not be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        }
    }
}
