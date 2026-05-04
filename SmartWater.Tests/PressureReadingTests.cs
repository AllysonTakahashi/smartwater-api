using FluentAssertions;
using SmartWater.Domain.Entities;

namespace SmartWater.Tests;

public class PressureReadingTests
{
    [Fact]
    public void Should_Create_Alert_When_Variation_GreaterOrEqual_5_And_InRange()
    {
        var reading = new PressureReading("A1", 20);

        var result = reading.HasCriticalVariation(15); // variation = 5, value in range

        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Not_Create_Alert_When_Variation_Less_Than_5()
    {
        var reading = new PressureReading("A1", 18);

        var result = reading.HasCriticalVariation(15); // variation = 3, value in range

        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Not_Create_Alert_When_Pressure_Outside_Operational_Range()
    {
        var reading = new PressureReading("A1", 10); // 10 < 15 mca — out of range

        var result = reading.HasCriticalVariation(5); // variation = 5, but out of range

        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Reject_Invalid_Pressure()
    {
        var act = () => new HydrantInspection(1, 0, 10, "Inspector");

        act.Should().Throw<ArgumentException>()
           .WithMessage("*Pressure*");
    }
}
