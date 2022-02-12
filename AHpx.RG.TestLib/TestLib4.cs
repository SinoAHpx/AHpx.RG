namespace AHpx.RG.TestLib;

/// <summary>
/// this is test lib4
/// </summary>
/// <typeparam name="T">this is class-level type arg</typeparam>
public class TestLib4<T> where T : new()
{
    /// <summary>
    /// This is generic prop 1
    /// </summary>
    public T Test1 { get; set; }

    //no summary
    public T Test2 { get; set; }

    /// <summary>
    /// This is test method 3, return generic type
    /// </summary>
    /// <returns></returns>
    public T Test3()
    {
        return new T();
    }

    /// <summary>
    /// test4, a method with generic parameter 
    /// </summary>
    /// <param name="p1"></param>
    public void Test4(T p1)
    {
        
    }

    /// <summary>
    /// test5, method with method-level generic arg as parameter 2
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2">this is p2</param>
    /// <typeparam name="T1">this is a method-level generic param</typeparam>
    public void Test5<T1>(T p1, T1 p2)
    {
        
    }

    /// <summary>
    /// test6, method with multiple type args
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public void Test6<T1, T2, T3>(T3 p1, T2 p2, T1 p3, T p4)
    {
        
    }
    
    /// <summary>
    /// test7, method with multiple type args, no summary
    /// </summary>
    public void Test7<T1, T2, T3>(T3 p1, T2 p2, T1 p3, T p4)
    {
        
    }
    
    /// <summary>
    /// test8, method with multiple type args, missing summary
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p4"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public void Test8<T1, T2, T3>(T3 p1, T2 p2, T1 p3, T p4)
    {
        
    }
}