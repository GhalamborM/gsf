﻿// ReSharper disable MustUseReturnValue

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using DNP3Adapters;
using GSF.IO;
using GSF.Reflection;

namespace DNP3ConfigGenerator;

internal class Program
{
    private static void Main()
    {
        // Create a new serialization of MasterConfiguration
        XmlSerializer serializer = new(typeof(MasterConfiguration));
        StreamWriter stream = new("device.xml");
            
        try
        {
            MasterConfiguration config = new();
            serializer.Serialize(stream, config);
        }
        finally
        {
            stream.Close();
        }

        // Restore example mapping and needed VC redistributable installer
        RestoreEmbeddedFiles();
    }

    private static void RestoreEmbeddedFiles()
    {
        Assembly executingAssembly = AssemblyInfo.ExecutingAssembly.Assembly;
        string targetPath = FilePath.AddPathSuffix(FilePath.GetAbsolutePath(""));

        // This simple file restoration assumes embedded resources to restore are in root namespace
        foreach (string name in executingAssembly.GetManifestResourceNames())
        {
            using Stream resourceStream = executingAssembly.GetManifestResourceStream(name);

            if (resourceStream is null)
                continue;

            const string SourceNamespace = $"{nameof(DNP3ConfigGenerator)}.";
            string filePath = name;

            // Remove namespace prefix from resource file name
            if (filePath.StartsWith(SourceNamespace))
                filePath = filePath.Substring(SourceNamespace.Length);

            string targetFileName = Path.Combine(targetPath, filePath);
            bool restoreFile = true;

            if (File.Exists(targetFileName))
            {
                string resourceMD5 = GetMD5HashFromStream(resourceStream);
                resourceStream.Seek(0, SeekOrigin.Begin);
                restoreFile = !resourceMD5.Equals(GetMD5HashFromFile(targetFileName));
            }

            if (!restoreFile)
                continue;

            string fileNameExtension = Path.GetExtension(targetFileName);

            if (string.Compare(fileNameExtension, ".exe", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(fileNameExtension, ".dll", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Binary file restore
                using FileStream writer = File.Create(targetFileName);
                resourceStream.CopyTo(writer);
            }
            else
            {
                // Text file restore
                byte[] buffer = new byte[resourceStream.Length];
                resourceStream.Read(buffer, 0, (int)resourceStream.Length);

                using StreamWriter writer = File.CreateText(targetFileName);
                writer.Write(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
            }
        }
    }

    private static string GetMD5HashFromFile(string fileName)
    {
        using FileStream stream = File.OpenRead(fileName);
        return GetMD5HashFromStream(stream);
    }

    private static string GetMD5HashFromStream(Stream stream)
    {
        using MD5 md5 = MD5.Create();
        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
    }
}