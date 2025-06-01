namespace AspNetCoreSharedKernel;

public interface IDomainEvent { }

public abstract record DomainEventBase(DateTime OccurredOnUtc) : IDomainEvent;
