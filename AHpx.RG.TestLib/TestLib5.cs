namespace AHpx.RG.TestLib;

/// <summary>
/// testlib 5, multiple class-level type args
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
public class TestLib5<T1, T2, T3>
{
    public Dictionary<T1, T2> Test1 { get; set; }

    /// <summary>
    /// test method3, all generic param are from class
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    public void Test2(T3 p1, T2 p2, T3 p3)
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <typeparam name="T"></typeparam>
    public void Test3<T>(T3 p1, T2 p2, T p3)
    {
        
    }
}