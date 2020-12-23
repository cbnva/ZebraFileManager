using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZebraFileManager.Zebra
{
    public abstract class Printer : IDisposable
    {
        public abstract bool Connect();
        public abstract string RunCommand(string command, bool response = true);
        public abstract byte[] RunCommand(byte[] command, bool response = true);

        public abstract bool Connected { get; }

        FileSystem lastfs;
        public FileSystem LastFileSystemResults => lastfs;

        public virtual FileSystem GetFileSystem(string rootDirectory = null, bool updateLastResults = true)
        {
            string command = null;
            bool reset = string.IsNullOrEmpty(rootDirectory);
            if (reset)
            {
                command = @"! U1 do ""file.dir"" """"
";
            }
            else
            {
                command = $@"! U1 do ""file.dir"" ""{rootDirectory[0]}:""
";
            }
            var value = this.RunCommand(command);

            var filesRegex = new Regex(@"^\*\s+(?<FileName>\S+)\s+(?<Size>\d+)(?:\s+(?<Attribute>\w))*", RegexOptions.Compiled | RegexOptions.Multiline);
            var drivesRegex = new Regex(@"^-\s*(?<free>\d+) bytes free (?<letter>[A-Za-z]):\s+(?<name>.+)$", RegexOptions.Compiled | RegexOptions.Multiline);
            var newFS = (updateLastResults && lastfs != null) ? lastfs : new FileSystem { Drives = new List<Drive>(), FileSystemEntries = new List<File>() };
            var activeDrives = new List<Drive>();
            var activeFiles = new List<File>();

            foreach (Match m in drivesRegex.Matches(value))
            {
                var drive = newFS.Drives.FirstOrDefault(x => x.Letter == m.Groups["letter"].Value) ?? new Drive
                {
                    Letter = m.Groups["letter"].Value,
                };

                drive.Free = int.Parse(m.Groups["free"].Value);
                drive.Name = m.Groups["name"].Value.Trim();
                if (!newFS.Drives.Contains(drive))
                    newFS.Drives.Add(drive);

                activeDrives.Add(drive);
            }

            foreach (Match m in filesRegex.Matches(value))
            {
                var file = newFS.FileSystemEntries.FirstOrDefault(x => x.Path == m.Groups["FileName"].Value.Trim()) ?? new File()
                {
                    Path = m.Groups["FileName"].Value.Trim(),
                };
                file.Size = int.Parse(m.Groups["Size"].Value);
                file.Attributes = new List<string>();
                foreach (Capture c in m.Groups["Attribute"].Captures)
                {
                    file.Attributes.Add(c.Value);
                }

                var parentDrive = newFS.Drives.FirstOrDefault(x => x.Letter[0] == file.Path[0]) ?? new Drive
                {
                    Letter = file.Path[0].ToString(),
                    Name = "",
                };

                if (!newFS.Drives.Contains(parentDrive))
                    newFS.Drives.Add(parentDrive);
                if (!activeDrives.Contains(parentDrive))
                    activeDrives.Add(parentDrive);

                if (!newFS.FileSystemEntries.Contains(file))
                    newFS.FileSystemEntries.Add(file);

                activeFiles.Add(file);
            }



            if (updateLastResults && string.IsNullOrEmpty(rootDirectory))
            {
                // All files have been retrieved, so delete anything missing from the latest info.
                newFS.Drives.RemoveAll(x => !activeDrives.Contains(x));
                newFS.FileSystemEntries.RemoveAll(x => !activeFiles.Contains(x));
                foreach (var drive in newFS.Drives)
                    drive.Used = newFS.FileSystemEntries.Where(x => x.Path.ToUpper()[0] == drive.Letter.ToUpper()[0]).Sum(x => x.Size);

            }
            else if (updateLastResults)
            {
                newFS.FileSystemEntries.RemoveAll(x => !activeFiles.Contains(x) && x.Path.ToUpper()[0] == rootDirectory.ToUpper()[0]);
                foreach (var drive in newFS.Drives.Where(x => x.Letter.ToUpper()[0] == rootDirectory.ToUpper()[0]))
                    drive.Used = newFS.FileSystemEntries.Where(x => x.Path.ToUpper()[0] == drive.Letter.ToUpper()[0]).Sum(x => x.Size);
            }
            else
            {
                foreach (var drive in newFS.Drives)
                    drive.Used = newFS.FileSystemEntries.Where(x => x.Path.ToUpper()[0] == drive.Letter.ToUpper()[0]).Sum(x => x.Size);
            }



            if (updateLastResults)
            {
                lastfs = newFS;
            }
            return newFS;
        }

        public virtual byte[] GetFileContents(string path)
        {
            var result = this.RunCommand(Encoding.UTF8.GetBytes($@"! U1 do ""file.type"" ""{path}""
"));

            return result;
        }

        public virtual void SetFileContents(string path, byte[] contents)
        {
            if (Regex.IsMatch(path, "\\s"))
            {
                throw new ArgumentException("Path on Zebra may not contain spaces.");
            }
            var header = $@"! CISDFCRC16
0000
{path}
{contents.Length.ToString("X8")}
0000
";
            var hdrBytes = Encoding.UTF8.GetBytes(header);
            var command = new byte[hdrBytes.Length + contents.Length];
            hdrBytes.CopyTo(command, 0);
            contents.CopyTo(command, hdrBytes.Length);
            var result = this.RunCommand(command, false);

        }

        public virtual void DeleteFile(string path)
        {
            var result = this.RunCommand($@"{{}}{{""file.delete"":""{path}""}}");
            var o = (JContainer)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            var value = o["file.delete"].Value<string>();
        }

        public virtual void RefreshFileSystem(string rootDirectory = null, FileSystem fs = null)
        {
            if (fs == null && LastFileSystemResults != null)
            {
                fs = LastFileSystemResults;
            }
            else
            {
                GetFileSystem(rootDirectory);
            }
        }

        public virtual void CopyFile(string source, string destination)
        {

        }

        public virtual void RenameFile(string oldName, string newName)
        {
            var result = this.RunCommand($@"{{}}{{""file.rename"":""{oldName} {newName}""}}");
            var o = (JContainer)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            var value = o["file.rename"].Value<string>();

        }

        public virtual List<Setting> GetSettings()
        {
            var results = RunCommand("{}{\"allconfig\":null}");
            var parsed = Newtonsoft.Json.JsonConvert.DeserializeObject(results) as Newtonsoft.Json.Linq.JObject;
            var settings = new List<Setting>();
            foreach (JProperty token in parsed["allconfig"]?.Children())
            {
                var setting = token.First.ToObject<Setting>();
                setting.Name = token.Name;
                settings.Add(setting);
            }
            return settings;
        }

        public virtual string GetSettingSGD(string settingName)
        {
            var results = RunCommand($"! U1 getvar \"{settingName}\"\r\n");

            return results;

        }

        public virtual string SetSettingSGD(string settingName, string value)
        {
            var results = RunCommand($"! U1 setvar \"{settingName}\" \"{value}\"\r\n", false);

            return results;

        }

        //public virtual 

        public virtual void FactoryReset()
        {
            // Delete files on E drive
            var fs = GetFileSystem("E:", false);
            foreach (var item in fs.FileSystemEntries)
            {
                DeleteFile(item.Path);
            }

            // Ensure that we're not in Line Print Mode, which disables JSON
            var languageMode = GetSettingSGD("device.languages");
            if (languageMode == "\"line_print\"")
            {
                SetSettingSGD("device.languages", "zpl");
            }


            // Generate a script to change all settings back to defaults, excluding time and odometer settings.
            var settings = GetSettings().Where(x => x.Access == SettingAccess.RW);

            var sb = new StringBuilder("! U ");
            foreach (var setting in settings)
            {
                if (setting.Name.StartsWith("rtc.") || setting.Name.StartsWith("odometer."))
                    continue;
                if (setting.Value != setting.Default)
                    sb.AppendLine($"setvar \"{setting.Name}\" \"{setting.Default}\"");
            }
            sb.AppendLine("END ");

            var results = RunCommand("! U1 do \"device.restore_defaults\" \"power\"\r\n", false); 
            results = RunCommand(sb.ToString(), false);


            results = RunCommand("! U1 do \"device.reset\" \"\" \r\n", false);


        }

        public abstract void Dispose();
    }
}
