using FluentValidation;
using MediatR;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Validation
{
    public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest> validator) : IPipelineBehavior<TRequest, ApiResponse<TResponse>> where TRequest : notnull
    {
        public async Task<ApiResponse<TResponse>> Handle(TRequest request, RequestHandlerDelegate<ApiResponse<TResponse>> next, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid) return ApiResponse<TResponse>.Fail(validationResult.Errors.Select(x => x.ErrorMessage));

            return await next();
        }
    }
}
