namespace AspNetCoreSharedKernel;

public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : DomainEventBase;
