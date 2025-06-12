using static AspNetCoreSharedKernel.Functional.F;
namespace AspNetCoreSharedKernel.Functional.Examples.Chapter05;

internal class Chapter5_TransfersController
{
    private readonly IValidator<MakeTransfer> validator;
    private readonly IRepository<AccountState> accounts;
    private readonly ISwiftService swift;

    internal void MakeTransfer(MakeTransfer transfer)
       => Some(transfer)
          .Map(Normalize)
          .Where(validator.IsValid)
          .ForEach(Book);

    private void Book(MakeTransfer transfer)
       => accounts.Get(transfer.DebitedAccountId)
          .Bind(account => account.Debit(transfer.Amount))
          .ForEach(newState =>
          {
              accounts.Save(transfer.DebitedAccountId, newState);
              swift.Wire(transfer, newState);
          });

    private MakeTransfer Normalize(MakeTransfer request)
       => request; // remove whitespace, toUpper, etc.
}


// domain model

internal class AccountState
{
    internal decimal Balance { get; }
    internal AccountState(decimal balance) { Balance = balance; }
}

internal static class Account
{
    internal static Option<AccountState> Debit
       (this AccountState acc, decimal amount)
       => (acc.Balance < amount)
          ? F.None
          : Some(new AccountState(acc.Balance - amount));
}


// dependencies

internal interface IRepository<T>
{
    Option<T> Get(Guid id);
    void Save(Guid id, T t);
}

internal interface ISwiftService
{
    void Wire(MakeTransfer transfer, AccountState account);
}

internal interface IValidator<T>
{
    bool IsValid(T t);
}

internal abstract class Command
{
    internal DateTime Timestamp { get; set; }

    internal T WithTimestamp<T>(DateTime timestamp)
       where T : Command
    {
        T result = (T)MemberwiseClone();
        result.Timestamp = timestamp;
        return result;
    }
}

internal class MakeTransfer : Command
{
    internal Guid DebitedAccountId { get; set; }

    internal string Beneficiary { get; set; }
    internal string Iban { get; set; }
    internal string Bic { get; set; }

    internal DateTime Date { get; set; }
    internal decimal Amount { get; set; }
    internal string Reference { get; set; }
}
