namespace SecondHandMarket;

public interface IActiveUser
{
    /// <summary>
    /// Deprecated. Was going to use this to keep track of active user before
    /// the implementation of the database. 
    /// </summary>
    /// <returns></returns>
    public bool IsActive();
}