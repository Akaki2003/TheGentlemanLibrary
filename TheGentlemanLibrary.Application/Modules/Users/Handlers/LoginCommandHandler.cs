using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TheGentlemanLibrary.Application.Models.Users.Commands;
using TheGentlemanLibrary.Application.Models.Users.Interfaces;
using TheGentlemanLibrary.Application.Models.Users.JWT;
using TheGentlemanLibrary.Application.Models.Users.Responses;

namespace TheGentlemanLibrary.Application.Models.Users.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, JWTModel>
    {
        private readonly IUserRepository _userRepo;
        private readonly IOptions<JWTConfiguration> _options;
        private readonly ILogger<LoginCommandHandler> _logger;
        private readonly IValidator<LoginCommand> _validator;

        public LoginCommandHandler(IUserRepository userRepo, IOptions<JWTConfiguration> options, ILogger<LoginCommandHandler> logger, IValidator<LoginCommand> validator)
        {
            _userRepo = userRepo;
            _options = options;
            _logger = logger;
            _validator = validator;
        }

        public async Task<JWTModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var user = await _userRepo.Login(request);
                if (user == null)
                {
                    return null;
                }

                var profileResponse = new ProfileResponseModel
                {
                    Email = user.Email,
                    Id = user.Id
                };

                var token = JWTExtensions.GenerateSecurityToken(profileResponse, _options);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in");
                return null;
            }
        }
    }
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
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
