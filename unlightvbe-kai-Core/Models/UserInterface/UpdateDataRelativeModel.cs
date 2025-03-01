﻿using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.UserInterface;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record UpdateDataRelativeModel
    {
        public required UpdateDataRelativeType Type { get; init; }
        public UserPlayerRelativeType Player { get; init; }
        public int Value { get; init; }
        public string? Message { get; init; }
    }
}
