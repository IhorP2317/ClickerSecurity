namespace Clicker.Security.API.Filters;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;


public class ValidateModelFilter : IEndpointFilter
{
    private readonly ILogger<ValidateModelFilter> _logger;

    public ValidateModelFilter(ILogger<ValidateModelFilter> logger)
    {
        _logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            foreach (var argument in context.Arguments)
            {
                if (argument != null)
                {
                    var validationContext = new ValidationContext(argument);
                    var validationResults = new List<ValidationResult>();
                    if (!Validator.TryValidateObject(argument, validationContext, validationResults, true))
                    {
                        var errors = new Dictionary<string, string[]>();
                        foreach (var validationResult in validationResults)
                        {
                            foreach (var memberName in validationResult.MemberNames)
                            {
                                if (!errors.ContainsKey(memberName))
                                {
                                    errors[memberName] = new[] { validationResult.ErrorMessage };
                                }
                                else
                                {
                                    var errorList = errors[memberName].ToList();
                                    errorList.Add(validationResult.ErrorMessage);
                                    errors[memberName] = errorList.ToArray();
                                }
                            }
                        }

                        _logger.LogWarning("Validation failed: {@Errors}", errors);
                        return Results.ValidationProblem(errors);
                    }
                }
            }

            return await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during validation.");
            throw;
        }
    }
}
