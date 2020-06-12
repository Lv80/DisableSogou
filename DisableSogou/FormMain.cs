using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MFGLib;

namespace DisableSogou
{
	public partial class FormMain : Form
	{
		bool m_autoMode = false;
		string m_sogouDir = null;

		public FormMain(bool autoMode)
		{
			InitializeComponent();
			m_autoMode = autoMode;

			RegistryHelper reg = new RegistryHelper();
			reg.Open("Abin", "DisableSogou");
			m_sogouDir = reg.ReadString("Sogou Directory", "");
			reg.Close();			
		}		

		bool RemoveSogouJunks()
		{
			try
			{
				Utils.RemoveSogouJunks(m_sogouDir);

				if (!m_autoMode)
				{
					MessageBox.Show(this, "程序已成功锁定目标，现将转入后台运行。", Application.ProductName);
				}

				this.Hide();
				this.ShowInTaskbar = false;
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}			
		}		

		private void Form1_Load(object sender, EventArgs e)
		{
			txtPath.Text = m_sogouDir;
			if (m_autoMode)
			{
				RemoveSogouJunks();
			}			
		}		

		private void btnStart_Click(object sender, EventArgs e)
		{
			if (RemoveSogouJunks())
			{
				RegistryHelper.AddAutoStartApp(Application.ProductName, Application.ExecutablePath, "-auto");
			}
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFolderDialog dlg = new OpenFolderDialog();
			dlg.SelectedPath = m_sogouDir;						
			if (dlg.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			if (string.IsNullOrEmpty(dlg.SelectedPath))
			{
				return;
			}

			if (!Utils.IsSougouFolder(dlg.SelectedPath))
			{
				MessageBox.Show(this, "请正确选择搜狗输入法安装目录（SogouPY.ime所在目录）。");
				return;
			}

			m_sogouDir = dlg.SelectedPath;
			txtPath.Text = m_sogouDir;

			RegistryHelper reg = new RegistryHelper();
			reg.Open("Abin", "DisableSogou", true);
			reg.WriteString("Sogou Directory", m_sogouDir);
			reg.Close();
		}
	}
}
