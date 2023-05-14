using WebApi.Entities;

namespace WebApi.Models;

public record Result(ItemTypes resultType, string message, object? objectResult = null);
