namespace AHpx.RG.TestLib;

/// <summary>
/// Testlib7, overloads tests
/// </summary>
public class TestLib7
{
    /// <summary>
    /// t1
    /// </summary>
    public void Test1()
    {
        
    }

    /// <summary>
    /// t1. overloaded
    /// </summary>
    /// <param name="p1"></param>
    public void Test1(string p1)
    {
        
    }

    /// <summary>
    /// t1, overloaded
    /// </summary>
    /// <param name="p1"></param>
    /// <returns></returns>
    public string Test1(int p1)
    {
        return "";
    }

    /// <summary>
    /// t2, generic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Test2<T>()
    {
        
    }

    /// <summary>
    /// t1, without generic
    /// </summary>
    public void Test2()
    {
        
    }

    /// <summary>
    /// T2, multiple generic args
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public void Test2<T, T1>()
    {
        
    }

    /// <summary>
    /// t2, generic method with generic parameter
    /// </summary>
    /// <param name="p1"></param>
    /// <typeparam name="T"></typeparam>
    public void Test2<T>(T p1)
    {
        
    }
}