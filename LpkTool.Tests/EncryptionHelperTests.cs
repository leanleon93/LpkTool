using LpkTool.Library.Helpers;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Text;

namespace LpkTool.Tests
{
    public class EncryptionHelperTests
    {

        private readonly byte[] _key = Encoding.UTF8.GetBytes("83657ea6ffa1e671375c689a2e99a598");

        [Test]
        [TestCase("This is a long test message that fullfills padding.")]
        [TestCase("AAAABBBBCCCCDDDDAAAABBBBCCCCDDDD")]
        [TestCase("ÄÖÜ")]
        [Parallelizable(ParallelScope.All)]
        public void TestAes(string testString)
        {
            var testData = Encoding.UTF8.GetBytes(testString);
            var encrypted = EncryptionHelper.AesEncrypt(testData, _key, out int _);
            var decrypted = EncryptionHelper.AesDecrypt(encrypted, _key);
            var decryptedUnpadded = new byte[testData.Length];
            Array.Copy(decrypted, decryptedUnpadded, decryptedUnpadded.Length);
            var result = Encoding.UTF8.GetString(decryptedUnpadded);
            ClassicAssert.AreEqual(testString, result);
        }

        [Test]
        [TestCase("This is a long test message that overflows padding. AAAABBBBCCCCDDDDAAAABBBBCCCCDDDDAAAABBBBCCCCDDDD")]
        [TestCase("AAAABBBBCCCCDDDDAAAABBBBCCCCDDDD")]
        [TestCase("ÄÖÜ")]
        [Parallelizable(ParallelScope.All)]
        public void TestBlowfish(string testString)
        {
            var testData = Encoding.UTF8.GetBytes(testString);
            var encrypted = EncryptionHelper.BlowfishEncrypt(testData, _key, out int _);
            var decrypted = EncryptionHelper.BlowfishDecrypt(encrypted, _key);
            var decryptedUnpadded = new byte[testData.Length];
            Array.Copy(decrypted, decryptedUnpadded, decryptedUnpadded.Length);
            var result = Encoding.UTF8.GetString(decryptedUnpadded);
            ClassicAssert.AreEqual(testString, result);
        }
    }
}