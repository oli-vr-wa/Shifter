using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Core.Entities.Common;

public class ServiceResult<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string Error { get; }

    /// <summary>
    /// Private constructor to enforce the use of factory methods for creating instances of Result.
    /// </summary>
    /// <param name="success">Indicates whether the operation was successful.</param>
    /// <param name="value">The value associated with the result (if successful).</param>
    /// <param name="error">The error message (if failed).</param>
    /// <exception cref="InvalidOperationException"></exception>
    protected ServiceResult(bool success, T? value, string error)
    {
        if (success && error != string.Empty)
            throw new InvalidOperationException("Successful result cannot have an error message.");

        IsSuccess = success;
        Value = value;
        Error = error;
    }

    // Factory methods for creating success and failure results
    public static ServiceResult<T> Success(T value) => new ServiceResult<T>(true, value, string.Empty);
    public static ServiceResult<T> Failure(string error) => new ServiceResult<T>(false, default, error);
}
