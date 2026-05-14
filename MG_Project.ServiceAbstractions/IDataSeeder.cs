using System;

namespace MG_Project.ServiceAbstractions
{
    public interface IDataSeeder
    {
        SeedResult Seed();
    }

    public sealed class SeedResult
    {
        public bool Success { get; init; }
        public string? Message { get; init; }

        public Guid AddrA { get; init; }
        public Guid AddrB { get; init; }

        public Guid PerA { get; init; }
        public Guid PerB { get; init; }

        public Guid EmpA { get; init; }
        public Guid EmpB { get; init; }

        public Guid DeptA { get; init; }
        public Guid DeptB { get; init; }

        public Guid FirmA { get; init; }
        public Guid FirmB { get; init; }

        public Guid Task1 { get; init; }
        public Guid Task2 { get; init; }
    }
}
