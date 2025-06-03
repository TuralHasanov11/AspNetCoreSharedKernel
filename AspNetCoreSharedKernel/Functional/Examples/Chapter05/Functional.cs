using static AspNetCoreSharedKernel.Functional.F;
namespace AspNetCoreSharedKernel.Functional.Examples.Chapter05;

public class Chapter5_TransfersController
{
    private readonly IValidator<MakeTransfer> validator;
    private readonly IRepository<AccountState> accounts;
    private readonly ISwiftService swift;

    public void MakeTransfer(MakeTransfer transfer)
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

public class AccountState
{
    public decimal Balance { get; }
    public AccountState(decimal balance) { Balance = balance; }
}

public static class Account
{
    public static Option<AccountState> Debit
       (this AccountState acc, decimal amount)
       => (acc.Balance < amount)
          ? F.None
          : Some(new AccountState(acc.Balance - amount));
}


// dependencies

public interface IRepository<T>
{
    Option<T> Get(Guid id);
    void Save(Guid id, T t);
}

internal interface ISwiftService
{
    void Wire(MakeTransfer transfer, AccountState account);
}

public interface IValidator<T>
{
    bool IsValid(T t);
}

public abstract class Command
{
    public DateTime Timestamp { get; set; }

    public T WithTimestamp<T>(DateTime timestamp)
       where T : Command
    {
        T result = (T)MemberwiseClone();
        result.Timestamp = timestamp;
        return result;
    }
}

public class MakeTransfer : Command
{
    public Guid DebitedAccountId { get; set; }

    public string Beneficiary { get; set; }
    public string Iban { get; set; }
    public string Bic { get; set; }

    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Reference { get; set; }
}
