using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Heus.Uow
{
    public interface IUnitOfWork
    {
        Guid Id { get; }
        
        UnitOfWorkOptions Options { get; }

        IUnitOfWork Outer { get; }

        string ReservationName { get; }

        void SetOuter([CanBeNull] IUnitOfWork outer);

        void Initialize([NotNull] UnitOfWorkOptions options);

        void Reserve([NotNull] string reservationName);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task CompleteAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);

    }
}