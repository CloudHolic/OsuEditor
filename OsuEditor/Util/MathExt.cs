using System.Linq;

namespace OsuEditor.Util
{
    public class MathExt
    {
        public static bool CompareInts(params int[] nums)
        {
            return nums.All(cur => cur == nums[0]);
        }
    }
}
