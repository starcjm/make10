#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("XN/R3u5c39TcXN/f3mUBCGKuAw43kJSjjr7Hz6q4tDGnoh/X8AxP4wELGJaw66ZjW3OhFbAmPTNvF7leHeWEqETeTut+iYO4xfdavQ996oTuXN/87tPY1/RYllgp09/f39ve3YGqHxfkf3ef8bsyccFU7vNjymZtYmsiteux8UFiuIej1uNkoLOYzP8uHeA5L6QN3XxSjxWXnoU4i4PsTwQyCn+KsEkhYQJPak9mEPT9jVGjTVx9n5zKrCmRfjKel/k8LivOKr4q5yZ0glG/g4GJrpXizWyyO6xwtL/8hdHGd0q5s6F5R08xmXRgcC32DFTmH3+w7a4zwXL0ZRKnVNU4I9fga5oLDgELd3KFq4MTDKkRWpamPi0ZAeC6vi1kbdzd397f");
        private static int[] order = new int[] { 1,2,8,7,8,7,8,9,10,10,13,13,12,13,14 };
        private static int key = 222;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
