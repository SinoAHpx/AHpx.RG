namespace AHpx.RG.TestLib;

/// <summary>
/// This is test1, basic
/// </summary>
public class TestLib1
{
    
    /// <param name="p1">this is a test parameter 1</param>
    public void Test1(string p1)
    {
        //omit
    }

    /// <summary>
    /// This is a test method 2
    /// </summary>
    /// <param name="p1">This is a test p1</param>
    public void Test2(int p1)
    {
        
    }

    /// <summary>
    /// This is a test method 3
    /// </summary>
    /// <param name="p1">this is test p3</param>
    /// <returns>this is a return value test 3</returns>
    public string Test3(object p1)
    {
        return "";
    }

    /// <summary>
    /// test4, multiple parameters
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    /// <param name="p5"></param>
    /// <param name="p6"></param>
    public void Test4(string p1, string p2, string p3, int p4, int[] p5, params string[] p6)
    {
        
    }

    /// <summary>
    /// test 5
    /// </summary>
    /// <param name="p8"></param>
    /// <param name="p9"></param>
    /// <param name="p10"></param>
    /// <param name="p11"></param>
    /// <param name="p12"></param>
    public void Test5(TestLib3 p8, Func<string> p9, Func<string, string> p10, Action<int> p11, IEnumerable<int> p12)
    {
        
    }

    /// <summary>
    /// test6
    /// </summary>
    /// <param name="p1"></param>
    public void Test6(Dictionary<int, Dictionary<int, string>> p1)
    {
        
    }

    /// <summary>
    /// t7, no parameters
    /// </summary>
    public void Test7()
    {
        
    }

    /// <summary>
    /// tp1
    /// </summary>
    public string TestProperty1 { get; set; }

    /// <summary>
    /// tp2
    /// </summary>
    public int TestProperty2 { get; set; }

    /// <summary>
    /// tp3
    /// </summary>
    public Dictionary<int, string> TestProperty3 { get; set; }

    /// <summary>
    /// tp4
    /// </summary>
    public string TestField1;

    /// <summary>
    /// tp5
    /// </summary>
    public Dictionary<int, string> TestField2;

    /// <summary>
    /// this is a constructor
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public TestLib1(string p1, Dictionary<int, string> p2)
    {
    }

    /// <summary>
    /// this is test delegate
    /// </summary>
    public delegate void TestDelegate1(string p1);
    
    /// <summary>
    /// this is a test event
    /// </summary>
    public event EventHandler<string> TestEvent1;
}