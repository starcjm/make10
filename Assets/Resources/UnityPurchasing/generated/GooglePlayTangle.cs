#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("UnnMxDespEwiaOGiEoc9ILAZtb7S2MtFYzh1sIigcsZj9e7gvMRqjT2PDC89AAsEJ4tFi/oADAwMCA0Ono+uTE8Zf/pCreFNRCrv/fgd+W3X4dmsWWOa8rLRnLmctcMnLl6CcM42V3uXDZ04rVpQaxYkiW7crjlXsbjxZjhiIpKxa1RwBTC3c2BLHyz5NPWnUYJsUFJafUYxHr9h6H+jZzO4Sdjd0tikoVZ4UMDfesKJRXXt/c4z6vx33g6vgVzGRE1W61hQP5yPDAINPY8MBw+PDAwNttLbsX3Q3WwvVgIVpJlqYHKqlJziSqezo/4l5ENHcF1tFBx5a2fidHHMBCPfnDDfhzXMrGM+feASoSe2wXSHBuvwBP7K0jNpbf63vg8ODA0M");
        private static int[] order = new int[] { 3,8,3,6,10,7,10,13,11,9,11,13,13,13,14 };
        private static int key = 13;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
