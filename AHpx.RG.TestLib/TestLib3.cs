namespace AHpx.RG.TestLib;

// TestLib3, missing tests
public class TestLib3
{
    //no summary
    public string Test1()
    {
        return "";
    }

    /// <summary>
    /// test method 2, incomplete summary
    /// </summary>
    public void Test2(string p1)
    {
        
    }

    /// <summary>
    /// test3, dedicated ignore parameter summary
    /// </summary>
    /// <param name="p1">p1</param>
    /// <returns></returns>
    public string Test3(string p1, string p2)
    {
        return null;
    }

    /// <summary>
    /// test4, ignore return summary
    /// </summary>
    public string Test4()
    {
        return null;
    }

    /// <summary>
    /// test5, ignore type arg summary
    /// </summary>
    public void Test5<T>()
    {
    }

    /// <summary>
    /// test6, missing type arg summary
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    public void Test6<T1, T2>()
    {
        
    }
    
    /// <summary>
    /// test7, no type arg summary
    /// </summary>
    public void Test7<T1, T2>()
    {
        
    }

    /// <summary>
    /// test8, missing multiple type args
    /// </summary>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T4"></typeparam>
    public void Test8<T1, T2, T3, T4>()
    {
        
    }

    /// <summary>
    /// test9, missing type arg summary and corresponding param summary
    /// </summary>
    /// <param name="p2"></param>
    /// <typeparam name="T2"></typeparam>
    public void Test9<T1, T2>(T1 p1, T2 p2)
    {
        
    }

    /// <summary>
    /// test10, missing used type args
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public void Test10<T1,T2,T3,T4>(T4 p1, T3 p2)
    {
        var a = new object();
    }

    /// <summary>
    /// test11, missing unused type args
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    public void Test11<T1,T2,T3,T4>(T4 p1, T3 p2)
    {
        
    }
}