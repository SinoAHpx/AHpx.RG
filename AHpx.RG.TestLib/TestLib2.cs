namespace AHpx.RG.TestLib;

/// <summary>
/// This is test2, generic tests
/// </summary>
public class TestLib2
{
    /// <summary>
    /// This is generic test1
    /// </summary>
    /// <typeparam name="T">this is type argument 1</typeparam>
    public void Test1<T>()
    {
        //omit
    }

    /// <summary>
    /// This is generic method 2
    /// </summary>
    /// <typeparam name="T1">this is type argument 1</typeparam>
    /// <typeparam name="T2">this is type argument 2</typeparam>
    public void Test2<T1, T2>()
    {
        
    }

    /// <summary>
    /// This is generic method 3
    /// </summary>
    /// <param name="p1">this is param1</param>
    /// <typeparam name="T">this is type arg 1</typeparam>
    public void Test3<T>(T p1)
    {
        
    }

    /// <summary>
    /// this is generic method 4
    /// </summary>
    /// <param name="p1">p1</param>
    /// <param name="p2"></param>
    /// <typeparam name="T1">g a 1</typeparam>
    /// <typeparam name="T2">g a 2</typeparam>
    public void Test4<T1, T2>(T1 p1, T2 p2)
    {
        
    }

    /// <summary>
    /// this is generic method 5
    /// </summary>
    /// <param name="p1">g p 1</param>
    /// <typeparam name="T">g a 1</typeparam>
    /// <returns>this is return value</returns>
    public List<T> Test5<T>(T p1)
    {
        return null;
    }

    /// <summary>
    /// this is generic method 6
    /// </summary>
    /// <param name="t1"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public Dictionary<T1, T2> Test6<T1, T2>(T1 t1)
    {
        return null;
    }

    /// <summary>
    /// this is multiple type arguments method 7
    /// </summary>
    /// <param name="p1">p1</param>
    /// <param name="p2"></param>
    /// <param name="p3">p3</param>
    /// <param name="p4"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3">t3</typeparam>
    /// <typeparam name="T4"></typeparam>
    public void Test7<T1, T3, T2, T4>(T1 p1, T2 p2, T3 p3, T4 p4)
    {
        
    }

    /// <summary>
    /// This is multiple type arguments method 8
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <returns>this is a complicated return value</returns>
    public Dictionary<T1, Dictionary<T2, Dictionary<T3, T4>>> Test8<T1, T2, T3, T4>(T1 p1, T2 p2, T3 p3, T4 p4)
    {
        return null;
    }
    
    /// <summary>
    /// This is multiple type arguments method 8
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <returns>this is a complicated return value</returns>
    public Dictionary<T1, Dictionary<T2, Dictionary<T3, T4>>> Test9<T1, T2, T3, T4>(T4 p1, T3 p2, T2 p3, T1 p4)
    {
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p1"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public void Test10<T1, T2>(Dictionary<T1, T2> p1)
    {
        //todo: dynamic generic typed dictionary issue
    }
}