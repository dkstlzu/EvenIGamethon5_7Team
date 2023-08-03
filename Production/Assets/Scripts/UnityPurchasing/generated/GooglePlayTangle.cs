// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("cZL9UT6j9/2oG1ccfbmBgXY+tE5B83BTQXx3eFv3OfeGfHBwcHRxcl8y/TK9XgCdwDxXdERZQcwsRfLdAbyXD++o2xISSgKCn7+91lWEpSHG6HNdL+QNK+sAI0CW3vOfWBS4zKGFjGMFErVn1lKO6ALABHjUlLjkO0ZCdbg3t1MUW4EtWJNzX/7KXanzcH5xQfNwe3PzcHBx0UHxfE1AeUq5J3qRy0yh9tV6Dr0m4okIUbOzgitUmx4NYeRRpqWupUSoRBigHQDaNqJPs7BFbA9kTjqobDF2TqlsKlTS3MOnqcu3wX+O7KVOVS6vdEbOTFX3nsC3Ub2/pA1LLSHHB0RVsJ2/X8ffaCtGd4cHpkW1mjbfqtam4ecjNRrYC1dn6nNycHFw");
        private static int[] order = new int[] { 8,8,9,7,6,10,6,8,12,11,12,12,12,13,14 };
        private static int key = 113;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
