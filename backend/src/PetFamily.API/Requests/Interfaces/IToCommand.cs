namespace PetFamily.API.Requests.Interfaces;

public interface IToCommand<TReturn, TParameter>
{
    TReturn ToCommand(TParameter parameter);
}

public interface IToCommand<TReturn>
{
    TReturn ToCommand();
}