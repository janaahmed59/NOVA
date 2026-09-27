using FluentValidation;
using MediatR;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next(cancellationToken);

            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count == 0)
                return await next(cancellationToken);

            // Convert FluentValidation errors to domain Error list
            var errors = failures
                .Select(f => Error.Validation(
                    f.ErrorCode,
                    f.ErrorMessage,
                    f.PropertyName))
                .ToList();

            // Check if TResponse is Result<T> and return failure
            var responseType = typeof(TResponse);
            if (responseType.IsGenericType &&
                responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var failureMethod = responseType.GetMethod(
                    nameof(Result<object>.Failure),
                    [typeof(IReadOnlyList<Error>)]);

                return (TResponse)failureMethod!.Invoke(
                    null, [errors.AsReadOnly()])!;
            }

            throw new ValidationException(failures);
        }
    }
}
