﻿namespace Project.Manager.Domain.Errors;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}