﻿namespace PetFamily.Framework.Abstractions;

public interface IToQuery<TReturn, TParameter, YParameter>
{
    TReturn ToQuery(TParameter xParameter, YParameter yParameter);
}

public interface IToQuery<TReturn, TParameter>
{
    TReturn ToQuery(TParameter parameter);
}

public interface IToQuery<TReturn>
{
    TReturn ToQuery();
}