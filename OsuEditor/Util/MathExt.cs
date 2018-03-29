using System.Linq;

namespace OsuEditor.Util
{
    public class MathExt
    {
        public static bool CompareInts(params int[] nums)
        {
            return nums.All(cur => cur == nums[0]);
        }

        public static int Clamp(int value, int min, int max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static double Clamp(double value, double min, double max)
        {
            return value < min ? min : value > max ? max : value;
        }
    }
}
