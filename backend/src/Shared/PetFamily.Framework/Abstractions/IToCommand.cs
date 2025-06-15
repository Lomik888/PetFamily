namespace PetFamily.Framework.Abstractions;

public interface IToCommand<TReturn, TParameter>
{
    TReturn ToCommand(TParameter parameter);
}

public interface IToCommand<TReturn>
{
    TReturn ToCommand();
}

public interface IToCommand<TReturn, TParameter, YParameter>
{
    TReturn ToCommand(TParameter xParameter, YParameter yParameter);
}