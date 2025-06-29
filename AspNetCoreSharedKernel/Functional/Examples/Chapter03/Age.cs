﻿using System.Globalization;
using static AspNetCoreSharedKernel.Functional.F;
namespace AspNetCoreSharedKernel.Functional.Examples.Chapter03;

internal readonly struct Age
{
    internal int Value { get; }

    internal static Option<Age> Of(int value)
    {
        return IsValid(value) ? Some(new Age(value)) : (Option<Age>)None;
    }

    internal Age(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Age must be a non-negative number.");
        }
        Value = value;
    }

    internal static bool IsValid(int value) => value is >= 0 and <= 120;

    public static bool operator <(Age l, Age r) => l.Value < r.Value;
    public static bool operator >(Age l, Age r) => l.Value > r.Value;

    public static bool operator <(Age l, int r) => l < new Age(r);
    public static bool operator >(Age l, int r) => l > new Age(r);

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}

