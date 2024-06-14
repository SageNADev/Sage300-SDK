// The MIT License (MIT) 
// Copyright (c) 2024 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#region Namespace

using System;
using System.Security.Cryptography;

#endregion

namespace Sage.CA.SBS.ERP.Sage300.ProxyTester.Utility
{
    /// <summary>
    /// Elliptic Curve Diffie-Hellman (ECDH) Encryption using ECDiffieHellmanCng
    /// </summary>
    public static class EllipticCurveDiffieHellman
    {
        private const ECDiffieHellmanKeyDerivationFunction KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
        private static CngAlgorithm EncryptionAlgorithm = CngAlgorithm.Sha256;

        /// <summary>
        /// Encrypt a string with ECDH
        /// </summary>
        /// <param name="clearText">Original string</param>
        /// <param name="encryptorPrivateKey">Encryptor's private key</param>
        /// <param name="decryptorPublicKey">Decryptor's public key</param>
        /// <param name="iv">Encryptor's iv</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string clearText, byte[] encryptorPrivateKey, byte[] decryptorPublicKey, out byte[] iv)
        {
            GetSymmetricKey(encryptorPrivateKey, decryptorPublicKey, out string key, out string sIV);
            iv = Convert.FromBase64String(sIV);

            return StringEncryptor.Encrypt(clearText, key, sIV);
        }

        /// <summary>
        /// Creates a new ECDH key pair and returns them
        /// </summary>
        /// <param name="privateKey">Private key</param>
        /// <param name="publicKey">Public key</param>
        public static void CreateKeyPair(out byte[] privateKey, out byte[] publicKey)
        {
            using (ECDiffieHellmanCng ecdh = new ECDiffieHellmanCng())
            {
                ecdh.KeyDerivationFunction = KeyDerivationFunction;
                ecdh.HashAlgorithm = EncryptionAlgorithm;

                privateKey = ecdh.Key.Export(CngKeyBlobFormat.EccPrivateBlob);
                publicKey = ecdh.PublicKey.ToByteArray();
            }
        }

        /// <summary>
        /// Gets the symmetric key and IV from an ECDH key
        /// </summary>
        /// <param name="privateKey">A's private key</param>
        /// <param name="publicKey">B's public key</param>
        /// <param name="key">Symmetric key</param>
        /// <param name="iv">Initialization vector</param>
        private static void GetSymmetricKey(byte[] privateKey, byte[] publicKey, out string key, out string iv)
        {
            using (ECDiffieHellmanCng ecdh = new ECDiffieHellmanCng(CngKey.Import(privateKey, CngKeyBlobFormat.EccPrivateBlob)))
            {
                ecdh.KeyDerivationFunction = KeyDerivationFunction;
                ecdh.HashAlgorithm = EncryptionAlgorithm;
                var symmetricKey = ecdh.DeriveKeyMaterial(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob));

                using (var aes = Aes.Create())
                {
                    aes.Key = symmetricKey;
                    key = Convert.ToBase64String(aes.Key);
                    iv = Convert.ToBase64String(aes.IV);
                }
            }
        }
    }
}
