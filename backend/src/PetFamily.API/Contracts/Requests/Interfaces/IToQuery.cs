namespace PetFamily.API.Contracts.Requests.Interfaces;

public interface IToQuery<TReturn, TParameter, YParameter>
{
    TReturn ToQuery(TParameter xParameter, YParameter yParameter);
}

public interface IToQuery<TReturn>
{
    TReturn ToQuery();
}