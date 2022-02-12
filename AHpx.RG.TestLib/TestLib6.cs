namespace AHpx.RG.TestLib;

/// <summary>
/// this is testlib6, interface inheritance test
/// </summary>
/// <typeparam name="TTT"></typeparam>
public class TestLib6<TTT> : TestInterface6<string>
{
    /// <summary>
    /// class level test1
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException">this is an exception summary</exception>
    public string Test1()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <typeparam name="T2"></typeparam>
    /// <exception cref="NotImplementedException"></exception>
    public void Test2<T2>(string p1, T2 p2)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <exception cref="NotImplementedException"></exception>
    public void Test3<T1, T2>(T2 p1, string p2, T1 p3)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// this is a summary for interface
/// </summary>
/// <typeparam name="T"></typeparam>
public interface TestInterface6<T>
{
    /// <summary>
    /// this is interface test1
    /// </summary>
    /// <returns></returns>
    T Test1();
    
    /// <summary>
    /// this is interface test2
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <typeparam name="T2"></typeparam>
    void Test2<T2>(T p1, T2 p2);
    
    /// <summary>
    /// this is interface test3
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    void Test3<T1, T2>(T2 p1, T p2, T1 p3);
}