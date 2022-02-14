namespace AHpx.RG.TestLib;

/// <summary>
/// type8, nested tests
/// </summary>
/// <typeparam name="T"></typeparam>
public class TestLib8<T>
{
    /// <summary>
    /// this is a nested type
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    public class NestedTestLib8<T1,T2,T3>
    {
        /// <summary>
        /// this is a nested type in nested type
        /// </summary>
        public class NestedNestedTestLib8<T4, T5>
        {
            /// <summary>
            /// t2
            /// </summary>
            /// <param name="p1"></param>
            public void Test2(Dictionary<T5, T> p1)
            {
                
            }
        }
        
        /// <summary>
        /// t1
        /// </summary>
        /// <param name="p1"></param>
        public void Test1(Dictionary<T3, T1> p1)
        {
            
        }
    }
}