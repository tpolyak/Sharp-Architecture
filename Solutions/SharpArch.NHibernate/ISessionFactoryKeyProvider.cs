﻿namespace SharpArch.NHibernate
{
  public interface ISessionFactoryKeyProvider
  {
    /// <summary>
    /// Gets the session factory key.
    /// </summary>
    /// <returns></returns>
    string GetKey();

    /// <summary>
    /// Gets the session factory key.
    /// </summary>
    /// <param name="anObject">An optional object that may have an attribute used to determine the session factory key.</param>
    /// <returns></returns>
    string GetKeyFrom(object anObject);
  }
}