using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DisableSogou
{
	class SougouJunk
	{
		public string Process { get; set; }
		public string File { get; set; }

		public SougouJunk(string process, string file)
		{
			Process = process;
			File = file;
		}
	}

	static class Utils
	{
		public static readonly SougouJunk[] JUNK_LIST =
		{
			//new SougouJunk("SGTool", "SGTool.exe"),
			new SougouJunk("SogouCloud", "SogouCloud.exe"),
			new SougouJunk("SohuNews", "SohuNews.exe"),
		};

		public static bool IsSougouFolder(string sogouDir)
		{
			return !string.IsNullOrEmpty(sogouDir) && File.Exists(sogouDir + "\\SogouPY.ime");
		}

		public static void RemoveSogouJunks(string sogouDir)
		{
			if (!IsSougouFolder(sogouDir))
			{
				throw new Exception("请正确选择搜狗输入法安装目录。");
			}

			foreach (SougouJunk junk in JUNK_LIST)
			{
				if (!KillProcess(junk.Process))
				{
					throw new Exception("无法杀死进程：" + junk.Process);
				}

				string filePath = sogouDir + "\\" + junk.File;
				if (File.Exists(filePath))
				{
					FileInfo fi = new FileInfo(filePath);
					if (fi.Length > 0)
					{
						string backupDir = sogouDir + "\\BackupFiles";
						try
						{
							Directory.CreateDirectory(backupDir);
							File.Copy(filePath, backupDir + "\\" + junk.File, false);
						}
						catch
						{
						}
					}

					try
					{
						File.Delete(filePath);
					}
					catch
					{
						throw new Exception("无法删除文件：" + filePath);
					}
				}

				try
				{
					File.OpenWrite(filePath);
				}
				catch
				{
					throw new Exception("无法占用文件：" + filePath);
				}
			}					
		}

		public static bool KillProcess(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return true;
			}

			try
			{
				foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcessesByName(name))
				{
					proc.Kill();
				}
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}
